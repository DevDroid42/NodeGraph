using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerScript : MonoBehaviour
{
    ColorVec colorRef;
    public void Setup(ColorVec colorRef)
    {
        this.colorRef = colorRef;
    }

    public void SetColor(Color color)
    {
        //A color assinment happens the second the color editor is opened before it has a chance to call setup
        if (colorRef != null)
        {
            colorRef.aw = color.a;
            colorRef.rx = color.r;
            colorRef.gy = color.g;
            colorRef.bz = color.b;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
