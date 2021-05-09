using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] data = { 2, 65, 65, 65, 0, 0 };
        NetworkMessage msg = new NetworkMessage(data, "4.12.6.346");
        Debug.Log(msg.dataType.ToString() + "\t" + msg.ID + "\t");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
