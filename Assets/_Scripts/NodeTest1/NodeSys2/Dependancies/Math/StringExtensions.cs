using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StringExtensions
{
    public static class StringExtensions
    {
        public static string JavaSubstring(this string s, int start, int end)
        {
            return s.Substring(start, end - start);
        }

        public static string JavaSubstring(this string s, int start)
        {
            return s.Substring(start);
        }
    }
}
