using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasEvents : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
     , IEndDragHandler
{
    //this (ab)uses unity's built in functionality to translate between coordinate systems.
    //the world space position is set via transform then then we pull the new anchored position via rect transform
    public RectTransform pointCalculator;
    public RectTransform selectionBox;

    private void Awake()
    {
        selectionBox.gameObject.SetActive(false);
    }

    private Vector2 initialPos = new Vector2();
    private Vector2 endPos = new Vector2();
    Rect boxRect = new Rect();
    private void UpdateRect()
    {
        boxRect.position = initialPos;
        boxRect.size = (initialPos - endPos);
        boxRect.width *= -1;            
        if (boxRect.width < 0)
        {
            boxRect.x += boxRect.width;
            boxRect.width = Mathf.Abs(boxRect.width);
        }

        if (boxRect.height < 0)
        {
            boxRect.y -= boxRect.height;
            boxRect.height = Mathf.Abs(boxRect.height);
        }
        selectionBox.localPosition = boxRect.position;
        selectionBox.sizeDelta = boxRect.size;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        selectionBox.gameObject.SetActive(true);
        initialPos = GetRectPos(CanvasUtilities.RaycastPosWorld());
        selectionBox.localPosition = initialPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        endPos = GetRectPos(CanvasUtilities.RaycastPosWorld());
        UpdateRect();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBox.gameObject.SetActive(false);
    }

    private Vector2 GetRectPos(Vector2 worldPos)
    {
        pointCalculator.transform.position = worldPos;
        return pointCalculator.anchoredPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Draggable.DeselectAll();
        }
    }
}
