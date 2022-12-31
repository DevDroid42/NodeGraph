using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMovement : MonoBehaviour
{
    public float scrollSensitivity;
    public float minZoom;
    public float maxZoom;
    public static float zoom;

    public UnityEvent OnTranslate;

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

    Vector3 dragCamInitialPos;
    Vector3 dragMouseInitialPos;
    private void PanStarted()
    {
        dragCamInitialPos = transform.position;
        dragMouseInitialPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }

    private void PanPreformed()
    {
        Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 delta = mouse - dragMouseInitialPos;
        delta.x *= Camera.main.aspect * Camera.main.orthographicSize / 0.5f;
        delta.y *= Camera.main.orthographicSize / 0.5f;
        transform.position = dragCamInitialPos - delta;
    }

    private void Awake()
    {
        Zoom(0);
    }

    private void Zoom_performed(float deltaScroll)
    {
        if (Application.isFocused)
        {
            Zoom(deltaScroll);
        }
    }

    void Zoom(float deltaZoom)
    {
        float zoom = Camera.main.orthographicSize - deltaZoom * 0.01f * scrollSensitivity * Camera.main.orthographicSize;
        if (zoom > maxZoom)
        {
            zoom = maxZoom;
        }
        else if (zoom < minZoom)
        {
            zoom = minZoom;
        }
        //transform.localScale = zoom;
        Camera.main.orthographicSize = zoom;
        OnTranslate.Invoke();
    }
}
