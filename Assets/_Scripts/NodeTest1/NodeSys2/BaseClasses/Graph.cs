using nodeSys2;
using System.Collections.Generic;
using Newtonsoft.Json;

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
                nodes[i].InitProperties();                
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