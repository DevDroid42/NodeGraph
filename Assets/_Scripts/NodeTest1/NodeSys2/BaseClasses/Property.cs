using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace nodeSys2
{

    public class Property
    {
        //this class will serve to wrap the constants, constant discriptions, and ports, all in one class.
        //these will be stored in a list in the node class and will be gotten via string lookups similar to get component
        //they will return the property object so object caching can take place

        //string identifier for lookups
        public string ID;
        //data that gets returned
        [JsonProperty]
        private object data;
        //Determines whether the port is input or output
        public bool isInput;
        //data discription. This will be used for port and editor discriptions. If not provided it will fallback to the ID
        [JsonProperty]private string disc;
        //Determines i this property has a port
        public bool connectable;
        //determines if the property has an editor assigned to it
        public bool visible;
        //determines if editor is read only. Same as setting a viewable vs constant in old system
        public bool interactable;
        //will determine the rect transform height
        public float height;
        //this port will be shown if the connectable flag is set to true.
        //Something important to note: The index of this port will be used when routing the data back to this property
        public Port dataPort;
        //this is the type that gates input. Data types that are not his type or a sublclass of said type will be regected from input
        //=====NOTE TO SELF====== type objects are problematic across threads - https://docs.microsoft.com/en-us/dotnet/api/system.type?view=net-5.0
        [JsonProperty] private Type gateType;

        public string Disc
        {
            get => disc;
            set
            {
                //
                if (dataPort != null)
                {
                    dataPort.portDisc = disc;
                }
                disc = value;
            }
        }

        [JsonConstructor]
        public Property()
        {

        }

        public Property(string ID, bool isInput, bool connectable, object DefaultData)
        {
            visible = true;
            dataPort = new Port();
            dataPort.portDisc = ID;
            data = DefaultData;
            this.isInput = isInput;
            //NOTE: all output properties should be connectable
            this.connectable = connectable;
            this.ID = ID;
            this.disc = ID;
            //by default allow all data types into the node
            gateType = typeof(object);
        }

        public Property(string ID, bool isInput, bool connectable, object DefaultData, Type type) : this(ID, isInput, connectable, DefaultData)
        {
            gateType = type;
        }

        //delagates can't be serialized so they need to be re-assigned here
        public void Setup()
        {
            if (isInput)
            {
                //remove first because if it's not already there it doesn't do anything. 
                dataPort.portDel -= Handle;
                dataPort.portDel += Handle;
            }
        }

        public void Handle(object data)
        {
            if (isa(data, gateType))
            {
                this.data = data;
            }
            else
            {
                Debug.LogWarning("Invalid data type received at property: " + ID + "\t Expected type of: ("
                    + gateType.Name + ") Instead got: (" + data.GetType().Name + ")");
            }
        }
        private bool isa(object data, Type type)
        {
            return data.GetType() == type || data.GetType().IsSubclassOf(type);
        }

        public void Invoke(object data)
        {
            if (isInput)
            {
                Debug.LogWarning("Attempted to invoke on an input property. This won't do anything as input ports don't send data anywhere.");
            }
            else
            {
                dataPort.Invoke(data);
            }
        }

        public object GetData()
        {
            if (isInput)
            {
                return data;
            }
            else
            {
                Debug.Log("Attempted to get data from output property");
                return new object();
            }
        }

        public void SetData(object data)
        {
            this.data = data;
        }

        //will auto dissconnect if connected and make the port not connectable
        public void Disconnectable()
        {
            dataPort.Disconnect();
            connectable = false;
        }

        public bool IsConnected()
        {
            return dataPort.IsConnected();
        }

        public bool TryGetDataType<T>(ref T reference)
        {
            if (data is T)
            {
                reference = (T)data;
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
