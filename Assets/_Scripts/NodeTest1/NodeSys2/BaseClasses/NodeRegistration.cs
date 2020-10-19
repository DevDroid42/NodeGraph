using nodeSys2;
using UnityEngine;

public class NodeRegistration
{
    public enum NodeTypes {floatConst, add, Viewer, loop}

    public static Node GetNode(NodeTypes type)
    {
        switch (type)
        {
            case NodeTypes.floatConst:
                return new FloatConstant(true);
            case NodeTypes.add:
                return new AddNode(true);                
            case NodeTypes.Viewer:
                return new ViewerNode(true);
            case NodeTypes.loop:
                return new LoopNode(true);
            default:
                return null;

        }
    }
}
