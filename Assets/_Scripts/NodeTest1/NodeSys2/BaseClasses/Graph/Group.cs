using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
public class Group
{
    private List<GroupInputNode> inputNodes = new List<GroupInputNode>();
    private List<GroupOutputNode> outputNodes = new List<GroupOutputNode>();
    private ColorVec vector = new ColorVec(0);
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

    //assigns the index of all output nodes in this group
    public void AssignOutputIndex(int index)
    {
        //iterate over all nodes
        for (int i = 0; i < graph.nodes.Count; i++)
        {
            if(graph.nodes[i] is GroupOutputNode output)
            {
                output.instanceIndex = index;
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
    public void SetVector(ColorVec vector)
    {
        this.vector = vector;
    }

    public void UpdateGraph()
    {
        graph.UpdateGraph();
    }

    public void PublishToGraph(string ID, object data)
    {
        foreach (GroupInputNode node in inputNodes)
        {
            //if instanced data then run handle on data that is evaluated instead.
            if (ID == node.getName())
            {
                if ((GroupInputNode.InputType)node.inputType.GetData() == GroupInputNode.InputType.Instanced && data is Evaluable eData)
                {
                    node.input.Handle(new EvaluableColorVec(eData.EvaluateColor(vector)));
                }
                else
                {
                    node.input.Handle(data);
                }
            }
        }
    }
}
