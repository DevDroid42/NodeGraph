using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jsonTesting
{
    [Serializable]
    public class ClassStructure
    {
        public string StringTest = "This is a data structure test";
        public Node[] nodes = new Node[3];
        public ClassStructure()
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new Node();
            }
        }
    }

    [Serializable]
    public class Node
    {
        public float test = 1;
        public Input x = new Input();

        internal void UpdateNode()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class Input
    {
        public String inputDis = "This is an input";
    }

    [Serializable]
    public class Output<DataType>
    {
        //public delegate void SendData(DataType data);
    }
}