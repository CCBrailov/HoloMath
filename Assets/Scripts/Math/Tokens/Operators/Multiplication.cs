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
        LoadOperandTokens();

        if (leftToken is Term && rightToken is Term)
        {
            Term leftTerm = (Term)leftToken;
            Term rightTerm = (Term)rightToken;

            Token newToken;

            if (leftTerm.hasKnownValue && rightTerm.hasKnownValue)
            {
                float coeff = leftTerm.coeff * rightTerm.coeff;
                newToken = new Term(expression, coeff);
            }
            else
            {
                float coeff = leftTerm.coeff * rightTerm.coeff;
                string variable = leftTerm.variable + rightTerm.variable;
                newToken = new Term(expression, coeff, variable);
            }

            int index = expression.tokens.IndexOf(this.leftToken);
            expression.tokens.Remove(leftToken);
            expression.tokens.Remove(this);
            expression.tokens.Remove(rightToken);
            expression.tokens.Insert(index, newToken);
        }
    }
}