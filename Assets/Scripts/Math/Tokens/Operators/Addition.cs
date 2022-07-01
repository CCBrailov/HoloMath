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
        if (CanOperate())
        {
            Term leftTerm = (Term)leftToken;
            Term rightTerm = (Term)rightToken;

            float coeff = leftTerm.coeff + rightTerm.coeff;
            Token newToken = new Term(expression, coeff);

            int index = expression.tokens.IndexOf(this.leftToken);
            expression.tokens.Remove(leftToken);
            expression.tokens.Remove(this);
            expression.tokens.Remove(rightToken);
            expression.tokens.Insert(index, newToken); 
        }
    }

    protected bool CanOperate()
    {
        bool canOperate = false;

        expression.AddParentheses();
        LoadOperandTokens();
        expression.RemoveParentheses();

        if (leftToken is Term && rightToken is Term)
        {
            Term leftTerm = (Term)leftToken;
            Term rightTerm = (Term)rightToken;

            if (leftTerm.hasKnownValue & rightTerm.hasKnownValue)
            {
                canOperate = true;
            }
        }

        return canOperate;
    }
}