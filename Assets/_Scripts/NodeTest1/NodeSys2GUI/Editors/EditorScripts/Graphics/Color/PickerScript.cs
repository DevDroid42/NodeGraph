using System.Collections;
using System.Collections.Generic;
using nodeSys2;
using UnityEngine;

public class PickerScript : MonoBehaviour
{
    Property prop;
    public void Setup(Property prop)
    {
        this.prop = prop;
    }

    public void SetColor(Color color)
    {
        //A color assinment happens the second the color editor is opened before it has a chance to call setup
        if (prop != null)
        {
            ColorVec newColorVec = new ColorVec(color.r, color.g, color.b, color.a);
            prop.SetData(newColorVec);
        }
    }

    public void Destroy()
    {
        GUIGraph.updateGraphGUI.Invoke();
        Destroy(gameObject);
    }
}
