using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GradientAudioSync : MonoBehaviour
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
    void Start()
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
        int smallestIndex = 0;
        float smallest = 1;
        for (int i = 0; i < colorKey.Length; i++)
        {
            if (ConstScroll)
                colorKey[i].time += scrollMin * Time.deltaTime;
            else
                colorKey[i].time += (AudioPeer.AmplitudeAvgBuffer + AudioPeer.AmplitudeAvg) * scrollMult * Time.deltaTime;
            if (colorKey[i].time > 1)
            {
                colorKey[i].time = 0;
            }
            if (colorKey[i].time < smallest)
            {
                smallestIndex = i;
                smallest = colorKey[i].time;
            }
        }
        if (pulse)
        {
            for (int i = 0; i < colorKey.Length; i++)
            {
                float h, s, v;
                Color.RGBToHSV(colorKey[i].color, out h, out s, out v);
                colorKey[i].color = Color.HSVToRGB(h, 1, avgBands(4, 6, AudioPeer.audioBandRange));
            }
        }

        //val = Mathf.Clamp((((avgBands(4,7,AudioPeer.audioBandRange)) / 3)
        //    -0.25f) * (1/0.75f),0,1);
        val = AudioPeer.audioBandRange[6];
        hue += avgBands(5, 7, AudioPeer.audioBandRangeBuffer) * Time.deltaTime;
        hue = hue - (int)hue;
        colorKey[smallestIndex].color = Color.HSVToRGB(hue, 1, val);
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
