using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace nodeSys2
{
    //JSON.NET cannot correctly deseralize enumerations stored in polymorphic arrays/variables
    //This class aims to solve that problem by storing a generic version of the ENUM
    //In Theory the generic type should give JSON.NET the ability to correctly deserialize
    public class EnumWrapper<T> where T : Enum
    {
        public T enumeration;
    }
}
