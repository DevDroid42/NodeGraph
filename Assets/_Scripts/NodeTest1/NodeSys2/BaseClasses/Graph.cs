using nodeSys2;
using System.Collections.Generic;

namespace nodeSys2
{
    public class Graph
    {
        public List<Node> nodes = new List<Node>();

        //to be used after loading from json
        public void InitGraph()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].InitPorts();
                for (int j = 0; j < nodes[i].inputs.Length; j++)
                {
                    nodes[i].inputs[j].Reconnect();
                }
                for (int j = 0; j < nodes[i].outputs.Length; j++)
                {
                    nodes[i].outputs[j].Reconnect();
                }
            }
        }

    }
}