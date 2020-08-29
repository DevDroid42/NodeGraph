using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeCreator : MonoBehaviour
{
    public EventSystem eventSystem;
    public PointerEventData eventData;
    public GraphicRaycaster raycaster;

    public GUIGraph graph;
    public EnumSelector enumSelector;
    NodeRegistration.NodeTypes nodeType = 0;

    // Start is called before the first frame update
    void Start()
    {
        enumSelector.SetUpEnum(typeof(NodeRegistration.NodeTypes), nodeType);
        enumSelector.selectionMade.AddListener(AddNode);
    }

    private void OnEnable()
    {
        GlobalInputDelagates.openMenu += OpenNodeMenu;
    }

    private void OnDisable()
    {
        GlobalInputDelagates.openMenu -= OpenNodeMenu;
    }

    
    void OpenNodeMenu()
    {
        enumSelector.transform.position = RaycastPos();
        //enumSelector.SetUpEnum()
        enumSelector.OpenMenu();        
    }

    void AddNode(int type)
    {
        Debug.Log((NodeRegistration.NodeTypes)type);
        graph.AddNode((NodeRegistration.NodeTypes)type);
    }

    //gets current cursor world position based on raycast intersection
    private Vector2 RaycastPos()
    {
        //Set up the new Pointer Event
        eventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        eventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        raycaster.Raycast(eventData, results);
        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray

        if (results.Count > 0)
        {
            return results[results.Count - 1].worldPosition;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
