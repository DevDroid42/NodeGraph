using Newtonsoft.Json;
using UnityEngine;

public interface ICopyable
{
    object GetCopy();
}

public interface IEvaluable : ICopyable {

    //Note: should always return a new color, not one with a reference inside the evaluable object
    ColorVec EvaluateColor(float vector = 0);

    float EvaluateValue(float vector = 0);

    //used to get the resolution of the Evaluable data type.
    int GetResolution();
}