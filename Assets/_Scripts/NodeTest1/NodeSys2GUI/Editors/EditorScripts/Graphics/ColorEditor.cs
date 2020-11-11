using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorEditor : EditorBase
{
    public Button button;
    public Text disc;
    //Color button will display color and interactability will depend on the buttons interactable flag. 
    //when button is clicked will open up color editor menu. 
    private ColorVec colorVecRef;
    private void Start()
    {
    }

    public void Update()
    {
        button.image.color = new Color(colorVecRef.rx, colorVecRef.gy, colorVecRef.bz, colorVecRef.aw);
    }

    public void SetColor(Color color)
    {
        colorVecRef.aw = color.a;
        colorVecRef.rx = color.r;
        colorVecRef.gy = color.g;
        colorVecRef.bz = color.b;
    }

    public override void Setup(Property prop)
    {
        base.Setup(prop);
        disc.text = prop.disc;
        switch (prop.GetData())
        {
            case EvaluableColorVec colorVec:
                {
                    colorVecRef = colorVec.GetColorVec();
                    break;
                }

            default:
                break;
        }
    }
}
