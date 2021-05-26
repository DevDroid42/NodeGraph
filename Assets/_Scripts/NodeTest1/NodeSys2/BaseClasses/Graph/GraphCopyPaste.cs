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
        List<Node> selectedNodes = copyFromGraph.getSelectedNodes();
        for (int i = 0; i < selectedNodes.Count; i++)
        {
            graph.nodes.Add(selectedNodes[i]);
        }
        Clipboard = GraphSerialization.GraphToJson(graph);
    }

    public void Cut(Graph cutFromGraph)
    {
        Copy(cutFromGraph);
        List<Node> selectedNodes = cutFromGraph.getSelectedNodes();
        for (int i = 0; i < selectedNodes.Count; i++)
        {
            selectedNodes[i].Delete();
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
