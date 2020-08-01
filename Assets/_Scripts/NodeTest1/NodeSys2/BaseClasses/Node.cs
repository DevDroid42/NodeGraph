using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys2
{
    public class Node
    {
        public Port[] inputs;
        public Port[] outputs;

        //the node discription for identification in JSON and GUI 
        [JsonProperty] protected string nodeDisc;

        //assigns indexes to all of the ports and links the input ports to the handle method
        public void InitPorts()
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] == null)
                    inputs[i] = new Port(i);
                inputs[i].portDel += Handle;
            }

            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] == null)
                    outputs[i] = new Port(i);
            }
        }

        public string GetName()
        {
            return nodeDisc;
        }

        //to be called on every node after all nodes are instantiated and connected. Usefull for sending constants down the graph
        //on startup
        public virtual void IntialInvokes()
        {

        }

        //the main meathod that handles data. The index specifies the port from which the node is receiving data and the
        //object contains that data that needs to be pattern matched in order to use
        public virtual void Handle(int index, object data)
        {

        }
    }
}