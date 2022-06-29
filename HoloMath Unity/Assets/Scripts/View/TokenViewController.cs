using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenViewController : MonoBehaviour
{
    //Temporary fields for debugging
    //Allows dev to input coefficient and variable, assumes Token will be a Term
    public float coeff;
    public char variable;
    //------------------------------

    public Token token;
    public TextMeshPro textMesh;
    public EquationViewController equationView;

    [ContextMenu("Expand")]
    public void Expand()
    {
        List<Token> newTokens = token.Expand();
        foreach(Token t in newTokens)
        {
            GameObject tokenView = Instantiate(gameObject, equationView.transform);
            TokenViewController controller = (TokenViewController)tokenView.GetComponent("TokenViewController");
            controller.token = t;
            tokenView.name = t.displayString;
        }

        Destroy(gameObject);
    }

    //Temporary method for debugging
    //Loads in Term Token based on coeff + variable
    [ContextMenu("Rebuild Token")]
    public void RebuildToken()
    {
        token = new Term(equationView.equation, coeff, variable);
    }
    //---------------------------------------------

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        textMesh.SetText(token.displayString);
        gameObject.name = token.displayString;
    }
}
