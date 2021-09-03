﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class GroupNode : GroupNodeBase
{    
    private Group group;
    public List<Property> groupInputs;
    public List<Property> groupOutputs;
    private GroupOutputNode.GroupOutDelegate groupOutDelegate;

    public GroupNode(bool x)
    {
        graph = new Graph();        
        base.nodeDisc = "Group Node";        
        groupInputs = new List<Property>();
        groupOutputs = new List<Property>();
    }

    public override void Init()
    {
        frameDelagate -= Frame;
        frameDelagate += Frame;

        groupOutDelegate = new GroupOutputNode.GroupOutDelegate(GroupOutHandler);
        group = new Group(graph, groupOutDelegate);
        SetupProperties();
    }

    private void GroupOutHandler(object data, string ID, int index)
    {
        foreach (Property output in groupOutputs)
        {
            if(output.ID == ID)
            {
                output.Invoke(data);
            }
        }
    }

    public override void Handle()
    {
        foreach (Property prop in groupInputs)
        {
            if (group != null)
            {
                group.PublishToGraph(prop.ID, prop.GetData());
            }
        }
    }

    //if the group contains nodes that don't have respective properties add them here. 
    private void SetupProperties()
    {
        List<string> inputTags = group.GetInputTags();
        List<string> outputTags = group.GetOutputTags();
        TrimProperties(inputTags, groupInputs);
        TrimProperties(outputTags, groupOutputs);
        Addproperties(inputTags, groupInputs, true);
        Addproperties(outputTags, groupOutputs, false);
    }

    public override void Frame(float deltaTime)
    {
        graph.UpdateGraph();
    }

}

