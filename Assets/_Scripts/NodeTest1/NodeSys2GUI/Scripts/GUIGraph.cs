using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using UnityEditor;
using System;
using UnityEngine.Events;

public class GUIGraph : MonoBehaviour
{
    Graph nodeGraph;
    public Transform background;
    //reference to node prefab
    public GameObject baseNode;
    public GameObject baseLineRenderer;
    public float LineZOffset;
    public Transform lineRendererParent;
    public List<GameObject> guiNodes = new List<GameObject>();
    public EditorNameLink[] editorTypes;
    public EditorNameLink[] viewerTypes;
    public static Dictionary<string, GameObject> editors = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> viewers = new Dictionary<string, GameObject>();
    public static UnityEvent updateGraphGUI;
    [Header("Colors")]
    public Color DefaultColor;
    public Color SelectedColor;

    private void Awake()
    {        
        for (int i = 0; i < editorTypes.Length; i++)
        {
            editors.Add(editorTypes[i].name, editorTypes[i].editor);
        }
        for (int i = 0; i < viewerTypes.Length; i++)
        {
            viewers.Add(viewerTypes[i].name, viewerTypes[i].editor);
        }
        if (updateGraphGUI == null)
        {
            updateGraphGUI = new UnityEvent();
            updateGraphGUI.AddListener(UpdateGUI);
        }
    }    

    IntConstant numNode0, numNode1;
    AddNode addNode;
    ViewerNode receiveNode;
    // Start is called before the first frame update
    void Start()
    {
        //nodeGraph = GraphSerialization.JsonToGraph("{\"$id\":\"1\",\"nodes\":[{\"$id\":\"2\",\"$type\":\"IntConstant, Assembly-CSharp\",\"xPos\":-714.2998,\"yPos\":-83.2910156,\"xScale\":250.0,\"yScale\":140.36792,\"expanded\":true,\"inputs\":[],\"outputs\":[{\"$id\":\"3\",\"index\":0,\"connectedPort\":null,\"portDisc\":null}],\"constants\":[{\"$id\":\"4\",\"$type\":\"nodeSys2.IntData, Assembly-CSharp\",\"num\":6}],\"constantsDisc\":[\"IntConstant\"],\"viewableData\":null,\"viewableDisc\":null,\"nodeDisc\":\"IntConstant\"},{\"$id\":\"5\",\"$type\":\"IntConstant, Assembly-CSharp\",\"xPos\":-655.5957,\"yPos\":149.014648,\"xScale\":250.0,\"yScale\":140.36792,\"expanded\":true,\"inputs\":[],\"outputs\":[{\"$id\":\"6\",\"index\":0,\"connectedPort\":null,\"portDisc\":null}],\"constants\":[{\"$id\":\"7\",\"$type\":\"nodeSys2.IntData, Assembly-CSharp\",\"num\":3}],\"constantsDisc\":[\"IntConstant\"],\"viewableData\":null,\"viewableDisc\":null,\"nodeDisc\":\"IntConstant\"},{\"$id\":\"8\",\"$type\":\"AddNode, Assembly-CSharp\",\"xPos\":-266.05957,\"yPos\":33.0859375,\"xScale\":250.0,\"yScale\":205.73584,\"expanded\":false,\"inputs\":[{\"$id\":\"9\",\"index\":0,\"connectedPort\":{\"$ref\":\"6\"},\"portDisc\":null},{\"$id\":\"10\",\"index\":1,\"connectedPort\":{\"$ref\":\"3\"},\"portDisc\":null}],\"outputs\":[{\"$id\":\"11\",\"index\":0,\"connectedPort\":null,\"portDisc\":null}],\"constants\":null,\"constantsDisc\":null,\"viewableData\":null,\"viewableDisc\":null,\"nodeDisc\":\"Add Node\"},{\"$id\":\"12\",\"$type\":\"ViewerNode, Assembly-CSharp\",\"data\":11,\"xPos\":470.7832,\"yPos\":-44.4785156,\"xScale\":250.0,\"yScale\":140.36792,\"expanded\":true,\"inputs\":[{\"$id\":\"13\",\"index\":0,\"connectedPort\":{\"$id\":\"14\",\"index\":0,\"connectedPort\":null,\"portDisc\":null},\"portDisc\":null}],\"outputs\":[],\"constants\":null,\"constantsDisc\":null,\"viewableData\":[11],\"viewableDisc\":[\"data:\"],\"nodeDisc\":\"Viewer node\"},{\"$id\":\"15\",\"$type\":\"IntConstant, Assembly-CSharp\",\"xPos\":-322.228516,\"yPos\":-246.139648,\"xScale\":250.0,\"yScale\":140.36792,\"expanded\":true,\"inputs\":[],\"outputs\":[{\"$id\":\"16\",\"index\":0,\"connectedPort\":null,\"portDisc\":null}],\"constants\":[{\"$id\":\"17\",\"$type\":\"nodeSys2.IntData, Assembly-CSharp\",\"num\":2}],\"constantsDisc\":[\"IntConstant\"],\"viewableData\":null,\"viewableDisc\":null,\"nodeDisc\":\"IntConstant\"},{\"$id\":\"18\",\"$type\":\"AddNode, Assembly-CSharp\",\"xPos\":114.518555,\"yPos\":-64.8125,\"xScale\":250.0,\"yScale\":205.73584,\"expanded\":false,\"inputs\":[{\"$id\":\"19\",\"index\":0,\"connectedPort\":{\"$ref\":\"11\"},\"portDisc\":null},{\"$id\":\"20\",\"index\":1,\"connectedPort\":{\"$ref\":\"16\"},\"portDisc\":null}],\"outputs\":[{\"$ref\":\"14\"}],\"constants\":null,\"constantsDisc\":null,\"viewableData\":null,\"viewableDisc\":null,\"nodeDisc\":\"Add Node\"}]}");    
        nodeGraph = new Graph();
        numNode0 = new IntConstant();
        numNode1 = new IntConstant();
        addNode = new AddNode();
        receiveNode = new ViewerNode();
        addNode.inputs[0].Connect(numNode0.outputs[0]);
        addNode.inputs[1].Connect(numNode1.outputs[0]);
        addNode.inputs[1].Disconnect();
        receiveNode.inputs[0].Connect(addNode.outputs[0]);
        nodeGraph.nodes.Add(numNode0);
        nodeGraph.nodes.Add(numNode1);
        nodeGraph.nodes.Add(addNode);
        nodeGraph.nodes.Add(receiveNode);
        nodeGraph.nodes.Add(new IntConstant());
        nodeGraph.nodes.Add(new AddNode());
        Debug.Log(GraphSerialization.GraphToJson(nodeGraph));

        UpdateGUI();
        //graph = new Graph();
    }

    // Update is called once per frame
    void Update()
    {
        //MakeConnections();
    }

    public void PrintJson()
    {
        Debug.Log(GraphSerialization.GraphToJson(nodeGraph));
    }

    public void UpdateGUI()
    {
        nodeGraph.InitGraph();
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
        MakeConnections();
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
                    //save the reference to the GUIPortHolders
                    GUIPortHolder portHolder1 = findGUI(inPort).GetComponent<GUIPortHolder>();
                    inPortGO = portHolder1.Port;
                    portHolder1.SetupPortPos();

                    GUIPortHolder portHolder2 = findGUI(outPort).GetComponent<GUIPortHolder>();
                    outPortGO = portHolder2.Port;
                    portHolder2.SetupPortPos();
                    
                    lines.Add(DrawLinesFromRect(inPortGO, outPortGO, baseLineRenderer, lineRendererParent));
                }
            }
        }

        GameObject findGUI(Port port)
        {
            //go through all gui nodes to search for node with the reference to the same port
            for (int i = 0; i < guiNodes.Count; i++)
            {
                GUINode guiNode = guiNodes[i].GetComponentInChildren<GUINode>();
                for (int j = 0; j < guiNode.inputPorts.Length; j++)
                {
                    //if the gui port has the same port referenced then store it 
                    if (guiNode.inputPorts[j].GetComponentInChildren<GUIPort>().portRef == port)
                    {
                        return guiNode.inputPorts[j];
                    }
                }
                for (int j = 0; j < guiNode.outputPorts.Length; j++)
                {
                    if (guiNode.outputPorts[j].GetComponentInChildren<GUIPort>().portRef == port)
                    {
                        return guiNode.outputPorts[j];
                    }
                }
            }
            Debug.LogError("a port on node existed in nodeSys without a matching GUI port");
            return null;
        }
    }

    public static GameObject DrawLinesFromRect(GameObject obj1, GameObject obj2, GameObject prefab, Transform parent)
    {
        Vector3[] points1 = new Vector3[4];
        obj1.GetComponent<RectTransform>().GetWorldCorners(points1);
        Vector3[] points2 = new Vector3[4];
        obj2.GetComponent<RectTransform>().GetWorldCorners(points2);

        Vector3[] points = new Vector3[2];
        points[0] = average(points1);
        points[1] = average(points2);
        GameObject lr = Instantiate(prefab, parent);
        lr.transform.localScale = new Vector2(110,110);
        LineRenderer lrScript = lr.GetComponent<LineRenderer>();
        lrScript.SetPositions(points);
        lrScript.widthMultiplier = BackgroundScroll.zoom.x / 10;
        return lr;
        

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
            return new Vector3(x / count, y / count, 0.9f);
        }
    }

    

    public void SavePosition(GameObject node)
    {
        GUINode guiNode = node.GetComponent<GUINode>();
        RectTransform rt = node.GetComponent<RectTransform>();
        for (int i = 0; i < nodeGraph.nodes.Count; i++)
        {
            if (guiNode.nodeRef == nodeGraph.nodes[i])
            {
                nodeGraph.nodes[i].xPos = rt.localPosition.x;
                nodeGraph.nodes[i].yPos = rt.localPosition.y;
                nodeGraph.nodes[i].xScale = rt.sizeDelta.x;
                nodeGraph.nodes[i].yScale = rt.sizeDelta.y;
            }
        }
    }
}

[Serializable]
public class EditorNameLink
{
    public GameObject editor;
    public string name;
}
