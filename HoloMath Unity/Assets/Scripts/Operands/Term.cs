using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Term : Token
{

    public float coeff;
    public char variable;

    protected bool simple;

    public Term(Equation eq, float i)
    {
        equation = eq;
        coeff = i;
        displayString = i.ToString();
        simple = true;
    }

    public Term(Equation eq, char v)
    {
        equation = eq;
        variable = v;
        displayString = v.ToString();
        simple = true;
    }

    public Term(Equation eq, float i, char v)
    {
        equation = eq;
        coeff = i;
        variable = v;
        displayString = i.ToString() + v.ToString();
        simple = false;
    }

    public override List<Token> Expand()
    {
        List<Token> returnList = new List<Token>();
        if (!simple)
        {
            returnList.Add(new Term(equation, coeff));
            returnList.Add(new Multiplication(equation));
            returnList.Add(new Term(equation, variable));
        }
        else
        {
            returnList.Add(this);
        }
        int index = equation.leftSide.IndexOf(this);
        equation.leftSide.Remove(this);
        foreach(Token t in returnList)
        {
            equation.leftSide.Insert(index, t);
        }
        return returnList;
    }
}
