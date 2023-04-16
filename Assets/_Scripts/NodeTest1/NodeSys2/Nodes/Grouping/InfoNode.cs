using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System.Linq;
using Newtonsoft.Json;

//alternative to groupInput node that has hardcoded output values
public abstract class InfoNode : Node
{
    //This will link input properties to a string
    //Deriving info nodes will contain non-visible input properties that will take the data in and pass it on to the outputs
    //this has the benifits of using the standard data transmission code instead of entagling states
    [JsonProperty] private Dictionary<string, Property> propertyMap = new Dictionary<string, Property>();

    public InfoNode(ColorVec pos) : base(pos)
    {

    }

    protected void RegisterInfoInputProperty(Property prop)
    {
        if (!prop.isInput)
        {
            Debug.LogError("cannot register output property on info node");
            return;
        }
        propertyMap.Add(prop.Disc, prop);
    }

    public List<string> GetNames()
    {
        return propertyMap.Keys.ToList();
    }

    public void PublishData(string id, object data)
    {
        if (!propertyMap.ContainsKey(id)) return;
        propertyMap[id].SetData(data);
    }
}
