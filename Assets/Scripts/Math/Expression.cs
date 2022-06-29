using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Expression
{

    public List<Token> tokens;

    public Expression()
    {
        Test1();
    }



    //-----Test Expressions------------------------------------

    // Test Expression 1
    // 3x + 12
    protected void Test1()
    {
        tokens = new List<Token>
        {
            new Term(this, 3, "x"),
            new Addition(this),
            new Term(this, 12)
        };
    }

    // Test Expression 2
    // 3 * 4
    protected void Test2()
    {
        tokens = new List<Token>
        {
            new Term(this, 3),
            new Multiplication(this),
            new Term(this, 4)
        };
    }

    // Test Expression 3
    // 3 * 4x
    protected void Test3()
    {
        tokens = new List<Token>
        {
            new Term(this, 3),
            new Multiplication(this),
            new Term(this, 4, "x")
        };
    }
}