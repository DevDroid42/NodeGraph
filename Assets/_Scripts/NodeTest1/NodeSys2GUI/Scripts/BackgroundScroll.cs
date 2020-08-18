using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BackgroundScroll : MonoBehaviour, IPointerClickHandler
{
    public float scrollSensitivity;
    public static Vector2 zoom = new Vector2(1,1);

    // Update is called once per frame
    void Update()
    {
        //zoom = new Vector2(
        //    transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSensitivity * zoom.x
        //    , transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSensitivity * zoom.x);
        transform.localScale = zoom;
    }

    public void SetZoom(InputAction.CallbackContext callback)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Draggable.DeselectAll();
    }
}
