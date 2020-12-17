using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatParseTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float test = float.Parse("0.00000001");
        Debug.Log(float.Parse(test.ToString()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
