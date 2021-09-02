using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
public class Group
{
    private List<GroupInputNode> inputNodes = new List<GroupInputNode>();
    private List<GroupOutputNode> outputNodes = new List<GroupOutputNode>();
    private Graph graph;

    public Group(Graph graph, GroupOutputNode.GroupOutDelegate callback)
    {
        this.graph = graph;
        graph.InitGraph();

        //iterate over all nodes
        for (int i = 0; i < graph.nodes.Count; i++)
        {
            switch (graph.nodes[i])
            {
                case GroupInputNode node:
                    {
                        inputNodes.Add(node);
                        break;
                    }
                case GroupOutputNode node:
                    {
                        node.outDel = null;
                        node.outDel += callback;
                        outputNodes.Add(node);
                        break;
                    }
            }
        }
    }

    public List<string> GetInputTags()
    {
        return GetNames(inputNodes);
    }
    public List<string> GetOutputTags()
    {
        return GetNames(outputNodes);
    }

    private List<string> GetNames<T>(List<T> list)
    {
        List<string> inputTags = new List<string>();
        foreach (INameable node in list)
        {
            inputTags.Add(node.getName());
        }
        return inputTags;
    }

    public void PublishToGraph(string ID, object data)
    {
        foreach (GroupInputNode node in inputNodes)
        {
            //if instanced data then run handle on data that is evaluated instead.
            if(ID == node.getName())
            {
                node.input.Handle(data);
            }
        }
    }
}
