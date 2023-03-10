using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLock
{
    private bool mutable = false;

    public void LockState()
    {
        mutable = false;
    }

    public void UnlockState()
    {
        mutable = true;
    }

    public bool IsMutable()
    {
        if (!mutable)
        {
            Debug.Log("attempted to get a variable within ");
        }
        return mutable;
    }
}
