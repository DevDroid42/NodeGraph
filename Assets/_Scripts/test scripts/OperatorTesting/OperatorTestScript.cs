using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ByteData y1 = new ByteData(1);
        ByteData y2 = y1 + y1;
        int y3 = y2;
        testPrint((ByteData)y3);
    }

    private void testPrint(ByteData x)
    {
        Debug.Log("byte data to int" + x);
    }
}
