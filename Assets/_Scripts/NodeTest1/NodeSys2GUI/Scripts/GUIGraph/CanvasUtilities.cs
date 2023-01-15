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

    public static List<RaycastResult> GetRaycastResults()
    {
        //Set the Pointer Event Position to that of the mouse position
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        //raycaster.Raycast(eventData, results);
        EventSystem.current.RaycastAll(pointerData, results);

        return results;
    }

    //Attempts to get a compenent from a list of raycast results. 
    //In the case where multiple components are present a random one will be selected
    public static bool TryGetRaycastComponent<T>(out T component) where T : class
    {
        component = null;

        List<RaycastResult> results = GetRaycastResults();

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent<T>(out T comp))
            {
                component = comp;
            }
        }
        //return true if the component is found
        return component != null;
    }

    //gets current cursor world position based on raycast intersection
    public static Vector2 RaycastPosWorld()
    {
        //Create a list of Raycast Results
        List<RaycastResult> results = GetRaycastResults();

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
