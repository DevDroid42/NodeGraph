using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPortHolder : MonoBehaviour
{
    public GameObject Port;
    public Text PortDisc;
    private RectTransform rt;
    private GUIPort guiPort;

    // Start is called before the first frame update
    void Start()
    {
        SetupPortPos();
        PortDisc.text = guiPort.portRef.portDisc;
    }

    public void SetupPortPos()
    {
        guiPort = Port.GetComponent<GUIPort>();
        rt = Port.GetComponent<RectTransform>();
        //if the port is an input put it on the left
        if (guiPort.isInputPort)
        {
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(0, 0.5f);
        }
        else
        {
            rt.anchorMin = new Vector2(1, 0.5f);
            rt.anchorMax = new Vector2(1, 0.5f);
        }        
    }


}
