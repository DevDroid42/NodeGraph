using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys2
{
    public class Node
    {
        public delegate void FloatDelagate(float num);
        [JsonIgnore]
        //Frame delagate is a delage that belongs to all nodes. It is called each frame by whatever host is currently running the graph. 
        public static FloatDelagate frameDelagate;

        //GUI info
        public float xPos, yPos;
        public float xScale = 250, yScale = 10;
        public bool expanded = false;

        public List<Property> inputs = new List<Property>();
        public List<Property> outputs = new List<Property>();
        //public Port[] inputs;
        //public Port[] outputs;        
        //these are constants that will be set before runtime. Examples are colors, numbers, ip adresses, ect
        //public object[] constants;
        //public string[] constantsDisc;
        //this is data to be used for debugging. It will contain things such as connection status
        //public object[] viewableData;
        //public string[] viewableDisc;


        //the node discription for identification in JSON and GUI 
        [JsonProperty] protected string nodeDisc;

        [JsonIgnore]public bool MarkedForDeletion = false;

        public Node()
        {

        }
        
        public void InitPorts()
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].Setup();
                inputs[i].dataPort.nodeDel -= Handle;
                inputs[i].dataPort.nodeDel += Handle;
            }
            for (int i = 0; i < outputs.Count; i++)
            {
                outputs[i].Setup();
            }
        }

        //creates a property and adds it to the list. Also returns the created property to optionally be used for caching
        public Property CreateInputProperty(string ID, bool connectable, object defaultData)
        {
            //===============DUPLICATE CHECKING MIGHT BE BUSTED================================
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            //this is done both to prevent accidental duplicates and duplicates from the constructor running on deserialization.
            if (inputs.TrueForAll(prop => prop.ID != ID))
            {
                Property tempRef = new Property(ID, true, connectable, defaultData);
                inputs.Add(tempRef);
                return tempRef;
            }
            else
            {
                Debug.Log("Duplicate property found, proably serialization. Returning old ");
                return inputs.FindAll(prop => prop.ID == ID)[0];
            }
        }

        public Property CreateOutputProperty(string ID)
        {
            if (outputs.TrueForAll(prop => prop.ID != ID))
            {
                Property tempRef = new Property(ID, false, true, null);
                outputs.Add(tempRef);
                return tempRef;
            }
            else
            {
                Debug.Log("Duplicate property found, proably serialization. Returning old ");
                return outputs.FindAll(prop => prop.ID == ID)[0];
            }
        }

        //does a lookup based on the ID and returns a property 
        public Property GetInputProperty(string ID)
        {
            return GetProperty(ID, inputs);
        }
        public Property GetOutputProperty(string ID)
        {
            return GetProperty(ID, outputs);
        }

        //generic property lookup
        private Property GetProperty(string ID, List<Property> list)
        {

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == ID)
                {
                    return list[i];
                }
            }
            Debug.LogError("Could not find a property of ID: " + ID + " : in list");
            return null;
        }

        public void RemoveInputProperty(string ID)
        {
            RemoveProperty(ID, inputs);
        }

        public void RemoveOutputProperty(string ID)
        {
            RemoveProperty(ID, outputs);
        }

        //generic property lookup
        private void RemoveProperty(string ID, List<Property> list)
        {            
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == ID)
                {
                    list.RemoveAt(i);
                    return; 
                }
            }
            Debug.LogError("Could not find a property of ID: " + ID + " : in list");            
        }

        public void RemoveInputContaining(string ID)
        {
            inputs.RemoveAll(prop => prop.ID.Contains(ID));
        }

        public void RemoveOutputContaining(string ID)
        {
            outputs.RemoveAll(prop => prop.ID.Contains(ID));
        }

        public void CleanUp()
        {
            frameDelagate -= Frame;
        }

        public string GetName()
        {
            return nodeDisc;
        }

        //to be called on every node after all nodes are instantiated and connected. Usefull for sending constants down the graph
        //on startup. 
        public virtual void Init()
        {

        }

        //called on every "frame" if running in unity these frames are called each unity frame. 
        //If running standalone then a main class will invoke on a while loop. In order for this to work Base.Init must be called at some point to register the node
        public virtual void Frame(float deltaTime)
        {

        }

        //the main meathod that handles data. The index specifies the port from which the node is receiving data and the
        //object contains that data that needs to be pattern matched in order to use
        public virtual void Handle()
        {

        }
    }
}