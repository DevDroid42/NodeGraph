using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmisiveLightUpdate : MonoBehaviour {

    public GameObject EmmisiveControllerObj;
    [Range(0, 7)]
    public int AudioBand;
    public float Multiplier;
    public bool addAverage;
    public float MinimumEmission;
    public bool UpdateGI;
    public Color EmmisionColor;

    Renderer ObjectRend;
    Material ObjectMat;
    // Use this for initialization
    void Start () {
        ObjectRend = gameObject.GetComponent<Renderer>();
        ObjectMat = ObjectRend.material;
    }
	
	// Update is called once per frame
	void Update () {
        Color finalColor;
        if (!addAverage)
        {
            finalColor = EmmisionColor * Mathf.LinearToGammaSpace(((AudioPeer.audioBandRangeBuffer[AudioBand]) * Multiplier) + MinimumEmission);
        }
        else
        {
            finalColor = EmmisionColor * Mathf.LinearToGammaSpace((((AudioPeer.audioBandRangeBuffer[AudioBand])+(AudioPeer.AmplitudeAvg/2)) * Multiplier) + MinimumEmission);
        }
        ObjectMat.SetColor("_EmissionColor", finalColor);
        if (UpdateGI) {
            DynamicGI.SetEmissive(ObjectRend, finalColor);//updates Realtime GI also seems to set instance of object material;
        }
    }


    EmmisiveShaders EmmisiveScript()
    {
        return  EmmisiveControllerObj.gameObject.GetComponent<EmmisiveShaders>();
    }
}
