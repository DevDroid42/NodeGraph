using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace nodeSys2
{

    public class Property
    {
        //this class will serve to wrap the constants, constant discriptions, and ports, all in one class.
        //these will be stored in a list in the node class and will be gotten via string lookups similar to get component
        //they will return the property object so object caching can take place

        //string identifier for lookups
        public string ID;
        //data that gets returned. This needs to be locked for thread safety
        [JsonProperty]
        private object data;
        //Determines whether the port is input or output
        public bool isInput;
        //data discription. This will be used for port and editor discriptions. If not provided it will fallback to the ID
        [JsonProperty] private string disc;
        //Determines if this property has a port
        [JsonProperty] private bool connectable;
        //determines if the property has an editor assigned to it
        public bool visible;
        //determines if editor is read only. Same as setting a viewable vs constant in old system
        public bool interactable;
        //will determine the rect transform height
        public float height;
        //this port will be shown if the connectable flag is set to true.
        public Port dataPort;

        
        //what does this property internally get used as? This is set on construction via the default data type
        [JsonConverter(typeof(StringEnumConverter))]
        public EditorTypeManagement.Editor internalRepresentation;
        //The current editor specifies what editor will be used to view the data
        [JsonConverter(typeof(StringEnumConverter))]
        public EditorTypeManagement.Editor currentEditor;

        //a reference to the node this property is a part of
        private Node node;        
        //this is the type that gates input. Data types that are not this type or a sublclass of said type will be regected from input
        //=====NOTE TO SELF====== type objects are problematic across threads - https://docs.microsoft.com/en-us/dotnet/api/system.type?view=net-5.0
        [JsonProperty] private Type gateType;

        public string Disc
        {
            get => disc;
            set
            {
                
                if (dataPort != null)
                {
                    dataPort.portDisc = disc;
                }
                disc = value;
            }
        }

        public void SetupRefs(Node node)
        {
            this.node = node;
            dataPort.SetupRefs(this);
        }
       
        [JsonConstructor]
        private Property()
        {

        }

        public Property(Node nodeRef, string ID, bool isInput, bool connectable, object DefaultData, Type type)
        {
            node = nodeRef;
            visible = true;
            dataPort = new Port(this);
            dataPort.portDisc = ID;
            data = DefaultData;
            this.isInput = isInput;
            //NOTE: all output properties should be connectable
            this.connectable = connectable;
            this.ID = ID;
            disc = ID;
            gateType = type;
            if(DefaultData != null)
            {
                internalRepresentation = EditorTypeManagement.GetEditorByType(DefaultData);
                currentEditor = internalRepresentation;
            }
        }

        //this is called by the the data port object this property holds when it receives data
        public void Handle(object data)
        {
            if (Isa(data, gateType))
            {
                this.data = data;
                if (node.Runnable())
                {
                    node.Handle();
                }
            }
            else
            {
                Debug.LogWarning("Invalid data type received at property: " + ID + "\t Expected type of: ("
                    + gateType.Name + ") Instead got: (" + data.GetType().Name + ")");
            }
        }
        private bool Isa(object data, Type type)
        {            
            return type.IsAssignableFrom(data.GetType());
        }

        public void Invoke(object data)
        {
            if (isInput)
            {
                Debug.LogWarning("Attempted to invoke on an input property. This won't do anything as input ports don't send data anywhere.");
            }
            else
            {
                this.data = data;
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
        public void SetConnectable(bool _connectable)
        {
            if (_connectable)
            {
                connectable = true;
            }
            else
            {
                dataPort.Disconnect();
                connectable = false;
            }
        }

        public bool GetConnectable()
        {
            return connectable;
        }

        public bool IsConnected()
        {
            return dataPort.IsConnected();
        }
        
        //Helper Get Methods. Cast here instead of within nodes, exceptions possible
        public IEvaluable GetEvaluable()
        {
            return (IEvaluable)data;
        }

        public Pulse GetPulse()
        {
            return (Pulse)data;
        }

        public T GetDataType<T>()
        {
            return (T)data;
        }

        public bool TryGetDataType<T>(ref T reference)
        {
            if (data is T t)
            {
                reference = t;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
