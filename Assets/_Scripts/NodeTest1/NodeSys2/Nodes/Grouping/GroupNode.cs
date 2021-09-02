using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class GroupNode : Node
{
    public Graph graph;
    private Group group;
    //vector used to evaluate instanced data
    private ColorVec vector;
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

    //a reference to this method is passed down into the group objects and is called when there is an output
    private void GroupOutHandler(object data, string ID)
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

    public void setVector(ColorVec vector)
    {
        this.vector = vector;
    }

    //if the group contains nodes that don't have respective properties add them here. 
    private void SetupProperties()
    {
        List<string> inputTags = group.GetInputTags();
        List<string> outputTags = group.GetOutputTags();
        RemoveFromGroupList(inputTags, groupInputs);
        RemoveFromGroupList(outputTags, groupOutputs);
        AddToGroupLists(inputTags, groupInputs, true);
        AddToGroupLists(outputTags, groupOutputs, false);
    }

    private void AddToGroupLists(List<string> tags, List<Property> propertyList, bool input)
    {        
        //iterate through every tag
        foreach (string tag in tags)
        {
            //if a tag doesn't exist in the list of properties create a new property with the tag
            if (!propertyList.Exists(e => e.ID == tag))
            {
                if (input)
                {
                    propertyList.Add(CreateInputProperty(tag, true, new Evaluable()));
                }
                else
                {
                    propertyList.Add(CreateOutputProperty(tag));
                }
            }
        }
    }

    private void RemoveFromGroupList(List<string> tags, List<Property> propertyList)
    {
        for (int i = propertyList.Count - 1; i >= 0 ; i--)
        {
            //if a property exists that doesn't have a tag remove the property
            if (!tags.Exists(e => e == propertyList[i].ID))
            {
                RemoveProperty(propertyList[i]);
                propertyList.RemoveAt(i);
            }
        }

    }

    public override void Frame(float deltaTime)
    {
        graph.UpdateGraph();
    }

}

