using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Makaretu.Dns;

public class UDPBlenderTesting : MonoBehaviour
{
    public string ip;
    public int port = 5005;
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
        byte[] message = new byte[1];
        message[0] = (byte)(AudioPeer.AmplitudeAvg * 255);
        //udpClient.Send(message, message.Length);
        udpClient.Send(message, message.Length);
    }
}
