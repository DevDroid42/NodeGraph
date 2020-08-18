using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Diagnostics.Eventing.Reader;

[RequireComponent(typeof(Image))]
public class Draggable : MonoBehaviour
     , IPointerClickHandler // 2
     , IBeginDragHandler
     , IDragHandler
     , IEndDragHandler
     , IPointerEnterHandler
     , IPointerExitHandler
{

    private Image image;
    private Sprite initalSprite;
    public Sprite hoverSprite;
    public UnityEvent onDrag;    
    public Color defaultColor;
    public Color selectedColor;
    public ColorSetter childrenColor;
    private bool selected = false;
    //list of all other draggable items
    private static List<Draggable> draggables;

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
        if(draggables == null)
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        Select();
        pointerData = eventData;
        beginDrag.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        pointerData = eventData;
        drag.Invoke();        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Deselect();
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
        //if shift isn't held
        foreach (Draggable draggable in draggables)
        {
            //draggable.Deselect();
        }

        if (selected)
        {
            Deselect();
        }
        else
        {
            Select();
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
    }

    public void Deselect()
    {
        selected = false;
        image.color = defaultColor;
        if (childrenColor != null)
        {
            childrenColor.SetColor(defaultColor);
        }
    }

    public bool IsSelected()
    {
        return selected;
    }
}
