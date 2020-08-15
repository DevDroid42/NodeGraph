using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;
using System;

public class IntEditor : MonoBehaviour
{
    public IntData intData;
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

    public void EditInt(string num)
    {
        intData.num = int.Parse(num);
    }
}
