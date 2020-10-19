using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;
using System;

public class TxtEditor : MonoBehaviour
{
    public IntData intData;
    public StringData stringData;
    public FloatData floatData;
    public InputField inputField;
    public Text discText;
    public string disc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateDisc(string disc) 
    {
        this.disc = disc;
        discText.text = disc;
    }

    public void UpdateField(string data)
    {
        inputField.text = data;
    }

    public void UpdateGUI()
    {
        GUIGraph.updateGraphGUI.Invoke();
    }

    public void EditString(string str)
    {
        stringData.txt = str;
    }

    public void EditInt(string num)
    {
        intData.num = int.Parse(num);
    }

    public void EditFloat(string num)
    {
        floatData.num = float.Parse(num);
    }
}
