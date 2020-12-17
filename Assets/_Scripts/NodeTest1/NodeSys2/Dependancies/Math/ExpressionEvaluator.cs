using System;
using System.Collections;
using System.Collections.Generic;
using StringExtensions;
using UnityEngine;

public class ExpressionEvaluator
{
    private bool debug = false;
    private char[] decimals = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'E', '.', '-' };

    public string Evaluate(string expression, float input)
    {
        expression = RemoveSpaces(expression);
        expression = FormatSubtraction(expression);
        expression = ImplicitMult(expression);
        while (expression.Contains("x"))
        {
            expression = Sub(expression, input.ToString(), expression.IndexOf("x"), expression.IndexOf("x") + 1);
            if (debug)
                Debug.Log(expression);
        }
        return EvaluateR(expression);
    }

    public string Evaluate(string expression)
    {
        expression = RemoveSpaces(expression);
        expression = FormatSubtraction(expression);
        expression = ImplicitMult(expression);
        /*while (expression.Contains("x")) {
            expression = sub(expression, "" + Calculator.x, expression.IndexOf("x"), expression.IndexOf("x") + 1);
            if (debug)
                Debug.Log(expression);
        }*/
        return EvaluateR(expression);

    }

    private string EvaluateR(string expression)
    {
        if (debug)
            Debug.Log(expression);

        if (expression.Contains("("))
        {
            int start = expression.IndexOf("(") + 1;
            int end = ParenthesisIndex(expression);
            return EvaluateR(Sub(expression, EvaluateR(expression.JavaSubstring(start, end)), start - 1, end + 1));
        }
        else if (expression.Contains("sin"))
        {
            ExpressionData data = Isolate3CharFunction(expression, expression.IndexOf("sin"));
            return EvaluateR(Sub(expression, Sin(data.expression), data.lower, data.upper));
        }
        else if (expression.Contains("cos"))
        {

            ExpressionData data = Isolate3CharFunction(expression, expression.IndexOf("cos"));
            return EvaluateR(Sub(expression, Cos(data.expression), data.lower, data.upper));
        }
        else if (expression.Contains("tan"))
        {

            ExpressionData data = Isolate3CharFunction(expression, expression.IndexOf("tan"));
            return EvaluateR(Sub(expression, Tan(data.expression), data.lower, data.upper));
        }
        else if (expression.Contains("int"))
        {
            ExpressionData data = Isolate3CharFunction(expression, expression.IndexOf("int"));
            return EvaluateR(Sub(expression, IntCast(data.expression), data.lower, data.upper));
        }
        else if (expression.Contains("abs"))
        {
            ExpressionData data = Isolate3CharFunction(expression, expression.IndexOf("abs"));
            return EvaluateR(Sub(expression, Abs(data.expression), data.lower, data.upper));
        }
        else if (expression.Contains("rng"))
        {
            ExpressionData data = Isolate3CharFunction(expression, expression.IndexOf("rng"));
            return EvaluateR(Sub(expression, Rng(data.expression), data.lower, data.upper));
        }
        else if (expression.Contains("^"))
        {
            ExpressionData data = IsolateExpression(expression, expression.IndexOf('^'));
            return EvaluateR(Sub(expression, Power(data.expression), data.lower, data.upper));
        }
        else if (expression.Contains("*") || expression.Contains("/"))
        {
            if (DecideMult(expression))
            {
                ExpressionData data = IsolateExpression(expression, expression.IndexOf('*'));
                return EvaluateR(Sub(expression, Multiply(data.expression), data.lower, data.upper));
            }
            else
            {
                ExpressionData data = IsolateExpression(expression, expression.IndexOf('/'));
                return EvaluateR(Sub(expression, Divide(data.expression), data.lower, data.upper));
            }
        }
        else if (expression.Contains("+") || expression.Contains("~"))
        {
            if (DecideAdd(expression))
            {
                ExpressionData data = IsolateExpression(expression, expression.IndexOf('+'));
                return EvaluateR(Sub(expression, Add(data.expression), data.lower, data.upper));
            }
            else
            {
                ExpressionData data = IsolateExpression(expression, expression.IndexOf('~'));
                return EvaluateR(Sub(expression, Subtract(data.expression), data.lower, data.upper));
            }
        }
        else
        {
            return expression;
        }
    }

    public string ImplicitMult(string expression)
    {
        if (debug)
            Debug.Log("formating implicit multiplication: in:" + expression + "\tout:");
        expression = ImplcitInsert(expression, '(', ')', true, false);
        expression = ImplcitInsert(expression, ')', '(', false, true);
        if (debug)
            Debug.Log(expression);
        return expression;
    }

    /**
     * adds multiplication symbols adjacent to given character if they are next to a
     * number
     * 
     * @param expression
     * @param character
     * @param multChar   additional character to check against for multiplication
     * @param checkBack  whether should check back for implicit multiplication
     * @param checkFront whether should check front for implicit multiplication.
     * @return
     */
    private string ImplcitInsert(string expression, char character, char multChar, bool checkBack,
            bool checkFront)
    {
        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == character)
            {
                // if at the last character on the string don't check in front
                if (i != expression.Length - 1 && checkFront)
                    if (IsNumber(expression[i + 1]) || expression[i + 1] == multChar)
                    {
                        expression = Sub(expression, "*", i + 1, i + 1);
                    }

                // if at the first character of string don't check behind
                if (i != 0 && checkBack)
                    if (IsNumber(expression[i - 1]) || expression[i - 1] == multChar)
                    {
                        expression = Sub(expression, "*", i, i);
                    }
            }
        }
        return expression;
    }

    private string FormatSubtraction(string input)
    {
        if (debug)
            Debug.Log("Formatting subtraction in:" + input + "\t");
        while (input.Contains("-"))
        {
            int index = input.IndexOf("-");
            //if the character before the subtraction is not a number insert a zero so that negatives get processed correctly
            if (!IsNumber(input[index - 1]))
            {
                if (debug)
                {
                    Debug.Log("inserting 0 for negative processing");
                }
                //insert (0 before the subtraction
                input = Sub(input, "(0", index, index);
                index = input.IndexOf("-");
                //find the index of the end of the number
                for (int i = index+1; i < input.Length; i++)
                {
                    if (!IsNumber(input[i]) || i == input.Length - 1)
                    {
                        index = i;
                        break;
                    }                    
                }
                input = Sub(input, ")", index+1, index+1);
            }
            index = input.IndexOf("-");
            input = Sub(input, "~", index, index + 1);
        }
        if (debug)
            Debug.Log("Out: " + input);
        return input;
    }

    private string RemoveSpaces(string input)
    {
        while (input.Contains(" "))
        {
            input = Sub(input, "", input.IndexOf(" "), input.IndexOf(" ") + 1);
        }
        return input;
    }

    /**
     * determines if there are multiple multiplication and divisions in the
     * expression. If not it chooses the only option. If there are it chooses the
     * first one
     * 
     * @param expression
     * @return returns true for * and false for /
     */
    private bool DecideMult(string expression)
    {
        if (expression.Contains("*") && !expression.Contains("/"))
        {
            return true;
        }
        else if (!expression.Contains("*") && expression.Contains("/"))
        {
            return false;
        }
        else if (expression.IndexOf('*') < expression.IndexOf('/'))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /**
     * determines if there are multiple multiplication and divisions in the
     * expression. If not it chooses the only option. If there are it chooses the
     * first one
     * 
     * @param expression
     * @return returns true for + and false for -
     */
    private bool DecideAdd(string expression)
    {
        if (expression.Contains("+") && !expression.Contains("~"))
        {
            return true;
        }
        else if (!expression.Contains("+") && expression.Contains("~"))
        {
            return false;
        }
        else if (expression.IndexOf('+') < expression.IndexOf('~'))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private ExpressionData IsolateExpression(string expression, int index)
    {
        int lower = 0, upper = 0;
        for (int i = index - 1; i >= 0; i--)
        {
            if (!IsNumber(expression[i]))
            {
                lower = i;
                break;
            }
            if (i == 0)
            {
                lower = i - 1;
            }
        }

        for (int i = index + 1; i < expression.Length; i++)
        {
            if (!IsNumber(expression[i]))
            {
                upper = i - 1;
                break;
            }
            if (i == expression.Length - 1)
            {
                upper = i;
            }
        }
        // Debug.Log("lower: " + lower + "\t upper: " + upper);
        return new ExpressionData(lower + 1, upper + 1, expression.JavaSubstring(lower + 1, upper + 1));
    }

    //returns 
    private ExpressionData Isolate3CharFunction(string expression, int index)
    {
        int lower = index - 1, upper = 0;
        //add 3 to get to the number in front of trig function
        for (int i = index + 3; i < expression.Length; i++)
        {
            if (!IsNumber(expression[i]))
            {
                upper = i - 1;
                break;
            }
            if (i == expression.Length - 1)
            {
                upper = i;
            }
        }
        // Debug.Log("lower: " + lower + "\t upper: " + upper);
        return new ExpressionData(lower + 1, upper + 1, expression.JavaSubstring(lower + 1, upper + 1));
    }

    /**
     * evaluates an expression such as 2^3. Must only have two numbers and the
     * carrot operator.
     * 
     * @param Expression
     * @return
     */
    private string Power(string Expression)
    {
        float first = float.Parse(Expression.JavaSubstring(0, Expression.IndexOf('^')));
        float second = float.Parse(Expression.JavaSubstring(Expression.IndexOf('^') + 1));
        float result = (float)(Math.Pow(first, second));
        return result.ToString();
    }

    private string Multiply(string Expression)
    {
        float first = float.Parse(Expression.JavaSubstring(0, Expression.IndexOf('*')));
        float second = float.Parse(Expression.JavaSubstring(Expression.IndexOf('*') + 1));
        float result = first * second;
        return result.ToString();
    }

    private string Divide(string Expression)
    {
        float first = float.Parse(Expression.JavaSubstring(0, Expression.IndexOf('/')));
        float second = float.Parse(Expression.JavaSubstring(Expression.IndexOf('/') + 1));
        float result = first / second;
        return result.ToString();
    }

    private string Add(string Expression)
    {
        float first = float.Parse(Expression.JavaSubstring(0, Expression.IndexOf('+')));
        float second = float.Parse(Expression.JavaSubstring(Expression.IndexOf('+') + 1));
        float result = first + second;
        return result.ToString();
    }

    private string Subtract(string Expression)
    {
        float first = float.Parse(Expression.JavaSubstring(0, Expression.IndexOf('~')));
        float second = float.Parse(Expression.JavaSubstring(Expression.IndexOf('~') + 1));
        float result = first - second;
        return result.ToString();
    }

    private string Sin(string Expression)
    {
        float num = float.Parse(Expression.JavaSubstring(3));
        num = (float)Math.Sin(num);
        return num.ToString();
    }
    private string Cos(string Expression)
    {
        float num = float.Parse(Expression.JavaSubstring(3));
        num = (float)Math.Cos(num);
        return num.ToString();
    }
    private string Tan(string Expression)
    {
        float num = float.Parse(Expression.JavaSubstring(3));
        num = (float)Math.Tan(num);
        return num.ToString();
    }

    private string IntCast(string Expression)
    {
        float num = float.Parse(Expression.JavaSubstring(3));
        num = (int)num;
        return num.ToString();
    }

    private string Abs(string Expression)
    {
        float num = float.Parse(Expression.JavaSubstring(3));
        num = Math.Abs(num);
        return num.ToString();
    }

    private string Rng(string Expression)
    {
        float num = float.Parse(Expression.JavaSubstring(3));
        System.Random rng;
        if (num < 0)
        {
            rng = new System.Random();
        }
        else
        {
            rng = new System.Random((int)num);
        }
        num = (float)rng.NextDouble();
        return num.ToString();
    }

    /**
     * returns a string with another string inserted in place of a portion of the
     * input string. To insert without replacing set the start and end indexes equal
     * 
     * @param in
     * @param sub
     * @param start
     * @param end
     * @return
     */
    public string Sub(string input, string sub, int start, int end)
    {
        return input.JavaSubstring(0, start) + sub + input.JavaSubstring(end);
    }

    /**
     * returns the index of the closing parenthesis
     * 
     * @return
     */
    private int ParenthesisIndex(string input)
    {
        int n = 0;
        for (int i = input.IndexOf('(') + 1; i < input.Length; i++)
        {
            if (input[i] == '(')
                n++;
            if (input[i] == ')')
                n--;
            if (n < 0)
                return i;
        }
        Debug.Log("Syntax error parenthesis");
        return 0;
    }

    public bool IsNumber(char character)
    {
        for (int i = 0; i < decimals.Length; i++)
        {
            if (character == decimals[i])
                return true;
        }
        return false;
    }

}


//there is probably a better way to do this but this will work
class ExpressionData
{
    public ExpressionData(int lower, int upper, string expression)
    {
        this.lower = lower;
        this.upper = upper;
        this.expression = expression;
    }

    // the original bounds of the expression to be used when subbing the evaluated
    // expression back in
    public int lower, upper;
    public string expression;
}