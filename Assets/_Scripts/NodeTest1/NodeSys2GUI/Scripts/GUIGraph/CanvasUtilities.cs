using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class CanvasUtilities : MonoBehaviour
{
    public EventSystem eventSystem;
    private static EventSystem eventSystemStatic;
    private static GraphicRaycaster raycaster;

    // Start is called before the first frame update
    void Start()
    {
        eventSystemStatic = eventSystem;
        raycaster = GetComponent<GraphicRaycaster>();
    }

    //gets current cursor world position based on raycast intersection
    public static Vector2 RaycastPosWorld()
    {
        PointerEventData eventData;
        //Set up the new Pointer Event
        eventData = new PointerEventData(eventSystemStatic);
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
