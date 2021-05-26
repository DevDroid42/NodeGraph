using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphDepthIndicator : MonoBehaviour
{
    public Text text;
    
    public void SetIndicator(int depth)
    {
        if(depth == 0)
        {
            text.text = "Root";
        }
        else
        {
            text.text = depth.ToString();
        }
    }
}
