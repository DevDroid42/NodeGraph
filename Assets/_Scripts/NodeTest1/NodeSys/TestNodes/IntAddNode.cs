using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine.XR;

namespace nodeSys
{
    public class FloatAddNode : Node
    {
        Input<float> Num1Link = new Input<float>("num1");
        Input<float> Num2Link = new Input<float>("num2");
        Output<float> outLink = new Output<float>("Added Out");        
        
        float num1 = 0, num2 = 0;
        public FloatAddNode()
        {
            inputs = new InputM[2];
            inputs[0] = Num1Link;
            inputs[1] = Num2Link;
            outputs = new OutputM[1];
            outputs[0] = outLink;
            //link up the delagates to the handle data method, this method will sort based on data types and
            //send to appropriate methods for processing. Because only nodes of the same data type can be connected, 
            //it should be impossible to receive a data type not of the type defined. 
            Num1Link.inputDataDel += HandleData;
            Num2Link.inputDataDel += HandleData;
        }

        public override void HandleData<T>(string discription, T data)
        {
            if (typeof(T) == typeof(float))
            {
                ProcessData(discription, (float)(object)data);
            }
            else 
            {
                throw new InvalidDataException();
            }
            /*Example of how to handle multiple data types in one node. Because this node only uses floats
             * there is no need to do any checking but this is how you would check the data types otherwise
             * 
            switch (typeof(T))
            {
                case Type intType when (intType == typeof(int)) || (intType == typeof(float)):
                    ProcessData(discription, ((int)(object)dataType));
                    break;
                default:
                    throw new InvalidDataException();                    
            }
            */
        }

        public void ProcessData(string disc, float input) 
        {
            if (disc == "num1")
                num1 = input;
            else if (disc == "num2")
                num2 = input;
            outLink.outputData.Invoke(0);
        }
    }
}