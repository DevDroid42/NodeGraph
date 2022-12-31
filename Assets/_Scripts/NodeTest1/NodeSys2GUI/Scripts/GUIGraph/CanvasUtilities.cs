using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class CanvasUtilities : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    //gets current cursor world position based on raycast intersection
    public static Vector2 RaycastPosWorld()
    {
        //Set the Pointer Event Position to that of the mouse position
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        //raycaster.Raycast(eventData, results);
        EventSystem.current.RaycastAll(pointerData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        //Debug.Log("Raycast Count: " + results.Count);
        if (results.Count > 0)
        {
            //Debug.Log("RaycastPos: " + results[results.Count - 1].worldPosition);
            return (Vector2)results[results.Count - 1].worldPosition;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
