using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Term : Token
{

    public float coeff;
    public char variable;
    public string displayString;

    public Term(float i)
    {
        coeff = i;
        displayString = i.ToString();
    }

    public Term(char v)
    {
        variable = v;
        displayString = v.ToString();
    }

    public Term(float i, char v)
    {
        coeff = i;
        variable = v;
        displayString = i.ToString() + v.ToString();
    }

    public override List<Token> Expand() 
    {
        List<Token> returnList = new List<Token>();
        returnList.Add(new Term(coeff));
        returnList.Add(new Multiplication());
        returnList.Add(new Term(variable));
        return returnList;
    }
}
