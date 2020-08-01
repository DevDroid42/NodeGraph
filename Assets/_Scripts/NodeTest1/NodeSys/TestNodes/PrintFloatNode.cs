using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys
{
    public class PrintNode : Node
    {
        Input<float> input = new Input<float>("input");

        public PrintNode()
        {
            inputs = new InputM[1];
            inputs[0] = input;
        }
        public override void HandleData<T>(string discription, T dataType)
        {
            Debug.Log(discription + dataType.ToString());
        }

    }
}
