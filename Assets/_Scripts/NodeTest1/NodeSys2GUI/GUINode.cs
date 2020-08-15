using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;

public class GUINode : MonoBehaviour
{
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
    public Transform editorContent;

    Vector2 minSize;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //sets up the node GUI to match with a given node
    public void SetupNode(Node node)
    {
        nodeRef = node;
        transform.localPosition = new Vector2(nodeRef.xPos, nodeRef.yPos);
        nodeTitle.text = nodeRef.GetName();
        SetupPorts();
        SetupNodeData();
    }

    private void SetupPorts()
    {
        inputPorts = new GameObject[nodeRef.inputs.Length];
        outputPorts = new GameObject[nodeRef.outputs.Length];

        createPorts(nodeRef.inputs, inputPorts, inputPortHolder, true);
        createPorts(nodeRef.outputs, outputPorts, outputPortHolder, false);

        void createPorts(Port[] ports, GameObject[] gameObjects, Transform portHolder, bool isInput)
        {
            for (int i = 0; i < ports.Length; i++)
            {
                GameObject port = Instantiate(basePort, portHolder);
                RectTransform rt = port.GetComponent<RectTransform>();
                float size = i * rt.rect.height + 10;
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, size, rt.rect.height);
                gameObjects[i] = port;
                gameObjects[i].GetComponent<GUIPort>().portRef = ports[i];
                gameObjects[i].GetComponent<GUIPort>().inputPort = isInput;
            }
        }

    }

    private void SetupNodeData()
    {
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


    }
}
