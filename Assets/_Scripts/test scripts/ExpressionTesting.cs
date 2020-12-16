using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionTesting : MonoBehaviour
{
    Calculator calc;
    // Start is called before the first frame update
    void Start()
    {
        calc = new Calculator(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Evaluate(string input)
    {
        calc.x = 5;
        calc.y = 2;
        calc.setExpression(input);
        Debug.Log(calc.Evaluate());
        //try
        //{
        //    Debug.Log(evaluator.Evaluate(input));
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogWarning("SyntaxError");
        //    throw e;
        //}
        
    }
}
