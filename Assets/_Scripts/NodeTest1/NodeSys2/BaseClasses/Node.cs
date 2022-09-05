using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys2
{
    public abstract class Node
    {
        public delegate void NetworkDelagate(NetworkMessage message);
        public delegate void FloatDelagate(float num);
        [JsonIgnore]
        //Frame delagate is a delage that belongs to all nodes. It is called each frame by whatever host is currently running the graph. 
        public static FloatDelagate frameDelagate;

        [JsonIgnore]
        //used to pass network data to nodes
        public static NetworkDelagate nodeNetDelagate;

        //GUI info
        public float xPos, yPos;
        public float xScale = 250, yScale = 10;
        public bool expanded = false;

        public List<Property> inputs = new List<Property>();
        public List<Property> outputs = new List<Property>();


        //the node discription for identification in JSON and GUI 
        [JsonProperty] public string nodeDisc;

        [JsonIgnore] public bool selected = false;

        [JsonIgnore]public bool MarkedForDeletion = false;
        //tracks if the handle method has already run this frame
        [JsonIgnore] private bool hasRan = false;

        //this method is called by input ports before they invoke portdel.
        public bool Runnable()
        {
            //if it's already run then return false
            if (hasRan)
            {
                return false;
            }
            else
            {
                //if it has yet to run mark as true and return true
                hasRan = true;
                return hasRan;
            }
        }

        public void ResetRunnable()
        {
            hasRan = false;
        }

        public void Delete()
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                //this will clear all delagates pointing towards this node to avoid delagates pointing to null function locations
                inputs[i].dataPort.Disconnect();
                CleanUp();
            }
            MarkedForDeletion = true;
        }

        [JsonConstructor]
        public Node()
        {

        }

        public Node(ColorVec position)
        {
            if (position == null)
            {
                position = 0;
            }
            xPos = position.rx;
            xPos = position.gy;
        }
        
        //ensures that all ports delagates are connected to each other and references to parents are properly set
        public void InitProperties()
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].SetupRefs(this);
                inputs[i].dataPort.Reconnect();
            }
            for (int i = 0; i < outputs.Count; i++)
            {
                outputs[i].SetupRefs(this);
                outputs[i].dataPort.Reconnect();
            }
        }

        //creates a property and adds it to the list. Also returns the created property to optionally be used for caching
        //if the default data is subclass of Evaluable then the gate type is set to evaluable, otherwise the gate type is of type object
        public Property CreateInputProperty(string ID, bool connectable, object defaultData, int index = -1)
        {
            //===============DUPLICATE CHECKING MIGHT BE BUSTED================================
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            //this is done both to prevent accidental duplicates and duplicates from the constructor running on deserialization.
            if (inputs.TrueForAll(prop => prop.ID != ID))
            {
                //if any subclass of evaluable is called then gate property to Evaluables, Otherwhise gate to type provided
                Type gate = defaultData.GetType().IsSubclassOf(typeof(Evaluable)) ? typeof(Evaluable) : defaultData.GetType();
                Property tempRef = new Property(this, ID, true, connectable, defaultData, gate);
                if (index < 0)
                {
                    inputs.Add(tempRef);
                }
                else
                {
                    inputs.Insert(index, tempRef);
                }
                return tempRef;
            }
            else
            {
                Debug.Log("Duplicate property found, proably serialization. Returning old ");
                return inputs.FindAll(prop => prop.ID == ID)[0];
            }
        }

        public Property CreateInputProperty(string ID, bool connectable, object defaultData, Type gate, int index = -1)
        {
            if (inputs.TrueForAll(prop => prop.ID != ID))
            {
                Property tempRef = new Property(this, ID, true, connectable, defaultData, gate);
                if (index < 0)
                {
                    inputs.Add(tempRef);
                }
                else
                {
                    inputs.Insert(index, tempRef);
                }
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
                Property tempRef = new Property(this, ID, false, true, null, typeof(object));
                outputs.Add(tempRef);
                return tempRef;
            }
            else
            {
                Debug.Log("Duplicate property found, proably serialization. Returning old ");
                return outputs.FindAll(prop => prop.ID == ID)[0];
            }
        }

        //removes property from list by reference. Returns true if reference was found and removed, false otherwise
        //also sets input reference to null
        public bool RemoveProperty(Property prop)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if(inputs[i] == prop)
                {
                    inputs[i].SetConnectable(false);
                    inputs.RemoveAt(i);                    
                    return true;
                }
            }
            for (int i = 0; i < outputs.Count; i++)
            {
                if (outputs[i] == prop)
                {
                    outputs[i].SetConnectable(false);
                    outputs.RemoveAt(i);                    
                    return true;
                }
            }
            return false;
        }

        public void CleanUp()
        {
            frameDelagate -= Frame;
        }

        public string GetName()
        {
            return nodeDisc;
        }

        //to be called on every node after all nodes are instantiated and connected. Outputs should never be invoked in init as
        //it is not garanteed that all nodes downstream are initialized yet
        public virtual void Init()
        {

        }

        //this init method runs after all nodes have had their init method run. This is useful for sending constants down the graph
        //as all nodes are garanteed to be initialized. 
        public virtual void Init2()
        {

        }

        //called on every "frame" if running in unity these frames are called each unity frame. 
        //If running standalone then a main class will invoke on a while loop. In order for this to work Base.Init must be called at some point to register the node
        public virtual void Frame(float deltaTime)
        {

        }

        //gets registered with the network receive delagate. This will be called each time data from the network is received. It's up to each node to 
        //decide if they want to use the data or not
        public virtual void ReceiveData(NetworkMessage message)
        {

        }

        //the main meathod that handles data. The index specifies the port from which the node is receiving data and the
        //object contains that data that needs to be pattern matched in order to use
        public virtual void Handle()
        {

        }
    }
}