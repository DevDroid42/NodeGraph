using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

//stores data along with data descriptors. The descriptors make up the start of packets
//(headers) and are parsed into variables in this class
public class NetworkMessage
{
    public enum DataType { Value, ValueArray, ColorArray, Text};
    //the dataType this message holds
    public DataType dataType = new DataType();
    //the ip this message was sent from
    public string ip;
    //the id of this data
    public string ID;
    //the bytes that represent the raw datatype
    public byte[] data;

    private int headerLength;

    public NetworkMessage(byte[] packet, string ip)
    {
        if (packet.Length < 7)
        {
            Debug.LogWarning("Invalid data received. Less than 7 bytes in packet: Byte amount was:" + packet.Length);
        }
        else
        {
            this.ip = ip;
            dataType = (DataType)packet[0];

            //determine length of ID
            byte IDlength = packet[1];
            //create byte array for id
            byte[] byteID = new byte[IDlength];
            if (IDlength > packet.Length - 2)
            {
                Debug.LogError("Invalid Data ID length. ID length was: " + IDlength + " message length was: " + packet.Length);
            }           
            for (int i = 0; i < byteID.Length; i++)
            {
                byteID[i] = packet[i + 2];
            }
            ID = Encoding.ASCII.GetString(byteID);

            //the length of header data (datatype and ID).
            headerLength = (2 + IDlength);
            UpdateDataBytes(packet);
        }
    }

    public NetworkMessage()
    {

    }

    //updates the bytes that represent the raw datatype given a full packet of data
    public void UpdateDataBytes(byte[] packet)
    {
        data = new byte[packet.Length - headerLength];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = packet[i + headerLength];
        }
    }

    //returns if the ID and data type match
    public bool CompareHeader(NetworkMessage msg)
    {
        return msg.ID == ID && msg.dataType == dataType;
    }

    //gets a copy of the networkMessage
    public NetworkMessage GetCopy()
    {
        NetworkMessage nt = new NetworkMessage();
        nt.dataType = dataType;
        nt.ip = ip;
        nt.ID = ID;
        nt.data = new byte[data.Length];
        nt.headerLength = headerLength;
        for (int i = 0; i < data.Length; i++)
        {
            nt.data[i] = data[i];
        }
        return nt;
    }

    public override string ToString()
    { 
        return "Network Message: dataType:[" + dataType.ToString() + "] ID:[" + ID + "] data:\n" + BitConverter.ToString(data);
    }
}
