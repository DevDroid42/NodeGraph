using nodeSys2;
using System;
using System.Net.Sockets;
using UnityEngine;

public class LedUDP : Node
{
    public Property ipProp, portProp, ledCountProp, EvaluableData, message;
    private int port, ledCount;
    private string ip;
    NonMonoUDP udp;

    public LedUDP(bool x)
    {
        nodeDisc = "LED Node";
        ipProp = CreateInputProperty("IP:", false, new StringData("4.3.2.1"));
        ipProp.interactable = true;
        ledCountProp = CreateInputProperty("Led Count", false, new EvaluableFloat(0));
        ledCountProp.interactable = true;
        portProp = CreateInputProperty("Port", false, new EvaluableFloat(21234));
        portProp.interactable = true;
        EvaluableData = CreateInputProperty("Color Data", true, new Evaluable());
        EvaluableData.visible = false;
        message = CreateInputProperty("status:", false, new Message(""));
    }

    public override void Init()
    {
        frameDelagate -= Frame;
        frameDelagate += Frame;
        port = (int)((Evaluable)(portProp.GetData())).EvaluateValue(0, 0, 0, 0);
        ip = (string)((StringData)(ipProp.GetData())).txt;
        ledCount = (int)((Evaluable)(ledCountProp.GetData())).EvaluateValue(0, 0, 0, 0);

        if (udp == null)
        {
            udp = new NonMonoUDP(ip, port);
        }
    }
 

    public override void Frame(float deltaTime)
    {
        udp.Update(ledCount, colorData);
    }

    private Evaluable colorData = new Evaluable();
    public override void Handle()
    {
        if (ProccessData())
        {
            //udp.Update(ledCount, colorData);
            //byte[] message = GenWARLS(ledCount, colorData);
            //udpClient.Send(message, message.Length);
        }
    }

    private bool ProccessData()
    {
        if (EvaluableData.TryGetDataType(ref colorData))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


public class NonMonoUDP
{
    public string ip = "4.3.2.1";
    public int port = 21324;
    private UdpClient udpClient;

    // Start is called before the first frame update
    public NonMonoUDP(string ip, int port)
    {
        udpClient = new UdpClient(port);
        InitConnection();
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

    public void Update(int ledCount, Evaluable data)
    {
        byte[] message = GenWARLS(ledCount, data);
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
