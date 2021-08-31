using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys2
{
    //the purpose of the port class is 
    public class Port
    {
        //used for sending data between ports and properties
        private delegate void PortDelagate(object data);

        //this delagate is used to pass data between ports
        [JsonIgnore]
        private PortDelagate portDel;

        //used to save the reference to the port that last connected
        [JsonProperty(IsReference = true)]
        public Port connectedPort;

        public string portDisc = "DefaultDisc";

        //used to track if there an existing connection to prevent multiple connections to the same port                
        private bool connected = false;

        //the reference to the property that this port is atatched to
        private Property property;

        public bool IsConnected()
        {
            return connected;
        }

        public void SetupRefs(Property property)
        {
            this.property = property;
        }

        [JsonConstructor]
        private Port()
        {

        }

        public Port(Property property)
        {
            this.property = property;
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
                //invoke null to clear out references back to data
                //connectedPort.portDel.Invoke(new object());
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
                portDel.Invoke(data);
            }
        }

        //this method is invoked from another connected port delagate. This means data was received on an input port.
        //we should send this data to the ports property.
        private void Handle(object data)
        {
            if (data is Evaluable d)
            {
                property.Handle(d.GetCopy());
            }
            else
            {
                property.Handle(data);
            }
        }
    }
}