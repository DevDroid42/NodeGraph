using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using UnityEditor;
using System;

public class GUIGraph : MonoBehaviour
{
    Graph nodeGraph;
    public Transform background;
    //reference to node prefab
    public GameObject baseNode;
    public GameObject baseLineRenderer;
    public List<GameObject> guiNodes = new List<GameObject>();
    public EditorNameLink[] editorTypes;
    public static Dictionary<string, GameObject> editors = new Dictionary<string, GameObject>();
    private void Awake()
    {
        for (int i = 0; i < editorTypes.Length; i++)
        {
            editors.Add(editorTypes[i].name, editorTypes[i].editor);
        }
    }    

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

        UpdateGUI();
        //graph = new Graph();
    }

    // Update is called once per frame
    void Update()
    {
        //MakeConnections();
    }

    public void UpdateGraph()
    {
        nodeGraph.InitGraph();
    }

    public void UpdateGUI()
    {
        for (int i = 0; i < guiNodes.Count; i++)
        {
            Destroy(guiNodes[i]);
        }
        guiNodes.Clear();

        
        for (int i = 0; i < nodeGraph.nodes.Count; i++)
        {
            GameObject node = Instantiate(baseNode, background);
            node.SetActive(true);
            node.GetComponent<GUINode>().SetupNode(nodeGraph.nodes[i]);
            guiNodes.Add(node);
        }
        
    }

    public List<GameObject> lines = new List<GameObject>();
    public void MakeConnections()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i]);
        }
        lines.Clear();

        //game object references to ports
        GameObject outPortGO, inPortGO;
        //direct references to nodeSys ports. 
        Port outPort, inPort;

        //go through all nodes
        for (int i = 0; i < nodeGraph.nodes.Count; i++)
        {
            //go through all of each nodes input ports
            for (int j = 0; j < nodeGraph.nodes[i].inputs.Length; j++)
            {
                //check if the port has a connection
                if (nodeGraph.nodes[i].inputs[j].isConnected())
                {
                    //if it does store the reference to the port and it's connection
                    inPort = nodeGraph.nodes[i].inputs[j];
                    outPort = nodeGraph.nodes[i].inputs[j].connectedPort;
                    inPortGO = findGUI(inPort);
                    outPortGO = findGUI(outPort);
                    DrawLines(inPortGO, outPortGO);
                }
            }
        }

        GameObject findGUI(Port port)
        {
            //go through all gui nodes to search for node with the reference to the same port
            for (int i = 0; i < guiNodes.Count; i++)
            {
                GUINode guiNode = guiNodes[i].GetComponent<GUINode>();
                for (int j = 0; j < guiNode.inputPorts.Length; j++)
                {
                    //if the gui port has the same port referenced then store it 
                    if (guiNode.inputPorts[j].GetComponent<GUIPort>().portRef == port)
                    {
                        return guiNode.inputPorts[j];
                    }
                }
                for (int j = 0; j < guiNode.outputPorts.Length; j++)
                {
                    if (guiNode.outputPorts[j].GetComponent<GUIPort>().portRef == port)
                    {
                        return guiNode.outputPorts[j];
                    }
                }
            }
            Debug.LogError("a port on node existed in nodeSys without a matching GUI port");
            return null;
        }
    }

    void DrawLines(GameObject obj1, GameObject obj2)
    {
        Vector3[] points1 = new Vector3[4];
        obj1.GetComponent<RectTransform>().GetWorldCorners(points1);
        Vector3[] points2 = new Vector3[4];
        obj2.GetComponent<RectTransform>().GetWorldCorners(points2);

        Vector3[] points = new Vector3[2];
        points[0] = average(points1);
        points[1] = average(points2);
        GameObject lr = Instantiate(baseLineRenderer);
        lr.GetComponent<LineRenderer>().SetPositions(points);
        lines.Add(lr);
        

        Vector3 average(Vector3[] vectors)
        {
            float x = 0, y = 0, z = 0, count = 0;
            for (int i = 0; i < vectors.Length; i++)
            {
                x += vectors[i].x;
                y += vectors[i].y;
                z += vectors[i].z;
                count++;
            }
            return new Vector3(x / count, y / count, 0.99f);
        }
    }
}

[Serializable]
public class EditorNameLink
{
    public GameObject editor;
    public string name;
}
