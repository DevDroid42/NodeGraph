using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used whenever we want to mutate the state of an evaluable, by convention
//evaluables should not be mutable outside the context of this wrapper class
public class MutableEvaluableContainer<T> where T : Evaluable
{
    //this is a shared reference to evaluable state lock
    private StateLock stateLock;
    private T evaluable;

    public MutableEvaluableContainer(T evaluable, StateLock stateLock)
    {
        this.evaluable = evaluable;
        evaluable.InjectStateLock(stateLock);
    }

    public T Get()
    {
        //this will ensure we can get all of the properties within evaluable
        stateLock.UnlockState();
        return evaluable;
    }
}
