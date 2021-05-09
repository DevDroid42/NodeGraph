using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientAudioSyncTwo : MonoBehaviour
{
    public Gradient gradient;
    public bool ConstScroll = true;
    public float scrollMin = 1;
    public float scrollMult = 1;
    public float colorSpeedMult = 1;
    public bool pulse = false;

    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    // Start is called before the first frame update
    void Awake()
    {
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[8];
        for (int i = 0; i < colorKey.Length; i++)
        {
            colorKey[i].time = i / 8.0f;
        }


        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;
        gradient.SetKeys(colorKey, alphaKey);
    }

    float hue = 0;
    float val = 0;
    float hsvSpeed = 0;
    // Update is called once per frame
    void Update()
    {

        //val = Mathf.Clamp((((avgBands(4,7,AudioPeer.audioBandRange)) / 3)
        //    -0.25f) * (1/0.75f),0,1);
        hue += avgBands(5, 7, AudioPeer.audioBandRangeBuffer) * Time.deltaTime;
        hue = hue - (int)hue;
        for (int i = 0; i < colorKey.Length; i++)
        {
            colorKey[i].color = Color.HSVToRGB(hue + (i/colorKey.Length * 2), 1, AudioPeer.audioBandRangeBuffer[i] * AudioPeer.audioBandRangeBuffer[i]);
        }

        gradient.SetKeys(colorKey, alphaKey);
    }

    private float avgBands(int first, int second, float[] bands)
    {
        float avg = 0;
        for (int i = first; i < second; i++)
        {
            avg += bands[i];
        }
        return avg /= second - first;
    }


}
