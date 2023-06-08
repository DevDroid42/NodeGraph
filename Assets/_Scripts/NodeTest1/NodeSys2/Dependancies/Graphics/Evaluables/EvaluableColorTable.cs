
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using UnityEngine;

public class EvaluableColorTable : IEvaluable
{
    [JsonProperty]
    private List<ColorVec> keys = new List<ColorVec>();
    //brightness mutliplier. 

    [JsonConverter(typeof(StringEnumConverter))]
    public enum InterpolationType
    {
        linear, sinusoidal, closest
    }
    public InterpolationType interType = InterpolationType.linear;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ClippingMode
    {
        tile, mirror, extend, clip
    }
    public ClippingMode clipType = ClippingMode.tile;

    public EvaluableColorTable(int keyAmt)
    {
        for (int i = 0; i < keyAmt; i++)
        {
            ColorVec color = new ColorVec(1);
            keys.Add(color);
        }
    }

    public void SetKey(int index, ColorVec color)
    {
        keys[index] = color;
    }

    public int GetkeyAmt()
    {
        return keys.Count;
    }


    private ColorVec Interpolate(float x)
    {
        int keyCount = keys.Count;
        switch (keyCount)
        {
            case 1:
                return keys[0];
            default:
                return normalInterpolate();
        }

        ColorVec normalInterpolate()
        {
            // ((light I'm at)/last light)) x (Number of keys)
            // casted to int = lowest key
            
            if (x == 1)
            {
                x = 0.99999f;
            }else if (x == 0)
            {
                x = 0.00001f;
            }

            int keyIndex1 = (int)((keyCount - 1) * x);
            ColorVec clr1 = keys[keyIndex1];
            int keyIndex2 = (int)((keyCount - 1) * x + 1);
            ColorVec clr2 = keys[keyIndex2];

            float keyPercent1 = (float)keyIndex1 / (keyCount - 1);
            float keyPercent2 = (float)keyIndex2 / (keyCount - 1);

            float g = (x - keyPercent1) / (keyPercent2 - keyPercent1);
            switch (interType)
            {
                case InterpolationType.linear:
                    return ColorOperations.Lerp(clr1, clr2, g);
                case InterpolationType.sinusoidal:
                    return ColorOperations.Slerp(clr1, clr2, g);
                case InterpolationType.closest:
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


    }

    public ColorVec EvaluateColor(float vector)
    {
        float x = vector;
        switch (clipType)
        {
            case ClippingMode.tile:
                x = x - (int)x;
                if (x < 0)
                {
                    x = x + 1;
                }
                return Interpolate(x);
            case ClippingMode.mirror:
                int remain = ((int)x) % 2;
                x = x - (int)x;
                if (x < 0)
                {
                    x = x + 1;
                }
                if (remain != 0)
                {
                    x = -x + 1;
                }
                return Interpolate(x);
            case ClippingMode.extend:
                if (x > 1)
                {
                    return Interpolate(1);
                }
                if (x < 0)
                {
                    return Interpolate(0);
                }
                return Interpolate(x);
            case ClippingMode.clip:
                if (x > 1)
                {
                    return new ColorVec(0, 0, 0, 0);
                }
                if (x < 0)
                {
                    return new ColorVec(0, 0, 0, 0);
                }
                return Interpolate(x);
            default:
                Debug.Log("Invalid Clip Type");
                return new ColorVec(0, 0, 0, 0);
        }
    }

    public float EvaluateValue(float vector)
    {                
        return (float)EvaluateColor(vector);
    }

    public object GetCopy()
    {
        EvaluableColorTable temp = new EvaluableColorTable(GetkeyAmt());
        for (int i = 0; i < temp.GetkeyAmt(); i++)
        {
            temp.SetKey(i, keys[i]);
        }
        return temp;
    }

    public int GetResolution()
    {
        return keys.Count;
    }

}
