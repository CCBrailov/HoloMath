using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquationViewController : MonoBehaviour
{

    public float coeff;
    public char variable;
    public TextMeshPro textObject;
    public string displayString;

    protected Term startTerm;

    protected List<Token> tokens;

    [ContextMenu("Expand")]
    public void ExpandTerm()
    {
        List<Token> newTokens = startTerm.Expand();
        int index = tokens.IndexOf(startTerm);
        tokens.Remove(startTerm);
        foreach(Token t in newTokens)
        {
            tokens.Add(t);
        }
    }

    protected string BuildDisplayString()
    {
        string tempString = "";
        foreach(Term t in tokens)
        {
            tempString += t.displayString;
        }
        return tempString;
    }


    // Start is called before the first frame update
    void Start()
    {
        startTerm = new Term(coeff, variable);
        tokens = new List<Token>
        {
            startTerm
        };
        displayString = BuildDisplayString();
    }

    // Update is called once per frame
    void Update()
    {
        displayString = BuildDisplayString();
        textObject.SetText(displayString);
    }
}
