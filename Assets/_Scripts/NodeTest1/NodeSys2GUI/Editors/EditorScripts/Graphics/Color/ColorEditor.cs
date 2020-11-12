using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorEditor : EditorBase
{
    public Button button;
    public Text disc;
    public GameObject PickerPrefab;
    public static Transform PickerHolder;
    //Color button will display color and interactability will depend on the buttons interactable flag. 
    //when button is clicked will open up color editor menu. 
    private ColorVec colorVecRef;
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
        button.image.color = new Color(colorVecRef.rx, colorVecRef.gy, colorVecRef.bz, colorVecRef.aw);
    }

    public void OpenPicker()
    {
        Instantiate(PickerPrefab, PickerHolder).GetComponent<PickerScript>().Setup(colorVecRef);
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
