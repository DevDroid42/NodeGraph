
using System;
using UnityEngine;

//collection of static methods for converting NodeNet data types from bytes to abstractions (variables)
public class ByteConverter
{
    public static int GetInt(byte[] bytes)
    {
        int i = BitConverter.ToInt32(bytes, 0);
        return i;
    }
}
