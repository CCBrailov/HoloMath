using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operator : Token
{
    protected Token leftToken;
    protected Token rightToken;

    protected void LoadOperandTokens()
    {
        int index = expression.tokens.IndexOf(this);
        leftToken = expression.tokens[index - 1];
        rightToken = expression.tokens[index + 1];
    }
}