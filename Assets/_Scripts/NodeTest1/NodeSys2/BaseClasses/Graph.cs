using nodeSys2;
using System.Collections.Generic;

namespace nodeSys2
{
    public class Graph
    {

        public List<Node> nodes = new List<Node>();
        public static NodeNetReceive nodeNetReceiver;

        //to be used after loading from json
        public void InitGraph()
        {
            if (nodeNetReceiver == null)
            {
                nodeNetReceiver = new NodeNetReceive();
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].InitPorts();
                for (int j = 0; j < nodes[i].inputs.Count; j++)
                {
                    nodes[i].inputs[j].dataPort.Reconnect();
                }
                for (int j = 0; j < nodes[i].outputs.Count; j++)
                {
                    nodes[i].outputs[j].dataPort.Reconnect();
                }
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Init();
            }
        }

        //handles shutting down threads and cleaning up data
        public void StopGraph()
        {
            nodeNetReceiver.Shutdown();
        }

    }
}