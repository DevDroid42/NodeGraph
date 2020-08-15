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

    public List<GameObject> editors = new List<GameObject>();
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
        nodeTitle.text = nodeRef.GetName();
        SetupPorts();
        SetupNodeData();
    }

    private void SetupPorts()
    {
        inputPorts = new GameObject[nodeRef.inputs.Length];
        outputPorts = new GameObject[nodeRef.outputs.Length];

        createPorts(nodeRef.inputs, inputPorts, inputPortHolder);
        createPorts(nodeRef.outputs, outputPorts, outputPortHolder);

        void createPorts(Port[] ports, GameObject[] gameObjects, Transform portHolder)
        {
            for (int i = 0; i < ports.Length; i++)
            {
                GameObject port = Instantiate(basePort, portHolder);
                RectTransform rt = port.GetComponent<RectTransform>();
                float size = i * rt.rect.height + 10;
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, size, rt.rect.height);
                gameObjects[i] = port;
                gameObjects[i].GetComponent<GUIPort>().portRef = ports[i];
            }
        }
        
    }

    private void SetupNodeData()
    {
        if (nodeRef.constants != null)
        {
            float offset = 0;
            for (int i = 0; i < nodeRef.constants.Length; i++)
            {
                GameObject editor;
                switch (nodeRef.constants[i])
                {
                    case IntData intData:
                        GUIGraph.editors.TryGetValue("int", out editor);
                        GameObject intEditor = Instantiate(editor, editorContent);
                        RectTransform rt = intEditor.GetComponent<RectTransform>();                        
                        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, offset, rt.rect.height);
                        offset += rt.rect.height + 5;
                        intEditor.GetComponent<IntEditor>().intData = intData;                        
                        editors.Add(intEditor);
                        break;
                    default:
                        Debug.Log("constants array in node " + nodeRef.GetName() + " contains an unsupported data type at index:" + i);
                        break;
                }
            }
            RectTransform rt1 = editorContent.GetComponent<RectTransform>();
            rt1.sizeDelta = new Vector2(rt1.sizeDelta.x, rt1.sizeDelta.y);
        }
    }
}
