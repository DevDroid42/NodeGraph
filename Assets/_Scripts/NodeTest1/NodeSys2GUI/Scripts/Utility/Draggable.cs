using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class Draggable : MonoBehaviour
     , IPointerClickHandler // 2
     , IPointerDownHandler
     , IBeginDragHandler
     , IDragHandler
     , IEndDragHandler
     , IPointerEnterHandler
     , IPointerExitHandler
{
    [Serializable]
    public class BoolEvent : UnityEvent<bool>
    {
    }

    private Image image;
    private Sprite initalSprite;
    public Sprite hoverSprite;
    public UnityEvent onDrag;
    public UnityEvent onEndDrag;
    public UnityEvent OnDelete;
    public Color defaultColor;
    public Color selectedColor;
    public ChildColorSetter childrenColor;
    public BoolEvent selectionChanged;
    private bool selected = false;
    //list of all other draggable items
    private static List<Draggable> draggables;

    private bool dragged;
    private Vector2 initialObjectPos;
    private Vector2 initialCursorPos;

    //used to move all slelected nodes at once
    public static UnityEvent drag;
    public static UnityEvent beginDrag;
    public static PointerEventData pointerData;

    private void Start()
    {
        image = GetComponent<Image>();
        initalSprite = image.sprite;
        if (draggables == null)
        {
            draggables = new List<Draggable>();
        }
        draggables.Add(this);
        if (drag == null)
        {
            drag = new UnityEvent();
        }
        drag.AddListener(Translate);
        if (beginDrag == null)
        {
            beginDrag = new UnityEvent();
        }
        beginDrag.AddListener(BeginTranslate);
    }

    private void OnDestroy()
    {
        draggables.Remove(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!selected)
            {
                DeselectAll();
                Select();
            }
            dragged = true;
            Select();
            pointerData = eventData;
            beginDrag.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            pointerData = eventData;
            drag.Invoke();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag.Invoke();
    }

    private void Translate()
    {
        onDrag.Invoke();
        if (selected)
            transform.position = initialObjectPos + ((Vector2)pointerData.pointerCurrentRaycast.worldPosition - initialCursorPos);
    }

    private void BeginTranslate()
    {
        if (selected)
        {
            initialObjectPos = transform.position;
            initialCursorPos = pointerData.pointerCurrentRaycast.worldPosition;
        }
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                if (!dragged)
                {
                    DeselectAll();
                }
            }


            if (selected && !dragged)
            {
                Deselect();
            }
            else
            {
                Select();                
            }

            dragged = false;
        }
    }

    public static void DeselectAll()
    {
        foreach (Draggable draggable in draggables)
        {
            draggable.Deselect();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = initalSprite;
    }

    public void Select()
    {
        selected = true;
        image.color = selectedColor;
        if (childrenColor != null)
        {
            childrenColor.SetColor(selectedColor);
        }
        selectionChanged.Invoke(selected);
    }

    public void Deselect()
    {        
        selected = false;
        image.color = defaultColor;
        if (childrenColor != null)
        {
            childrenColor.SetColor(defaultColor);
        }
        selectionChanged.Invoke(selected);
    }

    private void Delete()
    {
        if (selected)
        {
            OnDelete.Invoke();
            Destroy(gameObject);
        }
    }

    public bool IsSelected()
    {
        return selected;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetSiblingIndex(draggables.Count);
    }

    private void OnEnable()
    {
        GlobalInputDelagates.delete += Delete;
    }

    private void OnDisable()
    {
        GlobalInputDelagates.delete -= Delete;
    }
}
