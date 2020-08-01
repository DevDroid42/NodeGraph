using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManagerTester : MonoBehaviour
{
    public string audioTag;
    public AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(audio.GetAnalyzer(0));
        gameObject.transform.localScale = new Vector3(audioManager.GetAnalyzer(0).audioBandRangeBuffer[0] * 5,1,1);       
    }
}
