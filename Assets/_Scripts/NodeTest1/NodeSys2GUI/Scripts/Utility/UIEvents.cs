using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIEvents : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public UnityEvent pointerEnter, SingleClick, DoubleClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if (clickCount == 1)
            OnSingleClick();
        else if (clickCount == 2)
            OnDoubleClick();
    }

    private void OnDoubleClick()
    {
        DoubleClick.Invoke();
    }

    private void OnSingleClick()
    {
        SingleClick.Invoke();        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerEnter.Invoke();
    }
}
