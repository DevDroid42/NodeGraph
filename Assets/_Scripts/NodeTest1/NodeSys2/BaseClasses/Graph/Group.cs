using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
public class Group
{
    private List<InstancedPulseNode> pulseNodes = new List<InstancedPulseNode>();
    private List<GroupInputNode> inputNodes = new List<GroupInputNode>();
    private Dictionary<string, List<GroupInputNode>> inputNodesDict = new Dictionary<string, List<GroupInputNode>>();
    private List<GroupOutputNode> outputNodes = new List<GroupOutputNode>();
    private List<InfoNode> infoNodes = new List<InfoNode>();
    private Dictionary<string, List<InfoNode>> infoNodesDict = new Dictionary<string, List<InfoNode>>();

    private float vector = 0;
    private Graph graph;

    public Group(Graph graph, GroupOutputNode.GroupOutDelegate callback)
    {
        this.graph = graph;
        //iterate over all nodes
        for (int i = 0; i < graph.nodes.Count; i++)
        {
            switch (graph.nodes[i])
            {
                case GroupInputNode node:
                    {
                        inputNodes.Add(node);
                        RegisterInputNode(node);
                        break;
                    }
                case InstancedPulseNode node:
                    {
                        pulseNodes.Add(node);
                        break;
                    }
                case GroupOutputNode node:
                    {
                        node.outDel = null;
                        node.outDel += callback;
                        outputNodes.Add(node);
                        break;
                    }
                case InfoNode node:
                    {
                        infoNodes.Add(node);
                        RegisterInfoNode(node);
                        break;
                    }
            }
        }
    }

    public void Init()
    {
        for (int i = 0; i < graph.nodes.Count; i++)
        {
            graph.nodes[i].InitProperties();
        }
        for (int i = 0; i < graph.nodes.Count; i++)
        {
            graph.nodes[i].Init();
        }
    }
    public void Init2()
    {
        for (int i = 0; i < graph.nodes.Count; i++)
        {
            graph.nodes[i].Init2();
        }
    }

    private void RegisterInfoNode(InfoNode node)
    {
        foreach (string name in node.GetNames())
        {
            if (!infoNodesDict.ContainsKey(name))
            {
                infoNodesDict.Add(name, new List<InfoNode>());
            }
            infoNodesDict[name].Add(node);
        }   
    }

    public void PublishInfoNodeData()
    {
        for (int i = 0; i < infoNodes.Count; i++)
        {
            infoNodes[i].Handle();
        }
    }

    private void RegisterInputNode(GroupInputNode node)
    {
        if(!inputNodesDict.ContainsKey(node.getName()))
        {
            inputNodesDict.Add(node.getName(), new List<GroupInputNode>());
        }
        inputNodesDict[node.getName()].Add(node);
    }

    //assigns the index of all output nodes in this group
    public void AssignInstanceInfo(int index, int count, float ratio)
    {
        //iterate over all nodes
        foreach (GroupOutputNode outputNode in outputNodes)
        {
            outputNode.instanceIndex = index;
        }
        PublishToGraph(InstanceInfoNode.ratioKey, new EvaluableFloat(index));
        PublishToGraph(InstanceInfoNode.countKey, new EvaluableFloat(count));
        PublishToGraph(InstanceInfoNode.ratioKey, new EvaluableFloat(ratio));
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
    public void SetVector(float vector)
    {
        this.vector = vector;
    }

    public void UpdateGraph()
    {
        graph.UpdateGraph();
    }

    private void PublishInputNodes(string id, object data)
    {
        if (!inputNodesDict.ContainsKey(id)) return;

        List<GroupInputNode> inputNodes = inputNodesDict[id];
        for (int i = 0; i < inputNodes.Count; i++)
        {
            GroupInputNode node = inputNodes[i];
            if ((GroupInputNode.InputType)node.inputType.GetData() == GroupInputNode.InputType.Instanced && data is IEvaluable eData)
            {
                node.input.Handle(new EvaluableColorVec(eData.EvaluateColor(vector)));
            }
            else
            {
                node.input.Handle(data);
            }
        }
    }

    private void PublishInfoNodes(string id, object data)
    {
        if (!infoNodesDict.ContainsKey(id)) return;

        List<InfoNode> infoNodes = infoNodesDict[id];
        for (int i = 0; i < infoNodes.Count; i++)
        {
            infoNodes[i].PublishData(id, data);
            infoNodes[i].Handle();
        }
    }

    public void PublishToGraph(string id, object data)
    {
        PublishInputNodes(id, data);
        PublishInfoNodes(id, data);
    }

    public void PulseGraph()
    {
        foreach(InstancedPulseNode node in pulseNodes)
        {
            //Debug.Log("pulsedGraph");
            node.Handle();
        }
    }
}
