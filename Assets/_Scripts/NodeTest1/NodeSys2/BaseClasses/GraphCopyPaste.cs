using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class GraphCopyPaste
{
    //stores the json of a graph object with selected objects
    private string Clipboard = "";

    public void Copy(Graph copyFromGraph)
    {
        //temporary graph that will only contain selected nodes from 
        Graph graph = new Graph();
        for (int i = 0; i < copyFromGraph.nodes.Count; i++)
        {
            if (copyFromGraph.nodes[i].selected)
            {
                graph.nodes.Add(copyFromGraph.nodes[i]);
            }
        }
        Clipboard = GraphSerialization.GraphToJson(graph);
    }

    public void Cut(Graph cutFromGraph)
    {
        Copy(cutFromGraph);
        for (int i = 0; i < cutFromGraph.nodes.Count; i++)
        {
            if (cutFromGraph.nodes[i].selected)
            {
                cutFromGraph.nodes[i].Delete();
            }
        }
    }

    //pastes the nodes in the clipboard to the graph given
    public void Paste(Graph pasteToGraph)
    {
        Debug.Log(Clipboard);
        if (Clipboard != "")
        {
            pasteToGraph.MergeGraph(GraphSerialization.JsonToGraph(Clipboard));
        }
    }
}
