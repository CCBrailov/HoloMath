using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addition : Operator
{
    public Addition(Equation eq)
    {
        displayString = "+";
        equation = eq;
    }
}