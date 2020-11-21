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
    public GameObject VectorEditor;
    public GameObject EnumEditor;

    float offset = 0;
    public void SetupEditors(List<Property> props)
    {
        for (int i = 0; i < props.Count; i++)
        {
            if (props[i].visible)
            {
                switch (props[i].GetData())
                {
                    case EvaluableFloat num:
                        {
                            SetupEditor(Instantiate(floatEditor, transform), props[i]);
                            break;
                        }
                    case EvaluableColorVec clrVec:
                        {
                            if (clrVec.displayMode == EvaluableColorVec.DisplayMode.Color)
                            {
                                SetupEditor(Instantiate(ColorEditor, transform), props[i]);
                            }
                            else
                            {
                                throw new NotImplementedException();
                                SetupEditor(Instantiate(floatEditor, transform), props[i]);
                            }
                            break;
                        }
                    case Enum _enum:
                        {
                            SetupEditor(Instantiate(EnumEditor, transform), props[i]);
                            break;
                        }
                    default:
                        {
                            SetupEditor(Instantiate(StringViewer, transform), props[i]);
                            break;
                        }

                }
            }
        }
    }

    private void SetupEditor(GameObject editor, Property prop)
    {
        editor.GetComponent<GenericEditor>().SetupEditor(prop);
        RectTransform rt = editor.GetComponent<RectTransform>();
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, offset, rt.rect.height);
        //advance the offset for the next component
        offset += rt.rect.height + 5;
    }
}
