using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringViewer : EditorBase
{
    public object dataObj;
    public Text disc, data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dataObj != null)
        {
            data.text = dataObj.ToString();
        }        
    }

    public override void Setup(Property prop)
    {
        disc.text = prop.disc;
        dataObj = prop.GetData();
    }
}
