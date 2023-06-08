
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
    [JsonConstructor]
    public ColorVec(float rx, float gy, float bz = 0, float aw = 1)
    {
        this.rx = rx;
        this.gy = gy;
        this.bz = bz;
        this.aw = aw;
    }

    public ColorVec(float[] colors)
    {
        float[] newColors = { 1, 1, 1, 1 };
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

    public static ColorVec operator +(ColorVec color1, ColorVec color2)
    {
        return new ColorVec(color1.rx + color2.rx, color1.gy + color2.gy, color1.bz + color2.bz, color1.aw + color2.aw);
    }

    public static ColorVec operator -(ColorVec color1, ColorVec color2)
    {
        return new ColorVec(color1.rx - color2.rx, color1.gy - color2.gy, color1.bz - color2.bz, color1.aw - color2.aw);
    }

    public static ColorVec operator *(ColorVec color1, ColorVec color2)
    {
        return new ColorVec(color1.rx * color2.rx, color1.gy * color2.gy, color1.bz * color2.bz, color1.aw * color2.aw);
    }

    public static ColorVec operator /(ColorVec color1, ColorVec color2)
    {
        return new ColorVec(
            (color2.rx == 0) ? 0 : color1.rx / color2.rx,
            (color2.gy == 0) ? 0 : color1.gy / color2.gy,
            (color2.bz == 0) ? 0 : color1.bz / color2.bz,
            (color2.aw == 0) ? 0 : color1.aw / color2.aw
        );
    }

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

    public static ColorVec Lerp(ColorVec clr1, ColorVec clr2, float g)
    {
        return new ColorVec((clr2.rx - clr1.rx) * g + clr1.rx, (clr2.gy - clr1.gy) * g + clr1.gy,
                            (clr2.bz - clr1.bz) * g + clr1.bz);
    }

    //UNTESTED
    public static ColorVec Slerp(ColorVec clr1, ColorVec clr2, float g)
    {
        g = (1 - Mathf.Cos(g * Mathf.PI)) / 2;
        return new ColorVec((clr2.rx - clr1.rx) * g + clr1.rx, (clr2.gy - clr1.gy) * g + clr1.gy,
                            (clr2.bz - clr1.bz) * g + clr1.bz);
    }

    public static ColorVec Pow(ColorVec c, float exp)
    {
        return new ColorVec(
            (float)Math.Pow(c.rx, exp),
            (float)Math.Pow(c.gy, exp),
            (float)Math.Pow(c.bz, exp),
            (float)Math.Pow(c.aw, exp)
        );
    }
    public static ColorVec Log(ColorVec c, float @base)
    {
        return new ColorVec(
            (float)Math.Log(c.rx, @base),
            (float)Math.Log(c.gy, @base),
            (float)Math.Log(c.bz, @base),
            (float)Math.Log(c.aw, @base)
        );
    }

    public static ColorVec ClampColor(ColorVec c, float low = 0, float high = 1)
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
        float r = color.rx;
        float g = color.gy;
        float b = color.bz;

        float v= 0, h = 0, s = 0;
        float min = Math.Min(Math.Min(r, g), b);
        float max = Math.Max(Math.Max(r, g), b);

        v = max;

        float delta = max - min;

        if (max != 0)
        {
            s = delta / max;
        }
        else
        {
            s = 0;
            return new ColorVec(h, s, v);
        }

        if (r == max)
        {
            h = (g - b) / delta;
        }
        else if (g == max)
        {
            h = 2 + (b - r) / delta;
        }
        else
        {
            h = 4 + (r - g) / delta;
        }

        h *= 60;
        if (h < 0)
        {
            h += 360;
        }
        h /= 360f;
        return new ColorVec(h, s, v);
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