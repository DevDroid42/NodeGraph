using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class lightController : MonoBehaviour {
    
    Light CurrentLight;

    [Range(0,7)]
    public int band;
    public bool useBufferAvg;
    public bool useMinBuffer = false;
    public float multiplier = 2;
    public float min;

	// Use this for initialization
	void Start () {
        CurrentLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        float usedMin = 0;

        if (useBufferAvg)
        {
            min = AudioPeer.GetAmpBufferAvg() * 150.0f;
            if(min > 2)
            {
                min = 2;
            }
        }
        if (useMinBuffer)
        {
            usedMin += (min - usedMin) / 3;
            CurrentLight.intensity = (AudioPeer.audioBandRangeBuffer[band]) * multiplier + usedMin;
        }
        else
        {
            CurrentLight.intensity = (AudioPeer.audioBandRangeBuffer[band]) * multiplier + min;
        }
    }
}
