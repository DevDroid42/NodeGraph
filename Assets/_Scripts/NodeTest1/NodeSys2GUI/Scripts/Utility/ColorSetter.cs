using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSetter : MonoBehaviour
{
    public bool limitToSprite;
    public Sprite[] imageSprite;
    public Color imageColor;
    Image[] images;
    // Start is called before the first frame update
    void Start()
    {
        images = GetComponentsInChildren<Image>();        
    }

    public void SetColor()
    {
        SetColor(imageColor);
    }

    public void SetColor(Color imageColor)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (limitToSprite)
            {
                for (int j = 0; j < imageSprite.Length; j++)
                {
                    if(images[i].sprite == imageSprite[j])
                    {
                        images[i].color = imageColor;
                    }
                }
            }
            else
            {
                images[i].color = imageColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
