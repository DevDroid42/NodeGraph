using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Evaluable test = new Evaluable();
        EvaluableColorVec test2 = new EvaluableColorVec(1);

        if (test.GetType().IsSubclassOf(typeof(EvaluableFloat)))
        {
            Debug.Log("hardcoded test Pass isa=" + (isa(test, typeof(Vector2))));
        }

        if (test2.GetType().IsSubclassOf(typeof(Evaluable)))
        {
            Debug.Log("hardcoded child test Pass isa=" + (isa(test2, typeof(Evaluable))));
        }

        if (test.GetType() == test.GetType())
        {
            Debug.Log("test Pass");
        }

        if(test2.GetType() == test.GetType())
        {
            Debug.Log("Child test pass");
        }
        isa(test, typeof(Evaluable));
    }

    private bool isa(object data, Type type)
    {
        return data.GetType() == type || data.GetType().IsSubclassOf(type);                
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
