using nodeSys2;
using System;
using UnityEngine;

public class ColorConstant : Node
{
    //internal color used for manual color setting
    public Property red, green, blue, alpha, internalColor, colorMode, outputColor;
    public Property[] floatInputs = new Property[4];    

    public ColorConstant(bool x)
    {
        base.nodeDisc = "Color Constant";
        colorMode = CreateInputProperty("ColorSpace", false, new ColorOperations.ColorSpace());
        colorMode.interactable = true;
        internalColor = CreateInputProperty("Color", false, new EvaluableColorVec(1));
        red = base.CreateInputProperty("Red", true, new EvaluableFloat(0));
        floatInputs[0] = red;
        green = base.CreateInputProperty("Green", true, new EvaluableFloat(0));
        floatInputs[1] = green;
        blue = base.CreateInputProperty("Blue", true, new EvaluableFloat(0));
        floatInputs[2] = blue;
        alpha = base.CreateInputProperty("Alpha", true, new EvaluableFloat(0));
        floatInputs[3] = alpha;
        outputColor = base.CreateOutputProperty("Output Color");


        for (int i = 0; i < floatInputs.Length; i++)
        {
            floatInputs[i].visible = false;
        }
    }

    private bool rgb = true;
    public override void Init()
    {
        base.Init();
        //Enums are seralized as strings. On Deserialization the object type will be a string. Check for this case and replace it with 
        //the appropriate enum Type
        if(colorMode.GetData().GetType() == typeof(string))
        {
            colorMode.SetData(Enum.Parse(typeof(ColorOperations.ColorSpace), (string)colorMode.GetData()));
        }
        if (((ColorOperations.ColorSpace)colorMode.GetData()) == ColorOperations.ColorSpace.RGB)
        {
            rgb = true;
            floatInputs[0].Disc = "Red";
            floatInputs[1].Disc = "Green";
            floatInputs[2].Disc = "Blue";
        }
        else
        {
            rgb = false;
            floatInputs[0].Disc = "Hue";
            floatInputs[1].Disc = "Saturation";
            floatInputs[2].Disc = "Value";
        }
        //at start set all float inputs to the internal color. This internal color is a node constant. If data is received from the 
        //the float inputs the internal color will be set to non-interactable and will be used for visualization only. 
        //if all inputs are dissconnected this flag will ensure that users can edit the color 
        internalColor.interactable = true;
        //when data comes in this will be set to "color(Driven)" so reset it here on run
        internalColor.Disc = "Color";

        outputColor.Invoke(((Evaluable)(internalColor.GetData())).GetCopy());
    }

    public override void Handle()
    {
        EvaluableColorVec proccesedColor = new EvaluableColorVec(ProcessData());
        internalColor.SetData(proccesedColor);
        internalColor.interactable = false;
        internalColor.Disc = "Color(driven)";
        outputColor.Invoke(((Evaluable)(internalColor.GetData())).GetCopy());
    }


    private ColorVec ProcessData()
    {
        Evaluable c = null;
        float[] floatBuffer = new float[floatInputs.Length];
        for (int i = 0; i < floatInputs.Length; i++)
        {
            if (floatInputs[i].TryGetDataType(ref c))
            {
                floatBuffer[i] = c.EvaluateValue(0, 0, 0, 0);
            }
            else
            {
                floatBuffer[i] = 0;
            }
        }
        if (rgb)
        {
            return new ColorVec(floatBuffer[0], floatBuffer[1], floatBuffer[2], floatBuffer[3]);
        }
        else
        {
            ColorVec color = ColorOperations.HsvToRgb(floatBuffer[0], floatBuffer[1], floatBuffer[2]);
            color.aw = floatBuffer[3];
            return color;
        }
    }
}
