using nodeSys2;
using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class LedUDP : Node
{
    public Property ipProp, portProp, ledCountProp, input, message;
    private int port, ledCount;
    private string ip;
    NonMonoUDP udp;

    public LedUDP(ColorVec pos) : base(pos)
    {
        nodeDisc = "LED Node";
        ipProp = CreateInputProperty("IP:", false, new StringData("192.168.0.133"));
        ipProp.interactable = true;
        ledCountProp = CreateInputProperty("Led Count", false, new EvaluableFloat(0));
        ledCountProp.interactable = true;
        portProp = CreateInputProperty("Port", false, new EvaluableFloat(21234));
        portProp.interactable = true;
        input = CreateInputProperty("Color Data", true, new Evaluable());
        input.visible = true;
        message = CreateInputProperty("status:", false, new Message(""));
    }

    public override void Init()
    {
        frameDelagate -= Frame;
        frameDelagate += Frame;
        port = (int)((Evaluable)(portProp.GetData())).EvaluateValue(0);
        ip = (string)((StringData)(ipProp.GetData())).txt;
        ledCount = (int)((Evaluable)(ledCountProp.GetData())).EvaluateValue(0);

        if (udp == null)
        {
            udp = new NonMonoUDP(ip, port);
        }
    }
 

    public override void Frame(float deltaTime)
    {
        udp.Send(ledCount, (Evaluable)input.GetData());
    }

    
    public override void Handle()
    {
        
    }
  
}


public class NonMonoUDP
{
    public string ip = "192.168.0.49";
    public int port = 21324;
    private UdpClient udpClient;
    

    // Start is called before the first frame update
    public NonMonoUDP(string ip, int port)
    {
        this.ip = ip;
        udpClient = new UdpClient(port);
    }

    public void Send(int ledCount, Evaluable data)
    {
        byte[] message = GenWARLS(ledCount, data);        
        udpClient.Send(message, message.Length, ip, port);
    }

    public byte[] GenWARLS(int ledCount, Evaluable colorData)
    {
        byte[] message = new byte[ledCount * 4 + 2];
        message[0] = 1;
        message[1] = 2;
        for (int i = 0; i < ledCount; i++)
        {
            message[2 + i * 4] = (byte)i;
            message[3 + i * 4] = (byte)(colorData.EvaluateColor((float)i / ledCount).rx * 255);
            message[4 + i * 4] = (byte)(colorData.EvaluateColor((float)i / ledCount).gy * 255);
            message[5 + i * 4] = (byte)(colorData.EvaluateColor((float)i / ledCount).bz * 255);
        }

        return message;
    }
}
