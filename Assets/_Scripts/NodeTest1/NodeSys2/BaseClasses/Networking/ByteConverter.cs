
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

    public static EvaluableColorTable GetColorTable(byte[] bytes)
    {
        EvaluableColorTable table = new EvaluableColorTable(bytes.Length);
        for (int i = 0; i < bytes.Length; i++)
        {
            table.SetKey(i, bytes[i] / 255f);
        }
        return table;
    }
}
