using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys2
{
    public class Port
    {
        //the index will be used to identify the data source when proccesing in nodes        
        [JsonProperty] private int index;
        public delegate void PortDelagate(int index, object data);

        //this delagate is used to invoke method calls in nodes as well as other connected ports
        [JsonIgnore]
        public PortDelagate portDel;

        //used to save the reference to the port that last connected
        [JsonProperty(IsReference = true)]
        public Port connectedPort;

        //used to track if there an existing connection to prevent multiple connections to the same port                
        private bool connected = false;

        public bool isConnected()
        {
            return connected;
        }

        public Port(int index)
        {
            this.index = index;
        }

        //Connects another ports output to this ports input. Ports can only have one input source but can 
        //have theoretically infinite outputs
        public void Connect(Port port)
        {
            if (!connected)
            {
                port.portDel += Handle;
                connected = true;
                connectedPort = port;
            }
        }

        //used when loading from json. Delagates are not serialized so connections will need to be 
        //re-established
        public void Reconnect()
        {
            if (connectedPort != null)
            {
                connectedPort.portDel -= Handle;
                connectedPort.portDel += Handle;
                connected = true;
            }
        }

        //Removes the reference from the connected port. 
        public void Disconnect()
        {
            if (connected)
            {
                connectedPort.portDel -= Handle;
                connectedPort = null;
                connected = false;
            }

        }

        //wrapper meathod to invoke the delagate without needing the index
        public void Invoke(object data)
        {
            if (portDel != null)
            {
                portDel.Invoke(index, data);
            }
        }

        //this method is invoked from another connected port delagate. It then takes the data from that 
        //delagate and invokes it's own delagate attaching it's index to be used for processing in nodes
        private void Handle(int OtherIndex, object data)
        {
            if (portDel != null)
            {
                portDel.Invoke(index, data);
            }
        }
    }
}