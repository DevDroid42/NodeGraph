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
        public static FloatDelagate frameDelagate;

        //GUI info
        public float xPos, yPos;
        public float xScale = 250, yScale = 10;
        public bool expanded = false;

        public Port[] inputs;
        public Port[] outputs;        
        //these are constants that will be set before runtime. Examples are colors, numbers, ip adresses, ect
        public object[] constants;
        public string[] constantsDisc;
        //this is data to be used for debugging. It will contain things such as connection status
        public object[] viewableData;
        public string[] viewableDisc;


        //the node discription for identification in JSON and GUI 
        [JsonProperty] protected string nodeDisc;

        [JsonIgnore]public bool MarkedForDeletion = false;

        //assigns indexes to all of the ports and links the input ports to the handle method
        public void InitPorts(int inputCount, int outputCount)
        {
            inputs = new Port[inputCount];
            outputs = new Port[outputCount];

            InitPorts();
        }        

        public void InitPorts()
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] == null)
                    inputs[i] = new Port(i);
                //we remove the handle first. If there is no handle already attached then it does nothing.
                //If there is then it prevents dupicates
                inputs[i].portDel -= Handle;
                inputs[i].portDel += Handle;
            }

            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] == null)
                    outputs[i] = new Port(i);
            }
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
        //on startup
        public virtual void Init()
        {
            //removing does nothing if it's not already assigned so remove first incase it's already there to prevent dulplicates
            frameDelagate -= Frame;
            frameDelagate += Frame;
        }

        //called on every "frame" if running in unity these frames are called each unity frame. 
        //If running standalone then a main class will invoke on a while loop
        public virtual void Frame(float deltaTime)
        {

        }

        //the main meathod that handles data. The index specifies the port from which the node is receiving data and the
        //object contains that data that needs to be pattern matched in order to use
        public virtual void Handle(int index, object data)
        {

        }
    }
}