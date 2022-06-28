using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token
{

    public virtual List<Token> Expand()
    {
        return null;
    }

    public virtual List<Token> Simplify()
    {
        return null;
    }
}
