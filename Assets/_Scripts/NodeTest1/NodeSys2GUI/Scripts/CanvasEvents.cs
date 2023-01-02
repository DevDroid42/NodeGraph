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
    public Transform nodeParent;

    private bool dragging = false;
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
        dragging = true;
        if (!(eventData.button == PointerEventData.InputButton.Left))
        {
            return;
        }
            selectionBox.gameObject.SetActive(true);
        initialPos = GetRectPos(CanvasUtilities.RaycastPosWorld());
        selectionBox.localPosition = initialPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!(eventData.button == PointerEventData.InputButton.Left))
        {
            return;
        }
        endPos = GetRectPos(CanvasUtilities.RaycastPosWorld());
        UpdateRect();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        if (!(eventData.button == PointerEventData.InputButton.Left))
        {
            return;
        }
        selectionBox.gameObject.SetActive(false);
        SelectNodes(initialPos, endPos);
    }

    private void SelectNodes(Vector2 start, Vector2 end)
    {
        if(!Input.GetKey(KeyCode.LeftShift)){
            Draggable.DeselectAll();
        }
        Bounds bounds = new Bounds((start + end) / 2, (end-start));
        Vector3 ext = bounds.extents;
        ext.x = Mathf.Abs(ext.x);
        ext.y = Mathf.Abs(ext.y);
        ext.z = float.PositiveInfinity;
        bounds.extents = ext; 
        for (int i = 0; i < nodeParent.childCount; i++)
        {
            Transform child = nodeParent.GetChild(i);
            if (!child.gameObject.activeSelf) return;
            Vector3[] corners = new Vector3[4];
            child.GetComponent<RectTransform>().GetWorldCorners(corners);
            foreach (Vector2 corner in corners)
            {
                Vector2 pos = GetRectPos(corner);
                if (bounds.Contains(pos))
                {
                    child.GetComponent<Draggable>().Select();
                    break;
                }
            }
        }
    }

    private Vector2 GetRectPos(Vector2 worldPos)
    {
        pointCalculator.transform.position = worldPos;
        return pointCalculator.anchoredPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (dragging) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Draggable.DeselectAll();
        }
    }
}
