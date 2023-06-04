using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvaluableGradient : IEvaluable
{
    [JsonProperty]
    private float minPos, maxPos;
    private ColorVec minColor, maxColor;
    private int count;
    private List<ColorVec> colors;
    private List<float> positions;

    //brightness mutliplier
    public EvaluableColorTable.InterpolationType interType = EvaluableColorTable.InterpolationType.linear;

    public EvaluableGradient()
    {
        colors = new List<ColorVec>();
        positions = new List<float>();
    }

    public void SetKeyColor(int index, ColorVec color)
    {
        colors[index] = color;
    }

    public void SetKeyPositon(int index, float position)
    {
        positions[index] = position;
        UpdateMinMax();
    }

    public void AddKey(float position, ColorVec color)
    {
        count++;
        positions.Add(position);
        colors.Add(color);
        UpdateMinMax();
    }

    public void RemoveKey(int index)
    {
        count--;
        positions.RemoveAt(index);
        colors.RemoveAt(index);
        UpdateMinMax();
    }

    public int GetkeyAmt()
    {
        return colors.Count;
    }

    private void UpdateMinMax()
    {
        minPos = positions.Min();
        minColor = colors[positions.IndexOf(minPos)];
        maxPos = positions.Max();
        maxColor = colors[positions.IndexOf(maxPos)];
    }

    private int GetLeftIndex(float position)
    {
        int closestIndex = -1;
        float bestDelta = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            float delta = position - positions[i];
            if (positions[i] < position && (delta < bestDelta))
            {
                closestIndex = i;
                bestDelta = delta;
            }
        }
        return closestIndex;
    }

    private int GetRightIndex(float position)
    {
        int closestIndex = -1;
        float bestDelta = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            float delta = positions[i] - position;
            if (positions[i] > position && (delta < bestDelta))
            {
                closestIndex = i;
                bestDelta = delta;
            }
        }
        return closestIndex;
    }

    private ColorVec Interpolate(float x)
    {
        if (count == 1)
        {
            return colors[0];
        }
        if(x < minPos)
        {
            return minColor;
        }
        if(x > maxPos)
        {
            return maxColor;
        }

        // ((light I'm at)/last light)) x (Number of keys)
        // casted to int = lowest key

        if (x == 1)
        {
            x = 0.99999f;
        }
        else if (x == 0)
        {
            x = 0.00001f;
        }

        int keyIndex1 = GetLeftIndex(x);
        ColorVec clr1 = colors[keyIndex1];
        int keyIndex2 = GetRightIndex(x);
        ColorVec clr2 = colors[keyIndex2];

        float keyPercent1 = (float)keyIndex1 / (count - 1);
        float keyPercent2 = (float)keyIndex2 / (count - 1);

        float g = (x - keyPercent1) / (keyPercent2 - keyPercent1);
        switch (interType)
        {
            case EvaluableColorTable.InterpolationType.linear:
                return ColorOperations.lerp(clr1, clr2, g);
            case EvaluableColorTable.InterpolationType.closest:
                if (g < 0.5)
                {
                    return clr1;
                }
                else
                {
                    return clr2;
                }

            default:
                Debug.Log("Error=====Invalid Interpolation Type======Error");
                return new ColorVec(0, 0, 255);
        }
    }

    public ColorVec EvaluateColor(float vector)
    {
        return Interpolate(vector);
    }

    public float EvaluateValue(float vector)
    {
        return (float)EvaluateColor(vector);
    }

    public object GetCopy()
    {
        EvaluableGradient gradient = new EvaluableGradient();
        for (int i = 0; i < count; i++)
        {
            gradient.AddKey(positions[i], colors[i]);
        }
        return gradient;
    }

    public int GetResolution()
    {
        return count;
    }

}
