﻿
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using System;

public readonly struct ColorVec
{
    public readonly float rx;
    public readonly float gy;
    public readonly float bz;
    public readonly float aw;

    //creates a color from black to white
    public ColorVec(float value)
    {
        rx = value;
        gy = value;
        bz = value;
        aw = 1;
    }

    public ColorVec(float R, float G, float B = 0, float A = 1)
    {
        this.rx = R;
        this.gy = G;
        this.bz = B;
        this.aw = A;
    }

    public ColorVec(float[] colors)
    {
        float[] newColors = {1,1,1,1};
        for (int i = 0; i < Math.Min(colors.Length, 4); i++)
        {
            newColors[i] = colors[i];
        }
        rx = newColors[0];
        gy = newColors[1];
        bz = newColors[2];
        aw = newColors[3];
    }

    public static ColorVec GetColorWithUpdatedComponent(ColorVec color, int component, float value)
    {
        float[] components = new float[4];
        for (int i = 0; i < components.Length; i++)
        {
            components[i] = color.GetComponent(i);
        }
        components[component] = value;
        return new ColorVec(components);
    }

    public float GetComponent(int i)
    {
        switch (i)
        {
            case 0:
                return rx;

            case 1:
                return gy;

            case 2:
                return bz;

            case 3:
                return aw;

            default:
                Debug.LogWarning("Invalid vector compoent: " + i);
                return 0;
        }
    }

    public bool Equals(ColorVec other)
    {
        return rx == other.rx && gy == other.gy && bz == other.bz && aw == other.aw;
    }

    public override string ToString()
    {
        return "\tRx:" + rx + "\tGy:" + gy + "\tBz:" + bz + "\tAw:" + aw;
    }

    public static ColorVec operator -(ColorVec color) => new ColorVec(1 - color.rx, 1 - color.gy, 1 - color.bz, 1 - color.aw);
    public static ColorVec operator +(ColorVec color) => new ColorVec(color.rx, color.gy, color.bz, color.aw);
    public static ColorVec operator +(ColorVec color1, ColorVec color2) =>
       new ColorVec(color1.rx + color2.rx, color1.gy + color2.gy, color1.bz + color2.bz, color1.aw + color2.aw);

    public static ColorVec operator -(ColorVec color1, ColorVec color2) =>
        new ColorVec(color1.rx - color2.rx, color1.gy - color2.gy, color1.bz - color2.bz, color1.aw - color2.aw);
    public static ColorVec operator *(ColorVec color1, ColorVec color2) =>
        new ColorVec(color1.rx * color2.rx, color1.gy * color2.gy, color1.bz * color2.bz, color1.aw * color2.aw);

    public static explicit operator float(ColorVec b) => ColorOperations.RgbToHsv(b).bz;
    public static explicit operator bool(ColorVec b) => !((float)b < 0.5);
    public static implicit operator ColorVec(float b) => new ColorVec(b);
}

public class ColorOperations
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ColorSpace
    {
        RGB, HSV
    }

    public static ColorVec ClampColor(ColorVec c)
    {
        return new ColorVec(
            Clamp(c.rx, 0, 1),
            Clamp(c.gy, 0, 1),
            Clamp(c.bz, 0, 1),
            Clamp(c.aw, 0, 1)
        );
    }

    private static float Clamp(float val, float min, float max)
    {
        if (val > max)
        {
            return max;
        }
        else if (val < min)
        {
            return min;
        }
        else
        {
            return val;
        }
    }

    public static ColorVec RgbToHsv(ColorVec color)
    {
        float r = color.rx * 255;
        float g = color.gy * 255;
        float b = color.bz * 255;

        // h, s, v = hue, saturation, value 
        float cmax = Math.Max(r, Math.Max(g, b)); // maximum of r, g, b 
        float cmin = Math.Min(r, Math.Min(g, b)); // minimum of r, g, b 
        float diff = cmax - cmin; // diff of cmax and cmin. 
        float h = -1, s = -1;

        // if cmax and cmax are equal then h = 0 
        if (cmax == cmin)
            h = 0;

        // if cmax equal r then compute h 
        else if (cmax == r)
            h = (60 * ((g - b) / diff) + 360) % 360;

        // if cmax equal g then compute h 
        else if (cmax == g)
            h = (60 * ((b - r) / diff) + 120) % 360;

        // if cmax equal b then compute h 
        else if (cmax == b)
            h = (60 * ((r - g) / diff) + 240) % 360;

        // if cmax equal zero 
        if (cmax == 0)
            s = 0;
        else
            s = (diff / cmax) * 100;

        // compute v 
        float v = cmax;
        return new ColorVec(h / 255f, s / 255f, v / 255f);
    }

    public static ColorVec HsvToRgb(ColorVec hsvData)
    {
        return HsvToRgb(hsvData.rx, hsvData.gy, hsvData.bz);
    }

    /// <summary>
    /// Convert HSV to RGB
    /// h is from 0-360
    /// s,v values are 0-1  
    /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
    /// </summary>
    public static ColorVec HsvToRgb(float h, float S, float V)
    {
        h *= 360;
        // ######################################################################
        // T. Nathan Mundhenk
        // mundhenk@usc.edu
        // C/C++ Macro HSV to RGB

        float H = h;
        while (H < 0) { H += 360; };
        while (H >= 360) { H -= 360; };
        float R, G, B;
        if (V <= 0)
        { R = G = B = 0; }
        else if (S <= 0)
        {
            R = G = B = V;
        }
        else
        {
            float hf = H / 60.0f;
            int i = (int)Math.Floor(hf);
            float f = hf - i;
            float pv = V * (1 - S);
            float qv = V * (1 - S * f);
            float tv = V * (1 - S * (1 - f));
            switch (i)
            {

                // Red is the dominant color

                case 0:
                    R = V;
                    G = tv;
                    B = pv;
                    break;

                // Green is the dominant color

                case 1:
                    R = qv;
                    G = V;
                    B = pv;
                    break;
                case 2:
                    R = pv;
                    G = V;
                    B = tv;
                    break;

                // Blue is the dominant color

                case 3:
                    R = pv;
                    G = qv;
                    B = V;
                    break;
                case 4:
                    R = tv;
                    G = pv;
                    B = V;
                    break;

                // Red is the dominant color

                case 5:
                    R = V;
                    G = pv;
                    B = qv;
                    break;

                // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                case 6:
                    R = V;
                    G = tv;
                    B = pv;
                    break;
                case -1:
                    R = V;
                    G = pv;
                    B = qv;
                    break;

                // The color is not defined, we should throw an error.

                default:
                    //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                    R = G = B = V; // Just pretend its black/white
                    break;
            }
        }
        ColorVec output = new ColorVec(R, G, B);
        return output;
    }

}