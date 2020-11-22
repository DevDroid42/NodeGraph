using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
public class OnClickRefresh : MonoBehaviour, IPointerClickHandler
{
    public bool doubleref;
    public void OnPointerClick(PointerEventData eventData)
    {
        GUIGraph.updateGraphGUI.Invoke();
        if(doubleref)
            GUIGraph.updateGraphGUI.Invoke();
    }
}
