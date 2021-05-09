using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

//stores data along with data descriptors. The descriptors make up the start of packets
//(headers) and are parsed into variables in this class
public class NetworkMessage
{
    public enum DataType { Value, ValueArray, ColorArray };
    //the dataType this message holds
    public DataType dataType = new DataType();
    //the ip this message was sent from
    public string ip;
    //the id of this data
    public string ID;
    //the bytes that represent the raw datatype
    public byte[] data;

    private int headerLength;

    public NetworkMessage(byte[] data, string ip)
    {
        if (data.Length < 7)
        {
            Debug.LogWarning("Invalid data received. Less than 7 bytes in packet: Byte amount was:" + data.Length);
        }
        else
        {
            this.ip = ip;
            dataType = (DataType)data[0];

            //determine length of ID
            byte IDlength = data[1];
            //create byte array for id
            byte[] byteID = new byte[IDlength];
            if (IDlength > data.Length - 2)
            {
                Debug.LogError("Invalid Data ID length. ID length was: " + IDlength + " message length was: " + data.Length);
            }
            ID = "";
            for (int i = 0; i < byteID.Length; i++)
            {
                ID += byteID[i + 2];
            }
            //the length of header data (datatype and ID).
            headerLength = data.Length - (2 + IDlength);
            UpdateDataBytes(data);
        }
    }

    public NetworkMessage()
    {

    }

    //updates the bytes that represent the raw datatype given a full packet of data
    public void UpdateDataBytes(byte[] data)
    {
        this.data = new byte[headerLength];
        for (int i = 0; i < data.Length - headerLength; i++)
        {
            this.data[i] = data[i + headerLength];
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
}
