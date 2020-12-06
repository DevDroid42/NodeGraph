using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System;

public class EditorManager : MonoBehaviour
{
    //This class is responsible for doing the pattern matching to determine the editor to choose and sending that down to the generic editor 
    //component in each editor
    List<GameObject> editors = new List<GameObject>();

    public GameObject floatEditor;
    public GameObject StringEditor;
    public GameObject StringViewer;
    public GameObject ColorEditor;
    public GameObject ColorTableEditor;
    public GameObject VectorEditor;
    public GameObject EnumEditor;

    public void SetupEditor(Property props, Transform EditorHolder)
    {
        if (props.visible)
        {

            switch (props.GetData())
            {
                case EvaluableFloat num:
                    {
                        SetupEditor(Instantiate(floatEditor, EditorHolder), props);
                        break;
                    }
                case EvaluableColorVec clrVec:
                    {
                        if (clrVec.displayMode == EvaluableColorVec.DisplayMode.Color)
                        {
                            SetupEditor(Instantiate(ColorEditor, EditorHolder), props);
                        }
                        else
                        {
                            throw new NotImplementedException();
                            SetupEditor(Instantiate(floatEditor, EditorHolder), props);
                        }
                        break;
                    }
                case EvaluableColorTable table:
                    {
                        SetupEditor(Instantiate(ColorTableEditor, EditorHolder), props);
                        break;
                    }
                case Enum _enum:
                    {
                        SetupEditor(Instantiate(EnumEditor, EditorHolder), props);
                        break;
                    }
                default:
                    {
                        SetupEditor(Instantiate(StringViewer, EditorHolder), props);
                        break;
                    }

            }

        }

    }

    private void SetupEditor(GameObject editor, Property prop)
    {
        editor.GetComponent<GenericEditor>().SetupEditor(prop);
        RectTransform rt = editor.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);
    }
}
