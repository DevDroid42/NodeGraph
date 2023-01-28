using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ContextMenuButton : MonoBehaviour
{
    public Action<String> callback;
    public Text txt;

    public void Pressed()
    {
        callback.Invoke(txt.text);
    }
}
