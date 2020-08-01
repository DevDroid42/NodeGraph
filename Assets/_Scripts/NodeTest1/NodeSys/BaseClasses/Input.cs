using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace nodeSys
{
    public abstract class InputM { }

    [Serializable]
    public class Input<T> : InputM 
    {
        public string discription = "";
        public delegate void InputDataDel(string disc, T data);
        //This is invoked when a linked output delagate is invoked. This lets us take the raw data from the output link 
        //and attach a discription for processing inside the node
        public InputDataDel inputDataDel;
        public Input(string discription)
        {
            this.discription = discription;
        }

        public void Connect(Output<T> output)
        {
            output.outputData += HandleData;
        }

        public void Disconnect(Output<T> output)
        {
            output.outputData -= HandleData;
        }

        //this method is linked to the delagate in the attached output link
        public void HandleData(T data)
        {
            inputDataDel.Invoke(discription, data);
        }
    }
}
