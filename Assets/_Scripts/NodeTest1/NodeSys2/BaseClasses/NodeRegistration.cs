using nodeSys2;
using UnityEngine;

public class NodeRegistration
{
    public enum NodeTypes {add, Viewer, loop}

    public static Node GetNode(NodeTypes type)
    {
        switch (type)
        {           
            case NodeTypes.add:
                return new AddNode();                
            case NodeTypes.Viewer:
                return new ViewerNode();
            case NodeTypes.loop:
                return new LoopNode();
            default:
                return null;

        }
    }
}
