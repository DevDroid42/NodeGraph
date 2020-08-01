using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys
{
    public class NetFloatReceive : Node
    {
        public float number;
        public Output<float> outLink = new Output<float>("Float Const");

        public NetFloatReceive(float number)
        {
            outputs = new OutputM[1];
            outputs[0] = outLink;
            this.number = number;
        }

        public override void UpdateNode()
        {
            outLink.outputData.Invoke(number);
        }

    }
}
