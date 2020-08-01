using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys
{
    public class NodeManager
    {
        public List<Node> nodes = new List<Node>();

        public void updateNodes()
        {
            foreach (Node node in nodes)
            {
                node.UpdateNode();
            }
        }
    }
}
