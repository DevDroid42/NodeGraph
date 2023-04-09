using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System;
using Newtonsoft.Json;

//We want to create a midi receive and midi instancer node, but we can't use multiple inhertiance so 
//use composition to avoid redeclaring properties
public class MidiProperties
{
    public Property lowerBound, upperBound;
    public MidiProperties(Node parentNode)
    {
        lowerBound = parentNode.CreateInputProperty("low bound", true, new EvaluableFloat(0));
        lowerBound.interactable = true;
        upperBound = parentNode.CreateInputProperty("high bound", true, new EvaluableFloat(256));
        upperBound.interactable = true;
    }

    [JsonConstructor] public MidiProperties() { }

    private byte[] trackedValues;
    private List<int> justPressed = new List<int>();
    [JsonIgnore]public Action<List<int>> notePressedCallback { get; set;}

    public void UpdateState(byte[] notes)
    {
        TrackJustPressed(notes);
    }

    private void TrackJustPressed(byte[] incoming)
    {
        if (trackedValues == null || trackedValues.Length != incoming.Length)
        {
            trackedValues = new byte[incoming.Length];
        }
        justPressed.Clear();
        for (int i = 0; i < incoming.Length; i++)
        {
            if (incoming[i] > trackedValues[i])
            {
                justPressed.Add(i);
            }
            trackedValues[i] = incoming[i];
        }
        if(justPressed.Count > 0)
        {
            notePressedCallback.Invoke(justPressed);
        }
    }
}