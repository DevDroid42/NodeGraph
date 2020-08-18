using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformExpander : MonoBehaviour
{
    public RectTransform target;
    public Vector2 closedSize;
    public Vector2 openedSize;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    Coroutine targetAnimation;
    public void Close()
    {
        if (targetAnimation != null)
        {
            //StopCoroutine(targetAnimation);
        }
        //targetAnimation = StartCoroutine(Animate(0.3f, 60, target.sizeDelta, closedSize));
        //target.position = parent.position;        
        target.sizeDelta = closedSize;
        target.anchoredPosition = new Vector2(0.5f, 0);
    }

    public void Open()
    {

        if (targetAnimation != null)
        {
            //StopCoroutine(targetAnimation);
        }
        //targetAnimation = StartCoroutine(Animate(0.3f, 60, target.sizeDelta, parent.sizeDelta));
        //target.position = parent.position;
        target.sizeDelta = openedSize;
        target.anchoredPosition = new Vector2(0.5f, 0.5f);
    }

    IEnumerator Animate(float seconds, int maxfps, Vector2 initalSize, Vector2 finalSize)
    {        
        int frames = (int)(seconds * maxfps + 0.5f);
        float delay = seconds / frames;
        for (int i = 0; i < frames; i++)
        {            
            //target.sizeDelta = Vector3.Slerp(initalSize, finalSize, i/frames);            
            yield return new WaitForSeconds(delay);
        }
        
    }

}
