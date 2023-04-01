using Newtonsoft.Json;
using nodeSys2;
using System;
using UnityEngine;

public class ColorConstant : Node
{
    //internal color used for manual color setting
    [JsonProperty] private Property red, green, blue, alpha, internalColor, colorMode, outputColor;
    [JsonProperty] private Property[] floatInputs = new Property[4];

    //these are used to track when a data type is changed. because both the floatInputs and internal color control the same data, 
    //when init starts we have no way of knowing which one was changed. Each time init runs it will save a duplicate of the internal color Property
    //to compare. If the internal color and it's dupe differ we know the internal color property was manipulated in the color picker.
    //In this case we set the floatInputs to match the new color. If the internalColor property and it's data dupe match than the only other option
    //is that the floatInputs were changed. 
    private ColorVec internalColorDupe = new ColorVec();


    public ColorConstant(ColorVec pos) : base(pos)
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
        alpha = base.CreateInputProperty("Alpha", true, new EvaluableFloat(1));
        floatInputs[3] = alpha;
        outputColor = base.CreateOutputProperty("Output Color");

    }

    private bool rgb = true;
    public override void Init()
    {
        base.Init();

        for (int i = 0; i < floatInputs.Length; i++)
        {
            floatInputs[i].interactable = false;
        }

        //Enums are seralized as strings. On Deserialization the object type will be a string. Check for this case and replace it with 
        //the appropriate enum Type
        if (colorMode.GetData().GetType() == typeof(string))
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
        ProcessColorChanges();
        //at start set all float inputs to the internal color. This internal color is a node constant. If data is received from the 
        //the float inputs the internal color will be set to non-interactable and will be used for visualization only. 
        //if all inputs are dissconnected this flag will ensure that users can edit the color 
        internalColor.interactable = true;
        //when data comes in this will be set to "color(Driven)" so reset it here on run
        internalColor.Disc = "Color";        
    }

    public override void Init2()
    {
        base.Init2();
        outputColor.Invoke(((IEvaluable)(internalColor.GetData())));
    }

    private void ProcessColorChanges()
    {
        //if they are equal than the color picker hasn't been used. Set the color to be the floatInputs
        if (((IEvaluable)internalColor.GetData()).EvaluateColor(0).Equals(internalColorDupe))
        {
            EvaluableColorVec proccesedColor = new EvaluableColorVec(ProcessData());
            internalColor.SetData(proccesedColor);
            internalColorDupe = ((IEvaluable)internalColor.GetData()).EvaluateColor(0);
        }
        else //the color has been changed by the picker. Chnage the float constants to match the internal color and reset the dupe
        {
            internalColorDupe = ((IEvaluable)internalColor.GetData()).EvaluateColor(0);
            alpha.SetData(new EvaluableFloat(internalColorDupe.aw));
            if (rgb)
            {                
                red.SetData(new EvaluableFloat(internalColorDupe.rx));
                green.SetData(new EvaluableFloat(internalColorDupe.gy));                
                blue.SetData(new EvaluableFloat(internalColorDupe.bz));                
            }
            else
            {
                ColorVec HSVColor = ColorOperations.RgbToHsv(internalColorDupe);
                red.SetData(new EvaluableFloat(HSVColor.rx));
                green.SetData(new EvaluableFloat(HSVColor.gy));
                blue.SetData(new EvaluableFloat(HSVColor.bz));
            }
        }        
    }
    

    public override void Handle()
    {
        EvaluableColorVec proccesedColor = new EvaluableColorVec(ProcessData());
        internalColor.SetData(proccesedColor);
        internalColorDupe = ((IEvaluable)internalColor.GetData()).EvaluateColor(0);
        //ProcessColorChanges();
        internalColor.interactable = false;
        internalColor.Disc = "Color (Driven)";
        outputColor.Invoke(((IEvaluable)(internalColor.GetData())));
    }


    private ColorVec ProcessData()
    {
        IEvaluable c = null;
        float[] floatBuffer = new float[floatInputs.Length];
        for (int i = 0; i < floatInputs.Length; i++)
        {
            if (floatInputs[i].TryGetDataType(ref c))
            {
                floatBuffer[i] = c.EvaluateValue(0);
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
            color =  ColorVec.GetColorWithUpdatedComponent(color, 3, floatBuffer[3]);
            return color;
        }
    }
}
