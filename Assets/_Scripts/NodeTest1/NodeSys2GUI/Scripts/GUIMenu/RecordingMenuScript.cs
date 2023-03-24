using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingMenuScript : MonoBehaviour
{
    public Text statusText;

    float time;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
}
