using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace nodeSys2
{
    //JSON.NET cannot correctly deseralize enumerations stored in polymorphic arrays/variables
    //Currently it will serialize the ENUM as a string. However because the variable holding
    //it is of type object it will also be deserialized as a string instead of an ENUM.
    public class EnumUtils
    {
        //automatically will convert string to enum and reassign to the property
        public static void ConvertEnum<T>(Property prop) where T : Enum
        {
            if (prop.GetData().GetType() == typeof(string))
            {
                prop.SetData((T)Enum.Parse(typeof(T), (string)prop.GetData()));
            }
        }
    }
}
