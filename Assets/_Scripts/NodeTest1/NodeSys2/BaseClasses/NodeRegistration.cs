using nodeSys2;
using UnityEngine;

public class NodeRegistration
{
    public enum NodeTypes {floatConst, add, Viewer, loop, ColorConst, ColorTable, LED}

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
            case NodeTypes.ColorConst:
                return new ColorConstant(true);
            case NodeTypes.LED:
                return new LedUDP(true);
            case NodeTypes.ColorTable:
                return new ColorTableNode(true);
            default:
                return null;

        }
    }
}
