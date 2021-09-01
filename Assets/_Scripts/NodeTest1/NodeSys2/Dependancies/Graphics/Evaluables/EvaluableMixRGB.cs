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
        Add, Mix
    }
    public MixType mixType = MixType.Add;
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
        TransformVector(vector);
        ColorVec output = new ColorVec(0);
        switch (mixType)
        {
            case MixType.Add:
                output = Add(vector);
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
        if(elements.Count < 2)
        {
            return elements[0].EvaluateColor(vector);
        }

        //ColorVec color = new ColorVec(elements[0].EvaluateColor(vector).rx);
        ColorVec color = elements[0].EvaluateColor(vector).GetCopy();
        float fac = factor.EvaluateValue(vector);
        for (int i = 1; i < elements.Count; i++)
        {
            ColorVec elementColor = elements[i].EvaluateColor(vector);            
            //iterate through rgbw
            for (int j = 0; j < 4; j++)
            {
                //set color compoent to current component + element * factor
                float elementComp = elementColor.getComponent(j);                
                color.SetComponent(j, color.getComponent(j) + elementComp * fac);
            }
        }
        return color;
    }

    private ColorVec mix(ColorVec vector)
    {
        ColorVec color = new ColorVec();
        for (int i = 0; i < elements.Count; i++)
        {
            //iterate through rgbw
            for (int j = 0; j < 4; j++)
            {
                //set color compoent to current component + element * factor
                color.SetComponent(j, color.getComponent(j) + elements[i].EvaluateColor(vector).getComponent(i) * factor.EvaluateValue(vector));
            }
        }
        return color;
    }


    private ColorVec Clamp(ColorVec input, float low, float high)
    {
        for (int i = 0; i < 4; i++)
        {
            if(input.getComponent(i) < low)
            {
                input.SetComponent(i, low);
            }else if(input.getComponent(i) > high)
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

    public override Evaluable GetCopy()
    {
        EvaluableMixRGB mixRGB = new EvaluableMixRGB(factor.GetCopy());
        for (int i = 0; i < elements.Count; i++)
        {
            mixRGB.AddElement(elements[i].GetCopy());
        }
        return mixRGB;
    }
}
