
public class ColorVec
{
    public float rx;
    public float gy;
    public float bz;
    public float aw;

    public ColorVec()
    {
        rx = 1;
        gy = 1;
        bz = 1;
        aw = 1;
    }

    //creates a color from black to white
    public ColorVec(float value)
    {
        rx = value;
        gy = value;
        bz = value;
        aw = 1;
    }

    public ColorVec(float R, float G, float B)
    {
        this.rx = R;
        this.gy = G;
        this.bz = B;
        aw = 1;
    }

    public ColorVec(float R, float G, float B, float A)
    {
        this.rx = R;
        this.gy = G;
        this.bz = B;
        this.aw = A;
    }

    public override string ToString()
    {
        return "\tRx:" + rx + "\tGy:" + gy + "\tBz:" + bz + "\tAw:" + aw;
     }
}