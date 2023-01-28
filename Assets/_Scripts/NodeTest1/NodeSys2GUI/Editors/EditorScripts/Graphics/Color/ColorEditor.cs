using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using UnityEngine.UI;

public class ColorEditor : EditorBase
{
    public Button button;
    public GameObject PickerPrefab;
    public static Transform PickerHolder;
    //Color button will display color and interactability will depend on the buttons interactable flag. 
    //when button is clicked will open up color editor menu. 
    private Property prop;
    private void Start()
    {
        if (PickerHolder == null)
        {
            PickerHolder = GameObject.Find("PopupHolder").transform;
        }
        if(PickerHolder == null)
        {
            Debug.LogWarning("Could not find PopupHolder GameObject. Make sure the scene has an object of this name for popups");
        }
    }

    public void Update()
    {
        switch (prop.GetData())
        {
            case Evaluable evaluable:
                {
                    ColorVec color = evaluable.EvaluateColor(0);
                    button.image.color = new Color(color.rx, color.gy, color.bz, color.aw);
                    break;
                }
            default:
                Debug.LogWarning("Invalid data assigned to color editor, Can not display type of:" + prop.GetData().GetType().Name);
                break;
        }       
    }

    public void OpenPicker()
    {        
        Instantiate(PickerPrefab, PickerHolder).GetComponent<PickerScript>().Setup(prop);        
    }

    public override void Setup(Property prop)
    {
        base.Setup(prop);        
        this.prop = prop;
    }
}
