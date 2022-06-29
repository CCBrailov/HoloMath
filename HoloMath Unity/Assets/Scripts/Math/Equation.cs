using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Equation
{

    public List<Token> leftSide;
    //private List<Token> rightSide;

    public Equation()
    {
        leftSide = new List<Token>
        {
            new Term(this, 3, 'x'),
            new Addition(this),
            new Term(this, 12)
        };
    }
}