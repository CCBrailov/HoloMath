using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Expression
{

    public List<Token> tokens;

    public Expression()
    {
        tokens = new List<Token>
        {
            new Term(this, 3, 'x'),
            new Addition(this),
            new Term(this, 12)
        };
    }
}