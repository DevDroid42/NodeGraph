

using Newtonsoft.Json;

public class EvaluableColorVec : Evaluable
{
    [JsonProperty]
    protected ColorVec colorVec;

    //used for GUI display. Makes no difference to runtime processing. 
    public enum DisplayMode
    {
        Color, Vector2, Vector3, Vector4
    }
    public DisplayMode displayMode = DisplayMode.Color;

    public ColorVec GetColorVec()
    {
        return colorVec;
    }

    public EvaluableColorVec(ColorVec colorVec)
    {
        this.colorVec = colorVec;
    }

    public void SetColorVec(ColorVec colorVec)
    {
        this.colorVec = colorVec;
    }

    public override ColorVec EvaluateColor(float x, float y, float z, float w)
    {
        return colorVec;
    }

    public override float EvaluateValue(float x, float y, float z, float w)
    {
        return (float)colorVec;
    }
}
