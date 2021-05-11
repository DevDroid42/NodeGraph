using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NetworkMessageGUI : MonoBehaviour
{
    private NetworkMessage message;
    public Text txt;
    public Color newColor;
    private Color defaultColor;
    void Start()
    {
        defaultColor = txt.color;
    }

    // Update is called once per frame
    void Update()
    { 
            int seconds = (DateTime.Now - message.time).Seconds;
            if (seconds < 5)
            {
                txt.color = Color.Lerp(newColor, defaultColor, seconds / 5f);
            }
    }

    public void UpdateNetworkMessage(NetworkMessage message)
    {
        this.message = message;
        txt.text = "from:[" + message.ip + "]" + message.ToString().Substring(16);
    }
    public NetworkMessage GetMessage()
    {
        return message;
    }
}
