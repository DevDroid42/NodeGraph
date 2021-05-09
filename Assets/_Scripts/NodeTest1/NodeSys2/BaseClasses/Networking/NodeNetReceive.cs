using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using nodeSys2;
using System.Text;
using UnityEngine;
using System.Threading;

public class NodeNetReceive
{
    NodeNetReceiveThreaded netThreadObj;

    public NodeNetReceive()
    {
        Node.frameDelagate -= Frame;
        Node.frameDelagate += Frame;
        netThreadObj = new NodeNetReceiveThreaded();
        Thread networkReceiveThread = new Thread(netThreadObj.StartLoop);
        networkReceiveThread.Start();
    }

    public void Shutdown()
    {
        netThreadObj.Stop();
    }


    private void Frame(float delta)
    {
        List<NetworkMessage> messages = netThreadObj.GetMessages();
        foreach (NetworkMessage message in messages)
        {
            Debug.Log(message);
            if (Node.nodeNetDelagate != null)
            {
                Node.nodeNetDelagate.Invoke(message);
            }
        }
    }
}

//A thread to process and store data as it comes in. This is made for live data streaming. messages retreived will always
//be the most receint received from the header. All messages received in this manner are not garanteed to be processed
public class NodeNetReceiveThreaded
{
    private object msgLock = new object();
    //messages that have yet to be processed. All messages are the most recent version received of the header type
    private List<NetworkMessage> messageQueue = new List<NetworkMessage>();
    private bool run = true;

    public void StartLoop()
    {
        //IPEndPoint object will allow us to read datagrams sent from any source.
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        // This constructor arbitrarily assigns the local port number.
        UdpClient udpClient = new UdpClient(11000);
        while (run)
        {
            // Blocks until a message returns on this socket from a remote host.
            byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            lock (msgLock)
            {
                NetworkMessage receivedMsg = new NetworkMessage(receiveBytes, RemoteIpEndPoint.Address.ToString());
                if (messageQueue.Count == 0)
                {
                    messageQueue.Add(receivedMsg);
                }
                for (int i = 0; i < messageQueue.Count; i++)
                {
                    if (receivedMsg.CompareHeader(messageQueue[i]))
                    {
                        messageQueue[i].UpdateDataBytes(receiveBytes);
                    }
                    else
                    {
                        messageQueue.Add(receivedMsg);
                    }
                }
            }
        }
    }

    public void Stop()
    {
        lock (msgLock)
        {
            run = false;
        }
    }

    public List<NetworkMessage> GetMessages()
    {
        lock (msgLock)
        {
            List<NetworkMessage> messages = GetMessageQueueCopy();
            messageQueue.Clear();
            return messages;
        }
    }
    private List<NetworkMessage> GetMessageQueueCopy()
    {
        List<NetworkMessage> messages = new List<NetworkMessage>();
        for (int i = 0; i < messageQueue.Count; i++)
        {
            messages.Add(messageQueue[i].GetCopy());
        }
        return messages;
    }
}
