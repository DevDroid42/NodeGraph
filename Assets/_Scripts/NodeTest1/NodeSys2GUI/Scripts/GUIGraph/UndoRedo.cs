using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UndoRedo : MonoBehaviour
{
    [Serializable]
    public class StringEvent : UnityEvent<string>
    {
    }
    public StringEvent stateChanged;
    public List<string> history;
    public int maxHistory = 20;
    private int currentPos = 0;
    // Start is called before the first frame update
    void Awake()
    {
        history = new List<string>();
    }

    public void AddEntry(string entry)
    {
        for (int i = currentPos; i < history.Count; i++)
        {
            history.RemoveAt(i);
        }

        history.Add(entry);        
        if(history.Count > maxHistory)
        {
            history.RemoveAt(0);
        }
        currentPos = history.Count;
    }

    private void OnEnable()
    {
        GlobalInputDelagates.Undo += Undo;
        GlobalInputDelagates.Redo += Redo;
    }


    private void OnDisable()
    {
        GlobalInputDelagates.Undo += Undo;
        GlobalInputDelagates.Redo -= Redo;
    }

    private void Redo()
    {
        if(currentPos < history.Count - 1)
        {
            currentPos++;
            stateChanged.Invoke(history[currentPos]);
        }
    }

    private void Undo()
    {
        if(currentPos > 0)
        {
            currentPos--;
            stateChanged.Invoke(history[currentPos]);
        }
    }
}