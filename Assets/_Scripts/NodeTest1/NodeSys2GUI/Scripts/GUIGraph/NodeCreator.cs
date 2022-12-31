using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeCreator : MonoBehaviour
{
    public GUIGraph graph;
    public EnumSelector enumSelector;
    NodeRegistration.NodeTypes nodeType = 0;

    // Start is called before the first frame update
    void Start()
    {
        enumSelector.SetUpEnum(typeof(NodeRegistration.NodeTypes), nodeType);
        enumSelector.selectionMade.AddListener(AddNode);
    }

    private void OnEnable()
    {
        GlobalInputDelagates.openMenu += OpenNodeMenu;
    }

    private void OnDisable()
    {
        GlobalInputDelagates.openMenu -= OpenNodeMenu;
    }

    
    void OpenNodeMenu()
    {
        enumSelector.transform.position = CanvasUtilities.RaycastPosWorld();
        //This is a band-aid solution. For some reason the z value is being set to random values when directly assigning position
        enumSelector.transform.localPosition = (Vector2)enumSelector.transform.localPosition;
        //enumSelector.SetUpEnum()
        enumSelector.OpenMenu();        
    }

    void AddNode(int type)
    {
        Debug.Log((NodeRegistration.NodeTypes)type);
        Vector2 pos = CanvasUtilities.RaycastPosWorld() * (float)(1/0.01019898) * 2;
        graph.AddNode((NodeRegistration.NodeTypes)type, new ColorVec(pos.x, pos.y));
    }
}
