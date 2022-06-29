using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Term : Token
{

    public float coeff;
    public char variable;

    protected bool simple;

    public Term(Expression ex, float i)
    {
        expression = ex;
        coeff = i;
        displayString = i.ToString();
        simple = true;
    }

    public Term(Expression ex, char v)
    {
        expression = ex;
        variable = v;
        displayString = v.ToString();
        simple = true;
    }

    public Term(Expression ex, float i, char v)
    {
        expression = ex;
        coeff = i;
        variable = v;
        displayString = i.ToString() + v.ToString();
        simple = false;
    }

    protected override void OnExpand()
    {
        List<Token> returnList = new List<Token>();
        if (!simple)
        {
            returnList.Add(new Term(expression, coeff));
            returnList.Add(new Multiplication(expression));
            returnList.Add(new Term(expression, variable));
        }
        else
        {
            returnList.Add(this);
        }
        int index = expression.tokens.IndexOf(this);
        expression.tokens.Remove(this);
        foreach (Token t in returnList)
        {
            expression.tokens.Insert(index + returnList.IndexOf(t), t);
        }
    }
}