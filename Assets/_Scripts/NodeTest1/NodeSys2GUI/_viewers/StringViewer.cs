using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;
public class StringViewer : EditorBase
{
    public Property propCache;
    public Text data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(propCache != null && propCache.GetData() != null)
        {
            data.text = propCache.GetData().ToString();
        }        
    }

    public override void Setup(Property prop)
    {
        propCache = prop;
    }
}
