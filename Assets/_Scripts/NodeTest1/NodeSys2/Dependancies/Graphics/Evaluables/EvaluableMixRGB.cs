using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluableMixRGB : Evaluable
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MixType
    {
        Add, Multiply, MixLinear, MixClosest
    }
    public MixType mixType;
    //the factor controls to what extent the mix type is applied. Every add operation is multiplied by the factor and mix 
    //interpolates between elements based on the factor. 
    public Evaluable factor;
    public List<Evaluable> elements;
    public bool clamp = true;
    public float low = 0;
    public float high = 1;

    public EvaluableMixRGB(Evaluable factor)
    {
        this.factor = factor;
        elements = new List<Evaluable>();
    }

    public void AddElement(Evaluable element)
    {
        elements.Add(element);
    }

    public override ColorVec EvaluateColor(ColorVec vector)
    {
        vector = TransformVector(vector);
        ColorVec output = new ColorVec(0);        
        switch (mixType)
        {
            case MixType.Add:
                output = Add(vector);
                break;
            case MixType.MixClosest:
            case MixType.MixLinear:
                output = mix(vector);
                break;
            case MixType.Multiply:
                output = Multiply(vector);
                break;
            default:
                break;
        }
        if (clamp)
        {
            Clamp(output, low, high);
        }
        return output.GetCopy();
    }

    private ColorVec Add(ColorVec vector)
    {
        if (elements.Count < 2)
        {
            return elements[0].EvaluateColor(vector);
        }
        
        ColorVec color = elements[0].EvaluateColor(vector);
        float fac = factor.EvaluateValue(vector);
        for (int i = 1; i < elements.Count; i++)
        {
            ColorVec elementColor = elements[i].EvaluateColor(vector);
            color += elementColor * fac;

        }
        return color;
    }

    private ColorVec mix(ColorVec vector)
    {        
        float x = factor.EvaluateValue(vector);

        if (elements.Count < 2)
        {
            return elements[0].EvaluateColor(vector);
        }


        if (x >= 1)
        {
            x = 0.99999f;
        }
        if (x <= 0)
        {
            x = 0.00001f;
        }

        int keyIndex1 = (int)((elements.Count - 1) * x);
        ColorVec clr1 = elements[keyIndex1].EvaluateColor(vector);
        int keyIndex2 = (int)((elements.Count - 1) * x + 1);
        ColorVec clr2 = elements[keyIndex2].EvaluateColor(vector);

        float keyPercent1 = (float)keyIndex1 / (elements.Count - 1);
        float keyPercent2 = (float)keyIndex2 / (elements.Count - 1);

        float g = (x - keyPercent1) / (keyPercent2 - keyPercent1);
        switch (mixType)
        {
            case MixType.MixLinear:
                //ColorVec c = new ColorVec((clr2.rx - clr1.rx) * g + clr1.rx, (clr2.gy - clr1.gy) * g + clr1.gy,
                //        (clr2.bz - clr1.bz) * g + clr1.bz);
                ColorVec c = (clr2 - clr1) * g + clr1;
                return c;
            case MixType.MixClosest:
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

    private ColorVec Multiply(ColorVec vector)
    {
        if (elements.Count < 2)
        {
            return elements[0].EvaluateColor(vector);
        }

        float fac = factor.EvaluateValue(vector);
        ColorVec orignalColor = elements[0].EvaluateColor(vector);
        ColorVec newColor = elements[0].EvaluateColor(vector);
        for (int i = 1; i < elements.Count; i++)
        {
            newColor *= elements[i].EvaluateColor(vector);
        }
        return orignalColor + (newColor - orignalColor) * fac;
    }



    private ColorVec Clamp(ColorVec input, float low, float high)
    {
        for (int i = 0; i < 4; i++)
        {
            if (input.getComponent(i) < low)
            {
                input.SetComponent(i, low);
            }
            else if (input.getComponent(i) > high)
            {
                input.SetComponent(i, high);
            }
        }
        return input;
    }

    public override float EvaluateValue(ColorVec vector)
    {
        return (float)EvaluateColor(vector);
    }

    public override object GetCopy()
    {
        EvaluableMixRGB mixRGB = new EvaluableMixRGB((Evaluable)factor.GetCopy());
        mixRGB.mixType = mixType;
        for (int i = 0; i < elements.Count; i++)
        {
            mixRGB.AddElement((Evaluable)elements[i].GetCopy());
        }
        return mixRGB;
    }
}
