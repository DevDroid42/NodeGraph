using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAcessor : MonoBehaviour
{
    public static GameObject canvas;
    // Start is called before the first frame update
    void Awake()
    {
        canvas = gameObject;
    }

}
