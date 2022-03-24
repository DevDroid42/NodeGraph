
using System;
using UnityEngine;

//collection of static methods for converting NodeNet data types from bytes to abstractions (variables)
public class ByteConverter
{
    public static float GetFLoat(byte[] bytes)
    {
        float i = System.BitConverter.ToSingle(bytes, 0);
        //int i = BitConverter.to(bytes, 0);
        return i;
    }
}
