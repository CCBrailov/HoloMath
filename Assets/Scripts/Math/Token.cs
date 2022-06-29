using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token
{
    public string displayString;
    public Expression expression;

    public void Expand()
    {
        OnExpand();
    }

    protected virtual void OnExpand()
    {
        
    }

    public void Simplify()
    {
        OnSimplify();
    }

    protected virtual void OnSimplify()
    {

    }
}