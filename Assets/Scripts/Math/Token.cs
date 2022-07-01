using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token
{
    public string displayString;
    public Expression expression;
    public bool visible = true;

    public void Expand()
    {
        OnExpand();
    }

    public void Simplify()
    {
        OnSimplify();
    }

    public void Hide()
    {
        visible = false;
    }
    
    public void Show()
    {
        visible = true;
    }

    protected virtual void OnExpand()
    {

    }

    protected virtual void OnSimplify()
    {

    }
}