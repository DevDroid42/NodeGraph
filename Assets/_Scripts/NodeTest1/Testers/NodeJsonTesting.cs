using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using UnityEngine.UI;
using Newtonsoft.Json;

public class NodeJsonTesting : MonoBehaviour
{
    public Text viewer;
    Graph nodeGraph;

    IntConstant numNode0, numNode1;
    AddNode addNode;
    ReceiveNode receiveNode;
    // Start is called before the first frame update
    void Start()
    {

        nodeGraph = new Graph();
        numNode0 = new IntConstant();
        numNode1 = new IntConstant();
        addNode = new AddNode();
        receiveNode = new ReceiveNode();
        addNode.inputs[0].Connect(numNode0.outputs[0]);
        addNode.inputs[1].Connect(numNode1.outputs[0]);
        receiveNode.inputs[0].Connect(addNode.outputs[0]);
        nodeGraph.nodes.Add(numNode0);
        nodeGraph.nodes.Add(numNode1);
        nodeGraph.nodes.Add(addNode);
        nodeGraph.nodes.Add(receiveNode);
        Debug.Log(GraphSerialization.GraphToJson(nodeGraph));
    }

    public void NumNode0(string data)
    {
        ((IntConstant)(nodeGraph.nodes[0])).constants[0] = new IntData(int.Parse(data));
        nodeGraph.InitGraph();
    }

    public void NumNode1(string data)
    {
        ((IntConstant)(nodeGraph.nodes[1])).constants[0] = new IntData(int.Parse(data));
        nodeGraph.InitGraph();
    }

    string json = "";
    public void DeleteGraph()
    {
        json = GraphSerialization.GraphToJson(nodeGraph);
        nodeGraph = null;
        viewer.text = "";
        Debug.Log("set Graph to null. stored json: \n" + json);
    }

    public void RestoreGraph()
    {
        nodeGraph = GraphSerialization.JsonToGraph(json);
        
        Debug.Log("attempted to restore graph");
    }

    // Update is called once per frame
    void Update()
    {
        if (nodeGraph != null)
        {
            if (((ReceiveNode)(nodeGraph.nodes[3])).newData)
            {
                object data = ((ReceiveNode)(nodeGraph.nodes[3])).data;
                Debug.Log(data);
                viewer.text = data.ToString();
                ((ReceiveNode)(nodeGraph.nodes[3])).newData = false;
            }
        }
    }
}
