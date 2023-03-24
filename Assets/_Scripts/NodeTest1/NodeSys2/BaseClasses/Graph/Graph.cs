using nodeSys2;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace nodeSys2
{
    public class Graph
    {
        public static NodeCollections nodeCollection;
        public static Recorder recorder = new Recorder();
        public string graphName = "";
        public List<Node> nodes = new List<Node>();        
        public static NodeNetReceive nodeNetReceiver;

        public void ResetNodeCollections()
        {
            nodeCollection = new NodeCollections();
        }

        //to be used after loading from json
        public void InitGraph()
        {
            recorder = new Recorder();
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
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Init2();
            }
        }

        public void UpdateGraph()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                //nodes only run once per data input received. 
                nodes[i].ResetRunnable();
            }
        }

        public List<Node> getSelectedNodes()
        {
            List<Node> selectedNodes = new List<Node>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].selected)
                {
                    selectedNodes.Add(nodes[i]);
                }
            }
            return selectedNodes;
        }

        //appends nodes from another graph to this graph. Shared references between graphs will not be appended.
        //should this graph share references to another graph a warning will be printed.
        public void MergeGraph(Graph otherGraph)
        {
            //because we are changing the size of an array while looping it we get the initial count first and only check that
            //this means we assume the other graph argument does not have any duplicate references in its node list
            int initalCount = nodes.Count;
            for (int otherIndex = 0; otherIndex < otherGraph.nodes.Count; otherIndex++)
            {
                bool referenceFound = false;
                for (int thisIndex = 0; thisIndex < initalCount; thisIndex++)
                {
                    if(nodes[thisIndex] == otherGraph.nodes[otherIndex])
                    {
                        referenceFound = true;
                        Debug.LogWarning("Duplicate reference found on graph merge. Multiple of discription: " + otherGraph.nodes[otherIndex].nodeDisc);
                        break;
                    }
                }
                if (!referenceFound)
                {
                    nodes.Add(otherGraph.nodes[otherIndex]);
                }
            }
        }


        //handles shutting down threads and cleaning up data
        public void StopGraph()
        {
            nodeNetReceiver.Shutdown();
        }

    }

    public class NodeCollections
    {
        private HashSet<RecordingNode> recordingNodes = new HashSet<RecordingNode>();
        public void RegisterRecordingNode(RecordingNode node)
        {
            recordingNodes.Add(node);
        }

        public HashSet<RecordingNode> GetRecordingNodes()
        {
            return recordingNodes;
        }

        private Dictionary<string, List<NetReceiveNode>> netReceiveNodes = new Dictionary<string, List<NetReceiveNode>>();
        public void RegisterNetReceiveNode(string ID, string dataType, NetReceiveNode node)
        {
            string key = ID + dataType;
            if (!netReceiveNodes.ContainsKey(key))
            {
                netReceiveNodes.Add(key, new List<NetReceiveNode>());
                
            }            
            netReceiveNodes[key].Add(node);
        }

        public List<NetReceiveNode> GetNetReceiveNodes(string ID, string dataType)
        {
            string key = ID + dataType;
            if (netReceiveNodes.ContainsKey(key))
            {
                return netReceiveNodes[key];
            }
            else
            {
                return new List<NetReceiveNode>();
            }
        }
    }
}