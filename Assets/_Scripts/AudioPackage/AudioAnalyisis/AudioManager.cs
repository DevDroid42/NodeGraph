using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioAnalyzer[] Analyzers;

    //gets an audio analayzer by index
    public AudioAnalyzer GetAnalyzer(int index)
    {
        if(index < Analyzers.Length)
        {
            return Analyzers[index];
        }
        else
        {
            Debug.LogError("Specified index of: " + index + " is out of bounds");
            return null;
        }
    }

    public AudioAnalyzer GetAnalyzer(string tag)
    {
        for (int i = 0; i < Analyzers.Length; i++)
        {
            if (Analyzers[i].audioTag == tag)
                return Analyzers[i];
        }
        Debug.LogError("Could not find any AudioAnalyzers with tag: " + tag);
        return null;
    }
}
