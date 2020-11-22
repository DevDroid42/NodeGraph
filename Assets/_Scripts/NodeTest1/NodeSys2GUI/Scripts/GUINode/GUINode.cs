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

    //reference to port prefab
    public GameObject basePort;
    public Transform inputPortHolder;
    public GameObject[] inputPorts;
    public Transform outputPortHolder;
    public GameObject[] outputPorts;

    public List<GameObject> NodeDataList = new List<GameObject>();
    public EditorManager editorManager;
    public RectTransform editor;
    public GameObject openButton;
    public GameObject closeButton;

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
        SetupPorts();
        SetupNodeData();
        SetupEditor();
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

    private void SetupPorts()
    {
        float minHeight = 0;

        int connectableCount = 0;
        //get the amount if connectable input properties exist. 
        for (int i = 0; i < nodeRef.inputs.Count; i++)
        {
            if (nodeRef.inputs[i].connectable)
            {
                connectableCount++;
            }
        }

        inputPorts = new GameObject[connectableCount];
        //all output ports are connectable 
        outputPorts = new GameObject[nodeRef.outputs.Count];

        createPorts(nodeRef.inputs, inputPorts, inputPortHolder, true);
        createPorts(nodeRef.outputs, outputPorts, outputPortHolder, false);
        
        void createPorts(List<Property> properties, GameObject[] gameObjects, Transform portHolder, bool isInput)
        {
            float position = 0;
            int index = 0;
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].connectable)
                {
                    properties[i].interactable = !properties[i].IsConnected();
                    GameObject port = Instantiate(basePort, portHolder);
                    RectTransform rt = port.GetComponent<RectTransform>();
                    position = -index * (rt.rect.height - 2) - 15;
                    //rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, position, rt.rect.height);
                    rt.anchoredPosition = new Vector2(0.5f, position);
                    gameObjects[index] = port;
                    GUIPort guiPort = gameObjects[index].GetComponentInChildren<GUIPort>();
                    guiPort.portRef = properties[i].dataPort;
                    guiPort.GUIGraphRef = GUIGraphRef;
                    guiPort.GUINodeRef = this;
                    guiPort.inputPort = isInput;
                    index++;
                }
            }
                        
            //add some padding
            position -= 45f;
            position = 0 - position;
            if(position > minHeight)
            {
                //Debug.Log("position: " + position);
                minHeight = position;
            }
            
        }
        minSize =  new Vector2(20, minHeight);
    }

    private void SetupEditor()
    {
        if (nodeRef.expanded)
        {
            SetEditorOpen();
        }
        else
        {
            SetEditorClosed();
        }
    }

    public void SetExpanded(bool state)
    {
        nodeRef.expanded = state;
    }

    private void SetEditorOpen()
    {
        openButton.SetActive(false);
        closeButton.SetActive(true);
        editor.anchorMax = new Vector2(1f, 1f);
        editor.anchorMin = new Vector2(0f, 0f);
    }

    private void SetEditorClosed()
    {
        openButton.SetActive(true);
        closeButton.SetActive(false);
        editor.anchorMax = new Vector2(1f, 1f);
        editor.anchorMin = new Vector2(0f, 1f);
    }

    private void SetupNodeData()
    {
        editorManager.SetupEditors(nodeRef.inputs);
        /** //To be moved to new class
                float offset = 0;
        if (nodeRef.constants != null)
        {
            Debug.Log(nodeTitle.text + " is setting up. Has " + nodeRef.constants.Length + "constants");
            for (int i = 0; i < nodeRef.constants.Length; i++)
            {
                GameObject editorPrefab;
                switch (nodeRef.constants[i])
                {
                    case IntData intData:
                        //get the prefab from the dictionary
                        GUIGraph.editors.TryGetValue("int", out editorPrefab);
                        //instantiate the prefab
                        GameObject intEditorGameObject = Instantiate(editorPrefab, editorContent);
                        //get the rect trransform for positioning
                        RectTransform rt = intEditorGameObject.GetComponent<RectTransform>();
                        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, offset, rt.rect.height);
                        //advance the offset for the next component
                        offset += rt.rect.height + 5;
                        //set the references in the editors script
                        TxtEditor intEdit = intEditorGameObject.GetComponent<TxtEditor>();
                        intEdit.intData = intData;
                        intEdit.UpdateDisc(nodeRef.constantsDisc[i]);
                        intEdit.UpdateField(intData.ToString());
                        //add it to the list of all editors and viewers
                        NodeDataList.Add(intEditorGameObject);

                        break;
                    default:
                        Debug.Log("constants array in node " + nodeRef.GetName() + " contains an unsupported data type at index:" + i);
                        break;
                }
            }
        }
        if (nodeRef.viewableData != null)
        {
            for (int i = 0; i < nodeRef.viewableData.Length; i++)
            {
                GameObject viewerPrefab;
                switch (nodeRef.viewableData[i])
                {
                    default:
                        Debug.Log("status array in node " + nodeRef.GetName() + " contains an unsupported data type at index:" + i
                            + "\n defaulting to string");
                        GUIGraph.viewers.TryGetValue("string", out viewerPrefab);
                        GameObject stringViewerObject = Instantiate(viewerPrefab, editorContent);
                        RectTransform rt = stringViewerObject.GetComponent<RectTransform>();
                        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, offset, rt.rect.height);
                        offset += rt.rect.height + 5;
                        StringViewer sv = stringViewerObject.GetComponent<StringViewer>();
                        sv.disc.text = nodeRef.viewableDisc[i];
                        sv.dataObj = nodeRef.viewableData[i];
                        break;
                }
            }
            RectTransform rt1 = editorContent.GetComponent<RectTransform>();
            rt1.sizeDelta = new Vector2(rt1.sizeDelta.x, offset);
        }


        **/
    }
}
