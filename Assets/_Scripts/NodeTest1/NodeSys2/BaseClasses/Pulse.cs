using Newtonsoft.Json;
public class Pulse : ICopyable
{
    //determines if this pulse has been read already    
    [JsonProperty]
    private bool pulsePresent;

    public Pulse()
    {
        pulsePresent = true;
    }

    public Pulse(bool state)
    {
        pulsePresent = state;
    }

    public bool PulsePresent()
    {
        //if the pulse is present that means it hasn't been read yet
        if (pulsePresent)
        {
            pulsePresent = false;
            return true;
        }
        return false;
    }

    public object GetCopy()
    {
        return new Pulse(pulsePresent);
    }
}
