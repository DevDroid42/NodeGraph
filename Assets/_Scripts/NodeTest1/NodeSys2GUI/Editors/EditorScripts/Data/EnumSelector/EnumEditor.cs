using nodeSys2;
using System;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnumEditor : EditorBase
{
    public Dropdown dropDown;
    Property propCache;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private List<string> enumStrings;
    Type _enumType;
    public override void Setup(Property prop)
    {
        propCache = prop;        
        if (prop.GetData().GetType().IsEnum)
        {
            _enumType = prop.GetData().GetType();
            enumStrings = Enum.GetNames(_enumType).ToList();

            dropDown.ClearOptions();
            dropDown.AddOptions(enumStrings);
            dropDown.onValueChanged.AddListener(OnSelect);
            dropDown.value = (int)prop.GetData();
        }
        else
        {
            Debug.LogWarning("A data type of: (" + prop.GetData().GetType().Name + ") was sent to an enum editor. Cannon procces");
        }
    }

    //used for the default dropdown
    public void OnSelect(int selection)
    {
        if (propCache.GetData().GetType().IsEnum) {
            propCache.SetData(Enum.Parse(propCache.GetData().GetType(), enumStrings[selection]));
            //GUIGraph.updateGraphGUI.Invoke();
            //Debug.Log("TEST datatype" + propCache.GetData().GetType().Name + "\t data: " + propCache.GetData());            
        }
        else
        {
            Debug.LogWarning("A property has an ivalid data type of "+ propCache.GetType() +" it's data type " +
                "was likely changes during runtime at some point as the setup method passed");
        }
    }

}
