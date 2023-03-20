

using Newtonsoft.Json;

public class EvaluableColorVec : IEvaluable
{
    [JsonProperty]
    protected ColorVec colorVec;

    //used for GUI display. Makes no difference to runtime processing. 
    //IDEA don't use this in editor manager, instead use in editor script. use this to choose which editor
    //will open by default but allow user to switch with button
    public enum DefaultDisplayMode
    {
        Color, Vector2, Vector3, Vector4
    }
    public DefaultDisplayMode displayMode = DefaultDisplayMode.Color;

    public EvaluableColorVec(ColorVec colorVec)
    {
        this.colorVec = colorVec;
    }

    public void SetColorVec(ColorVec colorVec)
    {
        this.colorVec = colorVec;
    }

    public ColorVec EvaluateColor(float vector)
    {
        return colorVec;
    }

    public float EvaluateValue(float vector)
    {
        return (float)colorVec;
    }

    public object GetCopy()
    {
        return new EvaluableColorVec(new ColorVec(colorVec.rx, colorVec.gy, colorVec.bz, colorVec.aw));
    }

    public int GetResolution()
    {
        return 1;
    }
}
