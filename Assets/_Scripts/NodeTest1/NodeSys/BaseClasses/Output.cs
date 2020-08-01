using System;
using System.Collections;
using System.Collections.Generic;

namespace nodeSys
{
    public abstract class OutputM { }

    [Serializable]
    public class Output<T> : OutputM
    {
        public string discription;
        public delegate void OutputDataDel(T data);
        //the output data delagate is invoked by a node after processing is finished
        public OutputDataDel outputData;

        public Output(string discription)
        {
            this.discription = discription;
        }
    }
}
