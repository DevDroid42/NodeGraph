using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys
{
    public abstract class Node
    {
        public InputM[] inputs;
        public OutputM[] outputs; 
        //this method will be both overridden and overloaded in subclasses. When a method is linked to the input link delagate, 
        //it will use overloading to select the correct method to handle the data type.
        public virtual void HandleData<T>(string discription, T dataType) 
        {

        }

        public virtual void UpdateNode() { }
    }
}
