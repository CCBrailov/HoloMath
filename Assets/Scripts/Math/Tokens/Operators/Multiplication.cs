using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplication : Operator
{
    public Multiplication(Expression ex)
    {
        displayString = "*";
        expression = ex;
    }

    protected override void OnSimplify()
    {
        int index = expression.tokens.IndexOf(this.leftToken);
        expression.tokens.Remove(leftToken);
        expression.tokens.Remove(this);
        expression.tokens.Remove(rightToken);

    }
}