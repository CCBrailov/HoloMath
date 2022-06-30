using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addition : Operator
{
    public Addition(Expression ex)
    {
        displayString = "+";
        expression = ex;
    }

    protected override void OnSimplify()
    {
        LoadOperandTokens();

        if (!(leftToken is Term) || !(rightToken is Term))
        {
            return;
        }

        Term leftTerm = (Term)leftToken;
        Term rightTerm = (Term)rightToken;

        Token newToken;

        if (leftTerm.hasKnownValue & rightTerm.hasKnownValue)
        {
            float coeff = leftTerm.coeff + rightTerm.coeff;
            newToken = new Term(expression, coeff);
        }
        else
        {
            return;
        }

        int index = expression.tokens.IndexOf(this.leftToken);
        expression.tokens.Remove(leftToken);
        expression.tokens.Remove(this);
        expression.tokens.Remove(rightToken);
        expression.tokens.Insert(index, newToken);
    }
}