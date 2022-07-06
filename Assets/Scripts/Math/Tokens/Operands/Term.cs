using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Term : Operand
{
    public float coeff = 1;
    public string variable = "";

    public bool hasKnownValue = false;
    public float knownValue;

    protected bool simple;

    public Term(Expression ex, float f)
    {
        expression = ex;
        coeff = f;
        SetKnownValue(f);
        BuildDisplayString();
        simple = true;
    }

    public Term(Expression ex, string v)
    {
        expression = ex;
        variable = v;
        BuildDisplayString();
        simple = true;
    }

    public Term(Expression ex, float f, string v)
    {
        expression = ex;
        coeff = f;
        variable = v;
        BuildDisplayString();
        simple = false;
    }

    public void BuildDisplayString()
    {
        symbolString = "";
        if(variable.Equals(""))
        {
            symbolString = coeff.ToString();
        } 
        else if(coeff == 1) 
        {
            symbolString = variable.ToString();
        } 
        else
        {
            symbolString = coeff.ToString() + variable.ToString();
        }
    }

    protected void SetKnownValue(float f)
    {
        hasKnownValue = true;
        knownValue = f;
    }

    protected override void OnExpand()
    {
        List<Token> newTokens = new List<Token>();
        if (!simple)
        {
            newTokens.Add(new Term(expression, coeff));
            newTokens.Add(new Multiplication(expression));
            newTokens.Add(new Term(expression, variable));
        }
        else
        {
            newTokens = FactorizeToken();
        }
        Replace(newTokens);
    }

    protected override void OnSimplify()
    {

    }


    // If this Term is a constant (i.e. 12), factor the constant and return new Terms and Operators (i.e. 2 * 2 * 3)
    // If this Term is not a constant, return the term unchanged
    protected List<Token> FactorizeToken()
    {
        List<Token> newTokens = new List<Token>();

        // If this token has no known value, or if it is a non-integer constant, 
        if (!hasKnownValue || Mathf.Floor(coeff) != coeff) 
        {
            newTokens.Add(this);
            return newTokens;
        }

        List<float> factors = FactorizeInteger((int)coeff);
        foreach (float f in factors)
        {
            newTokens.Add(new Term(expression, f));
            newTokens.Add(new Multiplication(expression));
        }
        newTokens.RemoveAt(newTokens.Count - 1);

        return newTokens;
    }
    protected void Replace(List<Token> newTokens)
    {
        int index = expression.tokens.IndexOf(this);
        expression.tokens.Remove(this);
        foreach (Token t in newTokens)
        {
            expression.tokens.Insert(index + newTokens.IndexOf(t), t);
        }
    }
    // Takes in integer, returns factorized integer as list of floats
    // Source: https://www.geeksforgeeks.org/print-all-prime-factors-of-a-given-number/
    protected List<float> FactorizeInteger(int n)
    {
        List<float> factors = new List<float>();

        while (n % 2 == 0)
        {
            factors.Add(2);
            n /= 2;
        }

        for (int i = 3; i <= Mathf.Sqrt((float)n); i += 2)
        {
            while(n % i == 0)
            {
                factors.Add(i);
                n /= i;
            }
        }

        if (n > 2)
        {
            factors.Add(n);
        }

        return factors;
    }
}