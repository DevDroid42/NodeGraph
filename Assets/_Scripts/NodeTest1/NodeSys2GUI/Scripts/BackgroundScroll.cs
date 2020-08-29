using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BackgroundScroll : MonoBehaviour, IPointerClickHandler
{
    public EventSystem eventSystem;
    public PointerEventData eventData;
    public GraphicRaycaster raycaster;


    public float scrollSensitivity;
    public float panSensitivity;
    public float minZoom;
    public float maxZoom;
    public static Vector2 zoom;

    public UnityEvent OnTranslate;

    private void Awake()
    {        
        Zoom(0);
    }

    private void OnEnable()
    {
        GlobalInputDelagates.scroll += Zoom_performed;
        GlobalInputDelagates.pan += PanPreformed;
        GlobalInputDelagates.panStart += PanStarted;
    }

    private void OnDisable()
    {
        GlobalInputDelagates.scroll -= Zoom_performed;
        GlobalInputDelagates.pan -= PanPreformed;
        GlobalInputDelagates.panStart -= PanStarted;
    }

    private Vector2 initialCursorPos;
    private Vector3 initialObjectPos;
    private void PanStarted()
    {        
        initialObjectPos = transform.position;
        initialCursorPos = RaycastPos();
    }

    private void PanPreformed()
    {
        transform.position = initialObjectPos + (Vector3)(RaycastPos() - initialCursorPos) * panSensitivity;
        OnTranslate.Invoke();
    }

    private void Zoom_performed(float deltaScroll)
    {
        if (Application.isFocused)
        {
            Zoom(deltaScroll);
            //Debug.Log("zoom: " + deltaZoom);            
        }
    }


    void Zoom(float deltaZoom)
    {
        zoom = new Vector2(
    transform.localScale.x + deltaZoom * Time.deltaTime * scrollSensitivity * zoom.x
    , transform.localScale.y + deltaZoom * Time.deltaTime * scrollSensitivity * zoom.x);
        if (zoom.x > maxZoom)
        {
            zoom.x = maxZoom;
            zoom.y = maxZoom;
        }
        else if (zoom.x < minZoom)
        {
            zoom.x = minZoom;
            zoom.y = minZoom;
        }
        transform.localScale = zoom;
        OnTranslate.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Draggable.DeselectAll();            
        }
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
        
        if(results.Count > 0)
        {
            return results[results.Count - 1].worldPosition;
        }
        else
        {
            return Vector2.zero;
        }
    }

}
