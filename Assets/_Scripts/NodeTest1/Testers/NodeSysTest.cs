using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;
using System;

public class NodeSysTest : MonoBehaviour
{
    public Text viewer;

    IntConstant numNode0, numNode1;
    AddNode addNode;
    ViewerNode receiveNode;
    // Start is called before the first frame update
    void Start()
    {
        numNode0 = new IntConstant();
        numNode1 = new IntConstant();
        addNode = new AddNode();
        receiveNode = new ViewerNode();
        addNode.inputs[0].Connect(numNode0.outputs[0]);
        addNode.inputs[1].Connect(numNode1.outputs[0]);
        receiveNode.inputs[0].Connect(addNode.outputs[0]);
    }

    public void NumNode0(string data)
    {
        //numNode0.InvokeNum(int.Parse(data));
    }

    public void NumNode1(string data)
    {
        //numNode1.InvokeNum(int.Parse(data));
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (receiveNode.newData)
        {
            Debug.Log(receiveNode.data);
            viewer.text = receiveNode.data.ToString();
            receiveNode.newData = false;
        }
        */
    }
}
