using nodeSys2;
using UnityEngine;

public class NodeRegistration
{
    public enum NodeTypes {NumberConstant, Add, Loop, AdvancedLoop, ColorConst, ColorTable, Transform , LED, Math, mixRGB, 
        NetReceive, GroupInput, GroupOutput, Group, StaticInstancer, logic}

    public static Node GetNode(NodeTypes type)
    {
        switch (type)
        {
            case NodeTypes.NumberConstant:
                return new FloatConstant(true);
            case NodeTypes.Add:
                return new AddNode(true);
            case NodeTypes.Loop:                               
                return new LoopNode(true);
            case NodeTypes.AdvancedLoop:
                return new AdvancedLoopNode(true);
            case NodeTypes.ColorConst:
                return new ColorConstant(true);
            case NodeTypes.LED:
                return new LedUDP(true);
            case NodeTypes.ColorTable:
                return new ColorTableNode(true);
            case NodeTypes.Transform:
                return new TransformNode(true);
            case NodeTypes.Math:
                return new MathNode(true);
            case NodeTypes.NetReceive:
                return new netReceiveNode(true);
            case NodeTypes.GroupInput:
                return new GroupInputNode(true);
            case NodeTypes.GroupOutput:
                return new GroupOutputNode(true);
            case NodeTypes.Group:
                return new GroupNode(true);
            case NodeTypes.mixRGB:
                return new ColorMixNode(true);
            case NodeTypes.StaticInstancer:
                return new InstancerNode(true);
            case NodeTypes.logic:
                return new LogicNode(true);
            default:
                return null;

        }
    }
}
