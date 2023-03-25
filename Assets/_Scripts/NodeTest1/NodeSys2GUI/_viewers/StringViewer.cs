using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;
public class StringViewer : EditorBase
{
    public Property propCache;
    public Text data;

    // Update is called once per frame
    void Update()
    {
        if (propCache.GetData() is IEvaluable evaluable)
        {
            data.text = evaluable.EvaluateValue(0).ToString();
        }
        else if (propCache != null && propCache.GetData() != null)
        {
            data.text = propCache.GetData().ToString();
        }
    }

    public override void Setup(Property prop)
    {
        propCache = prop;
    }
}
