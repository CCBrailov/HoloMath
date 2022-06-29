using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplication : Operator
{

    public Multiplication(Equation eq)
    {
        displayString = "*";
        equation = eq;
    }
}
