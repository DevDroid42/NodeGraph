

using Newtonsoft.Json;

public class EvaluableColorVec : Evaluable
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

    public override ColorVec EvaluateColor(ColorVec vector)
    {
        return colorVec.GetCopy();
    }

    public override float EvaluateValue(ColorVec vector)
    {
        return (float)colorVec;
    }

    public override Evaluable GetCopy()
    {
        return new EvaluableColorVec(new ColorVec(colorVec.rx, colorVec.gy, colorVec.bz, colorVec.aw));
    }
}
