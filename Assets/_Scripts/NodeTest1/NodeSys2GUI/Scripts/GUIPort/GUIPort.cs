using UnityEngine;
using System;
using UnityEngine.EventSystems;
using nodeSys2;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GUIPort : MonoBehaviour
     , IPointerClickHandler // 2
     , IPointerDownHandler
     , IDragHandler
{
    public GUIGraph GUIGraphRef;
    public GUINode GUINodeRef;
    public Port portRef;
    public bool isInputPort;
    public Color PortColor;
    public Sprite HoverSprite;
    public GameObject LinePrefab;
    private GameObject line;
    //used to store cursors position
    private GameObject cursor;
    private RectTransform ct;

    //we will track if the node is currently being dragged with this
    private bool selected = false;
    //list of ports we can snap to
    List<GUIPort> snappablePorts;
    //we don't want to be able to drag more than one port at a time
    private static bool dragInProgess = false;
    // Start is called before the first frame update
    void Start()
    {
        cursor = new GameObject();
        cursor.transform.SetParent(transform);
        //cursor.AddComponent<RawImage>();
        cursor.AddComponent<RectTransform>();
        ct = cursor.GetComponent<RectTransform>();
        ct.sizeDelta = new Vector2(10, 10);
    }

    private void PopulateListOfSnappablePorts()
    {
        snappablePorts = new List<GUIPort>();
        foreach (GameObject node in GUIGraphRef.guiNodes)
        {
            if (!node.activeSelf) continue;
            foreach (GUIPort port in node.GetComponentsInChildren<GUIPort>())
            {
                //we can only snap to differing port types
                if (port.isInputPort != isInputPort)
                {
                    snappablePorts.Add(port);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            Drag();
        }
        if (dragInProgess && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            EndDrag();
        }
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        //print("I was clicked at " + eventData.position);
        if (isInputPort && portRef.IsConnected())
        {
            portRef.Disconnect();
            GUIGraphRef.ActionPreformed();
            GUIGraph.updateGraphGUI.Invoke();
        }
        else if (!dragInProgess)
        {
            BeginDrag();
        }

    }

    private void BeginDrag()
    {
        selected = true;
        dragInProgess = true;
    }

    private void Drag()
    {
        //Debug.Log("dragging");
        if (line != null)
        {
            Destroy(line);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            PopulateListOfSnappablePorts();
        }
        Vector2 mousePos = CanvasUtilities.RaycastPosWorld();
        //snap to nearest 
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            GUIPort minPort = null;
            float minDist = float.PositiveInfinity;
            foreach (GUIPort port in snappablePorts)
            {
                float dist = Vector2.Distance(port.transform.position, mousePos);
                if (dist < minDist)
                {
                    minDist = dist;
                    minPort = port;
                }
            }
            if (minPort == null)
            {
                ct.position = transform.position;
            }
            else
            {
                ct.position = minPort.transform.position;
            }
            if (Input.GetMouseButtonDown(0) && minPort != null)
            {
                ConnectToPort(minPort);
            }
        }
        else
        {
            ct.position = mousePos;
        }
        line = GUIGraph.DrawLinesFromRect(gameObject, cursor, LinePrefab, transform);
    }

    private void EndDrag()
    {
        selected = false;
        dragInProgess = false;
        if (CanvasUtilities.TryGetRaycastComponent<GUIPort>(out GUIPort otherPort))
        {
            ConnectToPort(otherPort);
        }
        else
        {
            Destroy(line);
        }

        if (line != null)
        {
            Destroy(line);
        }
    }

    private void ConnectToPort(GUIPort otherPort)
    {
        //if so make sure it's a different type of port
        if (otherPort.isInputPort == isInputPort) return;

        //run the connection method on the input port
        if (isInputPort)
        {
            if (portRef.IsConnected())
            {
                portRef.Disconnect();
            }
            portRef.Connect(otherPort.portRef);
        }
        else
        {
            if (otherPort.portRef.IsConnected())
            {
                otherPort.portRef.Disconnect();
            }
            otherPort.portRef.Connect(portRef);
        }
        GUIGraph.updateGraphGUI.Invoke();
        GUIGraphRef.ActionPreformed();
    }

    public void OnDrag(PointerEventData eventData)
    {
        BeginDrag();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
