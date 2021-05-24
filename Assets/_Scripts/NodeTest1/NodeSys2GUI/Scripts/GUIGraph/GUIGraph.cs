using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using nodeSys2;
using UnityEditor;
using System;
using UnityEngine.Events;

public class GUIGraph : MonoBehaviour
{
    Graph graphRef;
    GraphCopyPaste graphClipboard;
    public Camera cam;
    public Transform NodeParent;
    //reference to node prefab
    public GameObject baseNode;
    public GameObject baseLineRenderer;
    public float LineZOffset;
    public Transform lineRendererParent;
    public List<GameObject> guiNodes = new List<GameObject>();
    public static UnityEvent updateGraphGUI;
    [Header("Colors")]
    public Color DefaultColor;
    public Color SelectedColor;

    private UndoRedo undoRedo;

    [Serializable]
    public class StringEvent : UnityEvent<string>
    {
    }
    public StringEvent GraphChanged;

    private void Awake()
    {
        if (updateGraphGUI == null)
        {
            updateGraphGUI = new UnityEvent();
            updateGraphGUI.AddListener(UpdateGUI);
        }
        graphClipboard = new GraphCopyPaste();
        graphRef = new Graph();
        undoRedo = GetComponent<UndoRedo>();
        UpdateGUI();
    }

    // Start is called before the first frame update
    void Start()
    {


        //UpdateGUI();
        //graph = new Graph();
        //ActionPreformed();
    }

    public void CreateNewGraph()
    {
        graphRef = new Graph();
        UpdateGUI();
        undoRedo.ClearHistory();
        ActionPreformed();
    }

    public void SetGraph(string _graphJSON)
    {
        graphRef = GraphSerialization.JsonToGraph(_graphJSON);
        UpdateGUI();
        undoRedo.ClearHistory();
        ActionPreformed();
    }

    public void SetGraph(Graph graph)
    {
        graphRef = graph;
        undoRedo.ClearHistory();
        ActionPreformed();
    }

    public void ActionPreformed()
    {
        GraphChanged.Invoke(GraphSerialization.GraphToJson(graphRef));
    }

    public void PrintJson()
    {
        Debug.Log(GraphSerialization.GraphToJson(graphRef));
    }

    public string GetGraphJson()
    {
        return GraphSerialization.GraphToJson(graphRef);
    }

    public void AddNode(NodeRegistration.NodeTypes nodeType)
    {
        graphRef.nodes.Add(NodeRegistration.GetNode(nodeType));
        ActionPreformed();
        UpdateGUI();
    }

    public void UpdateGUI()
    {
        VerifyNodes();
        graphRef.InitGraph();
        for (int i = 0; i < guiNodes.Count; i++)
        {
            Destroy(guiNodes[i]);
        }
        guiNodes.Clear();


        for (int i = 0; i < graphRef.nodes.Count; i++)
        {
            GameObject node = Instantiate(baseNode, NodeParent);
            node.SetActive(true);
            node.GetComponent<GUINode>().SetupNode(graphRef.nodes[i], this);
            Draggable draggable = node.GetComponent<Draggable>();
            draggable.defaultColor = DefaultColor;
            draggable.selectedColor = SelectedColor;
            guiNodes.Add(node);
        }
        MakeConnections();
    }

    public List<GameObject> lines = new List<GameObject>();
    public void MakeConnections()
    {
        VerifyNodes();
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
        for (int i = 0; i < graphRef.nodes.Count; i++)
        {
            //go through all of each nodes input ports
            for (int j = 0; j < graphRef.nodes[i].inputs.Count; j++)
            {
                //check if the port has a connection
                if (graphRef.nodes[i].inputs[j].dataPort.IsConnected())
                {
                    //if it does store the reference to the port and it's connection
                    inPort = graphRef.nodes[i].inputs[j].dataPort;
                    outPort = graphRef.nodes[i].inputs[j].dataPort.connectedPort;
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
        lr.transform.localScale = new Vector2(110, 110);
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

    //verifies that there are no nulls in the node lists. These happen from deletes
    private void VerifyNodes()
    {

        graphRef.nodes.RemoveAll(_node => _node.MarkedForDeletion);

        for (int i = 0; i < graphRef.nodes.Count; i++)
        {
            for (int j = 0; j < graphRef.nodes[i].inputs.Count; j++)
            {
                if (!PortExistsInNodeGraph(graphRef.nodes[i].inputs[j].dataPort.connectedPort))
                {
                    graphRef.nodes[i].inputs[j].dataPort.Disconnect();
                }
            }
        }
        //when deleting a node other nodes input ports that were connected to the deleted output ports will still
        //be in a connected state. Because objects are reference types these ports will still exist in memory referenced from the input port
        //Check for a reference to the same port in the nodeSys node list. If one exists then the port hasn't been deleted
        bool PortExistsInNodeGraph(Port port)
        {
            for (int i = 0; i < graphRef.nodes.Count; i++)
            {
                for (int j = 0; j < graphRef.nodes[i].outputs.Count; j++)
                {
                    if (graphRef.nodes[i].outputs[j].dataPort == port)
                        return true;
                }
            }
            return false;
        }
    }

    public void SaveTransform(GameObject node)
    {
        GUINode guiNode = node.GetComponent<GUINode>();
        RectTransform rt = node.GetComponent<RectTransform>();
        for (int i = 0; i < graphRef.nodes.Count; i++)
        {
            if (guiNode.nodeRef == graphRef.nodes[i])
            {
                graphRef.nodes[i].xPos = rt.localPosition.x;
                graphRef.nodes[i].yPos = rt.localPosition.y;
                graphRef.nodes[i].xScale = rt.sizeDelta.x;
                graphRef.nodes[i].yScale = rt.sizeDelta.y;
            }
        }
    }

    private void CullNodes()
    {
        int marginX = cam.pixelWidth / 3;
        int marginY = cam.pixelHeight / 3;
        for (int i = 0; i < guiNodes.Count; i++)
        {
            Vector3[] corners = new Vector3[4];
            guiNodes[i].GetComponent<RectTransform>().GetWorldCorners(corners);
            bool inView = false;
            for (int j = 0; j < corners.Length; j++)
            {
                Vector2 screenPos = cam.WorldToScreenPoint(corners[j]);
                if (screenPos.x > 0 - marginX && screenPos.y > 0 - marginY && screenPos.x < cam.pixelWidth + marginX && screenPos.y < cam.pixelHeight + marginY)
                {
                    inView = true;
                    break;
                }
            }
            guiNodes[i].SetActive(inView);
        }
    }


    private void Update()
    {
        //float time = Time.realtimeSinceStartup;
        graphRef.UpdateGraph(Time.deltaTime);
        //Debug.Log("Compute time: " + (Time.realtimeSinceStartup - time));
        CullNodes();
    }
    private void Copy()
    {
        graphClipboard.Copy(graphRef);
    }

    private void Cut()
    {
        graphClipboard.Cut(graphRef);
        UpdateGUI();
    }

    private void Paste()
    {
        graphClipboard.Paste(graphRef);
        UpdateGUI();
    }

    private void OnApplicationQuit()
    {
        graphRef.StopGraph();
    }
    private void OnEnable()
    {
        GlobalInputDelagates.Copy += Copy;
        GlobalInputDelagates.Paste += Paste;
        GlobalInputDelagates.Cut += Cut;
    }

    private void OnDisable()
    {
        GlobalInputDelagates.Copy -= Copy;
        GlobalInputDelagates.Paste -= Paste;
        GlobalInputDelagates.Cut -= Cut;
    }
}

[Serializable]
public class EditorNameLink
{
    public GameObject editor;
    public string name;
}
