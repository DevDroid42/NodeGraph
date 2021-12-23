
public class Pulse : ICopyable
{
    //determines if this pulse has been read already
    private bool pulsePresent;

    public Pulse()
    {
        pulsePresent = true;
    }

    public bool PulsePresent()
    {
        //if the pulse is present that means it hasn't been read yet
        if (pulsePresent)
        {
            pulsePresent = false;
        }
        return pulsePresent;
    }

    public object GetCopy()
    {
        return this.MemberwiseClone();
    }
}
