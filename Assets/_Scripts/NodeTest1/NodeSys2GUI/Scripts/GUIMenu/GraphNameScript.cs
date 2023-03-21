using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GraphNameScript : MonoBehaviour
{
    public InputField nameField;

    public void GUIUpdated()
    {
        nameField.text = GUIGraph.currentInstance.GetCurrentName();
    }

    public void UpdateName(string name)
    {
        GUIGraph.currentInstance.SetCurrentName(name);
    }
}
