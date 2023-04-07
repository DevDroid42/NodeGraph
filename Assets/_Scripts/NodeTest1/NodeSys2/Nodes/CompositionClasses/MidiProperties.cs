using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

//We want to create a midi receive and midi instancer node, but we can't use multiple inhertiance so 
//use composition to avoid redeclaring properties
public class MidiProperties
{
    public Property lowerRange, upperRange;
    public MidiProperties(Node parentNode)
    {
        lowerRange = parentNode.CreateInputProperty("low bound", true, new EvaluableFloat(0));
        lowerRange.interactable = true;
        upperRange = parentNode.CreateInputProperty("high bound", true, new EvaluableFloat(256));
        upperRange.interactable = true;
    }
}