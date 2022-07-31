using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class AutoCompleteButtonAddon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public AutoCompExtended box;
    public Image image;
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        box.OnValChangedEvent.AddListener(SetColor);
    }

    void SetColor()
    {
        if(text.text == box.Text)
        {
            text.color = box.ValidSelectionTextColor;
        }
        else
        {
            text.color = box.MatchingItemsRemainingTextColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        box.OnSelectionChanged.Invoke(text.text, true);
        GlobalInputDelagates.select.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = defaultSprite;
    }
}
