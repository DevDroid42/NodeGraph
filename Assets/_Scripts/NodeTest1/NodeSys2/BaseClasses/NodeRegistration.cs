using nodeSys2;
using UnityEngine;

public class NodeRegistration
{
    public enum NodeTypes {integerConst, add, Viewer}

    public static Node GetNode(NodeTypes type)
    {
        switch (type)
        {
            case NodeTypes.integerConst:
                return new IntConstant();                
            case NodeTypes.add:
                return new AddNode();                
            case NodeTypes.Viewer:
                return new ViewerNode();                
            default:
                return null;

        }
    }
}
