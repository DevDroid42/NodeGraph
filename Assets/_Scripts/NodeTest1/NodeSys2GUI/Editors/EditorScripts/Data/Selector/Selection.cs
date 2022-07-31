using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selector : MonoBehaviour
{



    public void SelectElement(SelectionElement element)
    {
        if (element is Folder folder)
        {

        }
    }
}

public class SelectionElement
{
    string name;
    public SelectionElement(string name)
    {
        this.name = name;
    }

    public override string ToString()
    {
        return name;
    }
}

public class Folder : SelectionElement
{
    List<SelectionElement> elements = new List<SelectionElement>();

    public Folder(string name) : base(name)
    {

    }

    public void AddElements(string[] elems)
    {
        foreach(string elmnt in elems)
        {
            elements.Add(new Element(elmnt));
        }
    }
}

public class Element : SelectionElement
{
    public Element(string name) : base(name)
    {

    }
}

