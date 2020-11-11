using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteData 
{
    //for testing operator overloading
    public byte number;
    
    public ByteData(byte num)
    {
        number = num;
    }

    public static ByteData operator +(ByteData a) => a;
    public static ByteData operator +(ByteData a, ByteData b) => new ByteData((byte)(a.number + b.number));
    public static ByteData operator -(ByteData a, ByteData b) => new ByteData((byte)(a.number + b.number));

    public static implicit operator int(ByteData b) => b.number;
    public static explicit operator ByteData(int a) => new ByteData((byte)a);

    public override string ToString()
    {
        return "ByteData " + number;
    }

}
