using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;

public class GUINode : MonoBehaviour
{
    public GUIGraph GUIGraphRef;
    //a reference to the node running in nodeSys
    public Node nodeRef;
    public Text nodeTitle;

    public GameObject NonConnectablePortBase;
    //reference to port prefab    
    public GameObject ConnectablePortBase;
    public Transform PropHolder;
    public GameObject[] inputPorts;    
    public GameObject[] outputPorts;
   
    public EditorManager editorManager;

    public Vector2 minSize;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rect.sizeDelta.x < minSize.x)
        {
            rect.sizeDelta = new Vector2(minSize.x, rect.sizeDelta.y);
        }
        if(rect.sizeDelta.y < minSize.y)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, minSize.y);
        }
    }

    //sets up the node GUI to match with a given node
    public void SetupNode(Node node, GUIGraph graphRef)
    {
        GUIGraphRef = graphRef;
        nodeRef = node;
        transform.localPosition = new Vector3(nodeRef.xPos, nodeRef.yPos, 0f);           
        nodeTitle.text = nodeRef.GetName();
        SetupProperties();
        SetupNodeData();
        SetupScale();
    }

    public void DeleteNode()
    {
        for (int i = 0; i < nodeRef.inputs.Count; i++)
        {
            //this will clear all delagates pointing towards this node to avoid delagates pointing to null function locations
            nodeRef.inputs[i].dataPort.Disconnect();
            nodeRef.CleanUp();
        }
        nodeRef.MarkedForDeletion = true;
        GUIGraph.updateGraphGUI.Invoke();
    }

    private void SetupScale()
    {
        Vector2 size = new Vector2(minSize.x, minSize.y);
        if(nodeRef.xScale > minSize.x)
        {
            size.x = nodeRef.xScale;
        }
        if (nodeRef.yScale > minSize.y)
        {
            size.y = nodeRef.yScale;
        }
        rect.sizeDelta = size;
    }

    private void SetupProperties()
    {
        int connectableCount = 0;
        //get the amount if connectable input properties exist. 
        for (int i = 0; i < nodeRef.inputs.Count; i++)
        {
            if (nodeRef.inputs[i].GetConnectable())
            {
                connectableCount++;
            }
        }

        inputPorts = new GameObject[connectableCount];
        //all output ports are connectable 
        outputPorts = new GameObject[nodeRef.outputs.Count];

        createPorts(nodeRef.outputs, outputPorts, PropHolder, false);
        createPorts(nodeRef.inputs, inputPorts, PropHolder, true);

        void createPorts(List<Property> properties, GameObject[] gameObjects, Transform portHolder, bool isInput)
        {            
            int index = 0;
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].GetConnectable())
                {
                    properties[i].interactable = !properties[i].IsConnected();
                    GameObject port = Instantiate(ConnectablePortBase, portHolder);                    
                    
                    gameObjects[index] = port;
                    GUIPort guiPort = gameObjects[index].GetComponentInChildren<GUIPort>();
                    guiPort.portRef = properties[i].dataPort;
                    guiPort.GUIGraphRef = GUIGraphRef;
                    guiPort.GUINodeRef = this;
                    guiPort.inputPort = isInput;
                    //if it is an input setup the editor. However, outputs don't have editors so don't otherwise
                    if (properties[i].isInput)
                    {
                        editorManager.SetupEditor(properties[i], port.transform.GetChild(0));
                    }
                    else
                    {
                        //this enables the text because that's all outputs have
                        port.transform.GetChild(1).gameObject.SetActive(true);
                        //This destroys the editor portion of property because it is an output
                        Destroy(port.transform.GetChild(0).gameObject);                        
                    }
                    index++;
                }
                else
                {
                    GameObject holder = Instantiate(NonConnectablePortBase, portHolder);
                    editorManager.SetupEditor(properties[i], holder.transform.GetChild(0));
                }
            }
            
        }        
    }

    private void SetupNodeData()
    {
        //editorManager.SetupEditor(nodeRef.inputs);
    }
}
