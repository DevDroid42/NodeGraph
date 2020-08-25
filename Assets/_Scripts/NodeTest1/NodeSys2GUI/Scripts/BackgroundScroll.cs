using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BackgroundScroll : MonoBehaviour, IPointerClickHandler
{
    public float scrollSensitivity;
    public float panSensitivity;
    public float minZoom;
    public float maxZoom;
    public static Vector2 zoom;

    private void Awake()
    {
        Zoom(0);
    }

    private void OnEnable()
    {
        GlobalInputDelagates.scroll += Zoom_performed;
        GlobalInputDelagates.pan += PanPreformed;
    }

    private void OnDisable()
    {
        GlobalInputDelagates.scroll -= Zoom_performed;
        GlobalInputDelagates.pan -= PanPreformed;
    }

    private void PanStarted()
    {

    }

    private void PanPreformed()
    {
        GUIGraph.updateGraphGUI.Invoke();
    }

    private void Zoom_performed(float deltaScroll)
    {
        if (Application.isFocused)
        {
            Zoom(deltaScroll);
            //Debug.Log("zoom: " + deltaZoom);
            GUIGraph.updateGraphGUI.Invoke();
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

}
