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
            baseNode.GetComponent<GUINode>().SetupNode(nodeGraph.nodes[i]);
            guiNodes.Add(node);
        }
    }
}

[Serializable]
public class EditorNameLink
{
    public GameObject editor;
    public string name;
}
