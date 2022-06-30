using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomToken : Token
{
    public CustomToken(Expression ex, string s)
    {
        expression = ex;
        displayString = s;
    }
}
