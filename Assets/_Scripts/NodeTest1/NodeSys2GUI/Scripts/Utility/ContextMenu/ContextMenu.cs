using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    //button prefab
    public GameObject button;

    private Action<String> callback;

    public void OpenMenu(Action<String> callback, List<String> options)
    {
        this.callback = callback;
        foreach (String option in options)
        {
            GameObject createdButton = Instantiate(button, transform);
            createdButton.SetActive(true);
            createdButton.GetComponentInChildren<Text>().text = option;
            createdButton.GetComponent<ContextMenuButton>().callback = OptionSelected;
        }
    }

    private void OptionSelected(String selection)
    {
        callback.Invoke(selection);
        Destroy(gameObject);
    }
}
