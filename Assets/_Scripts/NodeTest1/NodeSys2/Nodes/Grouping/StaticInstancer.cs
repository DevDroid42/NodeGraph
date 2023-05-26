using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System;
using Newtonsoft.Json;

//similar functionality to group node but handles instances of graphs. Each instance will have it's outputs of same name mixed via 
//color mixing
public class StaticInstancer : GroupNodeBase
{

    private string graphJson;
    protected List<Group> groups = new List<Group>();
    //serves as a parrallel array to the groups list that stores evaluable data to be used in constructing new mixers
    private IEvaluable[] groupOutputData;
    private EvaluableMixRGB mixer;
    [JsonProperty] protected List<Property> groupInputs;
    [JsonProperty] private Property InstanceCount, mixType, factor, output;
    private GroupOutputNode.GroupOutDelegate groupOutDelegate;

    public StaticInstancer(ColorVec pos) : base(pos)
    {
        graph = new Graph();
        groupInputs = new List<Property>();

        nodeDisc = "Static Instancer";
        InstanceCount = CreateInputProperty("Instance Count", false, new EvaluableFloat(1));
        InstanceCount.interactable = true;
        mixType = CreateInputProperty("mixType", false, new EvaluableMixRGB.MixType());
        mixType.interactable = true;
        factor = CreateInputProperty("factor", true, new EvaluableFloat(1));
        factor.interactable = true;

        output = CreateOutputProperty("output");
    }

    public override void Init()
    {
        base.Init();
        ProccessEnums();
        groupOutDelegate = new GroupOutputNode.GroupOutDelegate(GroupOutputHandler);
        SetupInstances();
        SetupProperties();
        RegisterFrameMethod(Frame);
        InitGraphs();
    }

    public override void Init2()
    {
        InitGraphs2();
        Handle();
    }

    //if the group contains nodes that don't have respective properties add them here. 
    private void SetupProperties()
    {
        List<string> inputTags = groups[0].GetInputTags();
        TrimProperties(inputTags, groupInputs);
        Addproperties(inputTags, groupInputs, true);
    }

    private void SetupInstances()
    {
        groups.Clear();
        groups.Add(new Group(graph, groupOutDelegate));
        graphJson = GraphSerialization.GraphToJson(graph);
        //Create instances of groups. On first load of a graph new groups will need to be created
        for (int i = 1; i < (int)((EvaluableFloat)InstanceCount.GetData()).EvaluateValue(0); i++)
        {
            //if this is an init after the project is already open no need to recreate instances
            if (groups.Count - 1 < i)
            {
                Group g = new Group(GraphSerialization.JsonToGraph(graphJson), groupOutDelegate);
                groups.Add(g);
            }
        }
        groupOutputData = new IEvaluable[groups.Count];
        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].AssignInstanceInfo(i, groups.Count, i/(float)(groups.Count));
            float position = i / (float)groups.Count;
            groups[i].SetVector(position);
            groups[i].PublishInfoNodeData();
        }
    }

    private void InitGraphs()
    {
        for (int i = 0; i < groups.Count; i++)
        { 
            groups[i].Init();
        }
    }

    private void InitGraphs2()
    {
        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].Init2();
        }
    }

    private void GroupOutputHandler(object data, string ID, int index)
    {
        if (data is IEvaluable eData)
        {
            groupOutputData[index] = eData;
        }

        for (int i = 0; i < groupOutputData.Length; i++)
        {
            if (groupOutputData[i] == null)
            {
                return;
            }
        }

        if (mixer == null || mixer.elements.Count != groupOutputData.Length)
        {
            mixer = new EvaluableMixRGB((IEvaluable)factor.GetData());
            foreach (IEvaluable evaluable in groupOutputData)
            {
                mixer.elements.Add(evaluable);
            }
        }
        else
        {
            mixer.factor = (IEvaluable)factor.GetData();
            //if we are in this else then we know the mixer elements length is the same as the groupOutputData
            for (int i = 0; i < mixer.elements.Count; i++)
            {
                mixer.elements[i] = groupOutputData[i];
            }
        }
        mixer.mixType = (EvaluableMixRGB.MixType)mixType.GetData();
        output.Invoke(mixer);
    }

    public override void Handle()
    {
        foreach (Property prop in groupInputs)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i] != null)
                {
                    groups[i].PublishToGraph(prop.ID, prop.GetData());
                }
            }
        }
    }

    public override void Frame(float deltaTime)
    {
        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].UpdateGraph();
        }
    }

    //run when exiting the group editing. This will clear the groups and run setupInstances
    public void GraphUpdated()
    {

    }
    private void ProccessEnums()
    {
        if (mixType.GetData().GetType() == typeof(string))
        {
            mixType.SetData(Enum.Parse(typeof(EvaluableMixRGB.MixType), (string)mixType.GetData()));
        }
    }
}
