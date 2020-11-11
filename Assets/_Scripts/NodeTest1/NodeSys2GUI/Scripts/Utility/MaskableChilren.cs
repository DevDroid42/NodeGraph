using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskableChilren : MonoBehaviour
{    
    public bool maskable;
    Image[] images;
    // Start is called before the first frame update
    void Start()
    {
        images = GetComponentsInChildren<Image>();

        for (int i = 0; i < images.Length; i++)
        {
            for (int j = 0; j < images.Length; j++)
            {
                images[i].maskable = maskable;
            }

        }
    }
}
