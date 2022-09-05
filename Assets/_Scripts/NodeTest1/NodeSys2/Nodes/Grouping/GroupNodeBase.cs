using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

//serves as the base class for nodes that contain nodegraphs
public class GroupNodeBase : Node
{
    public Graph graph;

    public GroupNodeBase(ColorVec pos) : base(pos) { }

    protected void Addproperties(List<string> tags, List<Property> propertyList, bool input)
    {
        //iterate through every tag
        foreach (string tag in tags)
        {
            //if a tag doesn't exist in the list of properties create a new property with the tag
            if (!propertyList.Exists(e => e.ID == tag))
            {
                if (input)
                {
                    propertyList.Add(CreateInputProperty(tag, true, new Evaluable(), typeof(ICopyable)));
                }
                else
                {
                    propertyList.Add(CreateOutputProperty(tag));
                }
            }
        }
    }

    protected void TrimProperties(List<string> tags, List<Property> propertyList)
    {
        for (int i = propertyList.Count - 1; i >= 0; i--)
        {
            //if a property exists that doesn't have a tag remove the property
            if (!tags.Exists(e => e == propertyList[i].ID))
            {
                RemoveProperty(propertyList[i]);
                propertyList.RemoveAt(i);
            }
        }
    }
}
