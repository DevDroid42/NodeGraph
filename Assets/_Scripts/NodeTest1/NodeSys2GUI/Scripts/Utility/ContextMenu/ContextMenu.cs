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

    public void SetupMenu(Action<String> callback, List<String> options, Dictionary<String, Color> colors = null)
    {
        this.callback = callback;
        foreach (String option in options)
        {
            GameObject createdButton = Instantiate(button, transform);
            createdButton.SetActive(true);
            Text buttonText = createdButton.GetComponentInChildren<Text>();
            buttonText.text = option;
            if (colors != null && colors.ContainsKey(option))
            {
                buttonText.color = colors[option];
            }
            createdButton.GetComponent<ContextMenuButton>().callback = OptionSelected;
        }
    }

    private void OptionSelected(String selection)
    {
        callback.Invoke(selection);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
            StartCoroutine(DestroyCouroutine());
        }
    }

    IEnumerator DestroyCouroutine()
    {
        yield return null;
        Destroy(gameObject);
    }
}
