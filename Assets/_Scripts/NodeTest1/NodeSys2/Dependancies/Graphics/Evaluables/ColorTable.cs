
using System;
using System.ComponentModel;
using UnityEngine;

public class ColorTable : Evaluable
{
    private ColorVec[] keys;
    //brightness mutliplier. 

    public enum InterpolationType
    {
        linear, closest
    }
    public InterpolationType interType = InterpolationType.linear;

    public enum ClippingMode
    {
        tile, mirror, extend, clip
    }
    public ClippingMode clipType = ClippingMode.tile;

    public ColorTable(int keyAmt)
    {
        keys = new ColorVec[keyAmt];
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = new ColorVec();
            keys[i].aw = 1;
            keys[i].rx = 1;
            keys[i].gy = 1;
            keys[i].bz = 1;
        }
    }

    public void SetKey(int index, ColorVec color)
    {
        keys[index] = color;
    }

    public int GetkeyAmt()
    {
        return keys.Length;
    }


    private ColorVec Interpolate(float x)
    { // ((light I'm at)/last light)) x (Number of keys)
      // casted to int = lowest key
        if (x == 1)
        {
            x = 0.9999f;
        }
        if (x == 0)
        {
            x = 0.0001f;
        }
        //remap x 
        float mapping1 = 1.0f / keys.Length;
        float mapping2 = (keys.Length - 1.0f) / keys.Length;
        x = x * (mapping2 - mapping1) + mapping1;

        int keyIndex1 = (int)(keys.Length * x);
        ColorVec clr1 = keys[keyIndex1];
        int keyIndex2 = (int)(keys.Length * x + 1);
        ColorVec clr2 = keys[keyIndex2];

        float keyPercent1 = (float)keyIndex1 / keys.Length;
        float keyPercent2 = (float)keyIndex2 / keys.Length;

        float g = (x - keyPercent1) / (keyPercent2 - keyPercent1);
        switch (interType)
        {
            case InterpolationType.linear:
                ColorVec c = new ColorVec((clr2.rx - clr1.rx) * g + clr1.rx, (clr2.gy - clr1.gy) * g + clr1.gy,
                        (clr2.bz - clr1.bz) * g + clr1.bz);
                return c;
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

    public override ColorVec EvaluateColor(float x, float y, float z, float w)
    {
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

    public override float EvaluateValue(float x, float y, float z, float w)
    {
        return base.EvaluateValue(x, y, z, w);
    }

    /*
    public FloatColor getFloatColor(float input)
    {
        input = input - (int)input; //Make sure input is just decimal. 
        input *= scale; //scale the input decimal
        input += 1000; //add to prevent negative numbers
        input += offset - (int)offset; //add the offset to the decimal
        input = input - (int)input; //make sure the input is only a decimal again to allow for looping
        double position = keys.length * input;
        FloatColor FloatColor1 = keys[(int)position];

        FloatColor FloatColor2;
        // check to see if the next FloatColor wraps around to index 0
        if (((int)position + 1) > keys.length - 1)
        {
            FloatColor2 = keys[0];
        }
        else
        {
            FloatColor2 = keys[(int)position + 1];
        }
        double distance = position - (int)position;
        FloatColor c = Interpolate(FloatColor1, FloatColor2, distance);
        c = new FloatColor(c.red * brightness, c.green * brightness, c.blue * brightness);
        return c;
    }

    */

}
