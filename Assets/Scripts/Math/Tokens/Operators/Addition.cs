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

    }
}