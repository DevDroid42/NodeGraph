using nodeSys2;
using UnityEngine;

public class NodeRegistration
{
    //public enum NodeGroups {,Instancers,Math,Mixing}

    public enum NodeTypes
    {
        NumberConstant, Add, Loop, AdvancedLoop, ColorConst, ColorTable, Gradient, Point, Noise, Transform,
        LED, Equation, Math, mixRGB, NetReceive, MidiReceive, MidiInstancer, MidiInfo, GroupInput, GroupOutput, Group,
        StaticInstancer, DynamicInstancer, InstancedPulse, InstanceInfo, logic, udpSend, recording, remap, pulseRouter, Switch,
        Follow
    }

    public static Node GetNode(NodeTypes type, ColorVec pos)
    {
        switch (type)
        {
            case NodeTypes.NumberConstant:
                return new FloatConstant(pos);
            case NodeTypes.Add:
                return new AddNode(pos);
            case NodeTypes.Loop:
                return new LoopNode(pos);
            case NodeTypes.AdvancedLoop:
                return new AdvancedLoopNode(pos);
            case NodeTypes.ColorConst:
                return new ColorConstant(pos);
            case NodeTypes.LED:
                return new LedUDP(pos);
            case NodeTypes.ColorTable:
                return new ColorTableNode(pos);
            case NodeTypes.Transform:
                return new TransformNode(pos);
            case NodeTypes.Equation:
                return new EquationNode(pos);
            case NodeTypes.NetReceive:
                return new NetReceiveNode(pos);
            case NodeTypes.GroupInput:
                return new GroupInputNode(pos);
            case NodeTypes.GroupOutput:
                return new GroupOutputNode(pos);
            case NodeTypes.Group:
                return new GroupNode(pos);
            case NodeTypes.Math:
                return new MathNode(pos);
            case NodeTypes.mixRGB:
                return new ColorMixNode(pos);
            case NodeTypes.StaticInstancer:
                return new StaticInstancer(pos);
            case NodeTypes.DynamicInstancer:
                return new DynamicInstancer(pos);
            case NodeTypes.InstancedPulse:
                return new InstancedPulseNode(pos);
            case NodeTypes.InstanceInfo:
                return new InstanceInfoNode(pos);
            case NodeTypes.logic:
                return new LogicNode(pos);
            case NodeTypes.udpSend:
                return new LedUDP(pos);
            case NodeTypes.recording:
                return new RecordingNode(pos);
            case NodeTypes.remap:
                return new RemapNode(pos);
            case NodeTypes.pulseRouter:
                return new PulseRouterNode(pos);
            case NodeTypes.Switch:
                return new SwitchNode(pos);
            case NodeTypes.Follow:
                return new FollowNode(pos);
            case NodeTypes.MidiReceive:
                return new MidiReceiveNode(pos);
            case NodeTypes.MidiInstancer:
                return new MidiInstancer(pos);
            case NodeTypes.MidiInfo:
                return new MidiInfoNode(pos);
            case NodeTypes.Noise:
                return new NoiseNode(pos);
            case NodeTypes.Point:
                return new PointNode(pos);
            case NodeTypes.Gradient:
                return new GradientNode(pos);
            default:
                return null;
        }
    }
}
