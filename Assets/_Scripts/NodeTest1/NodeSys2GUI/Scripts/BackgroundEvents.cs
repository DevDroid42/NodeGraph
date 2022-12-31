using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundEvents : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Draggable.DeselectAll();
        }
    }

}
