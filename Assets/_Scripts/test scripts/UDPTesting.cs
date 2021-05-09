using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class UDPTesting : MonoBehaviour
{
    public GradientAudioSyncTwo ColorMap;
    public string ip = "4.3.2.1";
    public int ledCount;
    public int port = 21324;
    byte[] message = { 1, 2, 0, 0, 255, 255, 1, 255, 0, 0};
    private UdpClient udpClient;

    // Start is called before the first frame update
    void Start()
    {
        udpClient = new UdpClient(port);
        InitConnection();
    }

    public void netDiscover()
    {

    }

    public byte[] GenWARLS(int ledCount, Gradient gradient)
    {
        byte[] message = new byte[ledCount * 4 + 2];
        message[0] = 1;
        message[1] = 2;
        for (int i = 0; i < ledCount; i++)
        {           
            message[2 + i * 4] = (byte)i;
            message[3 + i * 4] = (byte)(gradient.Evaluate((float)i / ledCount).r * 255);
            message[4 + i * 4] = (byte)(gradient.Evaluate((float)i / ledCount).g * 255);
            message[5 + i * 4] = (byte)(gradient.Evaluate((float)i / ledCount).b * 255);
        }

        return message;
    }

    public void InitConnection()
    {
        try
        {
            udpClient.Connect(ip, port);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
        }
    }

    public void CloseConnection()
    {
        udpClient.Close();
    }

    // Update is called once per frame
    void Update()
    {
        byte[] message = GenWARLS(ledCount, ColorMap.gradient);
        udpClient.Send(message, message.Length);
    }

    public byte[] GenWARLS(int ledCount, Evaluable colorData)
    {
        byte[] message = new byte[ledCount * 4 + 2];
        message[0] = 1;
        message[1] = 2;
        for (int i = 0; i < ledCount; i++)
        {
            message[2 + i * 4] = (byte)i;
            message[3 + i * 4] = (byte)(colorData.EvaluateColor(((float)i / ledCount), 0, 0, 0).rx * 255);
            message[4 + i * 4] = (byte)(colorData.EvaluateColor(((float)i / ledCount), 0, 0, 0).gy * 255);
            message[5 + i * 4] = (byte)(colorData.EvaluateColor(((float)i / ledCount), 0, 0, 0).bz * 255);
        }

        return message;
    }
}
