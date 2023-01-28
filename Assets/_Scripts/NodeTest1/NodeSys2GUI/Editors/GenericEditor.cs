using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class GenericEditor : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public GameObject interactableObj;
    public GameObject nonInteractableObj;
    public Text propertyDisc;
    private Property prop;
    //A prefab dependancy injected via editor manager
    private GameObject contextMenu;
    private Transform popupHolder;

    public void SetupEditor(Property prop, GameObject contextMenu, Transform popupHolder)
    {
        this.popupHolder = popupHolder;
        this.contextMenu = contextMenu;
        this.prop = prop;
        interactableObj.GetComponent<EditorBase>().Setup(prop);
        nonInteractableObj.GetComponent<EditorBase>().Setup(prop);
        //ToDo set transform height here
        if (prop.interactable)
        {
            nonInteractableObj.SetActive(false);
            interactableObj.SetActive(true);
        }
        else
        {
            interactableObj.SetActive(false);
            nonInteractableObj.SetActive(true);
        }
    }

    public void Update()
    {
        if (prop != null)
        {
            propertyDisc.text = prop.Disc;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!(eventData.button == PointerEventData.InputButton.Right)) return;
        if (!(prop.GetData() is Evaluable)) return;

        ContextMenu menu = Instantiate(contextMenu, popupHolder).GetComponent<ContextMenu>();
        menu.transform.position = CanvasUtilities.RaycastPosWorld();
        menu.gameObject.SetActive(true);
        Dictionary<String, Color> colors = new Dictionary<String, Color>();
        colors.Add(prop.currentEditor.ToString(), Color.cyan);
        if (prop.currentEditor != prop.internalRepresentation)
        {
            colors.Add(prop.internalRepresentation.ToString(), Color.green);
        }
        List<String> options = Enum.GetNames(typeof(EditorTypeManagement.Editor)).ToList();
        options.Remove("nonEvaluable");
        menu.SetupMenu(ChangeEditorType, options, colors);
    }

    private void ChangeEditorType(string editorType)
    {
        prop.currentEditor = (EditorTypeManagement.Editor)Enum.Parse(typeof(EditorTypeManagement.Editor), editorType);
        GUIGraph.currentInstance.UpdateGUI();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
