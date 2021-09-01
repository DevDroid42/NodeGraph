using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerScript : MonoBehaviour
{
    EvaluableColorVec colorRef;
    public void Setup(EvaluableColorVec colorRef)
    {
        this.colorRef = colorRef;
    }

    public void SetColor(Color color)
    {
        //A color assinment happens the second the color editor is opened before it has a chance to call setup
        if (colorRef != null)
        {
            ColorVec newColorVec = new ColorVec(color.r, color.g, color.b, color.a);
            colorRef.SetColorVec(newColorVec);
        }
    }

    public void Destroy()
    {
        GUIGraph.updateGraphGUI.Invoke();
        Destroy(gameObject);
    }
}
