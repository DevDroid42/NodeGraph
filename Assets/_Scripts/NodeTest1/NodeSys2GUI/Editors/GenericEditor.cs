using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class GenericEditor : MonoBehaviour
{
    public GameObject interactableObj;
    public GameObject nonInteractableObj;
    public void SetupEditor(Property prop)
    {
        interactableObj.GetComponent<EditorBase>().Setup(prop);
        nonInteractableObj.GetComponent<EditorBase>().Setup(prop);
        //ToDo set transform height here
        if (prop.interactable)
        {
            interactableObj.SetActive(true);
            nonInteractableObj.SetActive(false);
        }
        else
        {
            interactableObj.SetActive(false);
            nonInteractableObj.SetActive(true);
        }
    }
}
