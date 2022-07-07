using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ExpressionController : MonoBehaviour
{
    public Expression expression;
    public List<TokenController> tokenControllers;
    public float spacing = 0.3f;

    public string expressionString;

    public GameObject tokenPrefab;

    public void BuildTokenControllers()
    {
        DestroyTokenControllers();
        Debug.Log("Building Token Controllers");
        foreach (Token t in expression.tokens)
        {
            GameObject tokenObject = Instantiate(tokenPrefab, transform.position, Quaternion.identity, transform);
            TokenController controller = tokenObject.GetComponent<TokenController>();
            controller.token = t;
            controller.expressionController = this;
            tokenControllers.Add(controller);
        }
        PositionTokenControllers();
    }

    protected void PositionTokenControllers()
    {
        float expressionWidth = 0;

        foreach (TokenController t in tokenControllers)
        {
            expressionWidth += t.textMesh.bounds.size.x + spacing * 2;
        }

        float tokenCenter = 0 - (expressionWidth / 2);
        float thisTokenExtent;
        float previousTokenExtent = 0;

        foreach (TokenController t in tokenControllers)
        {
            thisTokenExtent = t.textMesh.bounds.extents.x + spacing; // Half of this token's width
            tokenCenter += thisTokenExtent + previousTokenExtent;
            Vector3 position = new(
                gameObject.transform.position.x + tokenCenter,
                gameObject.transform.position.y,
                gameObject.transform.position.z);
            t.gameObject.transform.position = position;
            previousTokenExtent = t.textMesh.bounds.extents.x + spacing;
        }
    }

    protected void DestroyTokenControllers()
    {
        Debug.Log("Destroying Token Controllers");
        foreach (TokenController t in tokenControllers)
        {
            Destroy(t.gameObject);
        }
        tokenControllers.Clear();
    }

    public void BuildTokenControllersFrom(TokenController tc, Vector3 pos)
    {
        int index = tokenControllers.IndexOf(tc);
        DestroyTokenControllers();

        foreach (Token t in expression.tokens)
        {
            GameObject tokenObject = Instantiate(tokenPrefab, transform.position, Quaternion.identity, transform);
            TokenController controller = tokenObject.GetComponent<TokenController>();
            controller.token = t;
            controller.expressionController = this;
            tokenControllers.Add(controller);
        }

        float tokenCenter = pos.x;
        float thisTokenExtent;
        float previousTokenExtent = 0;

        for (int i = index - 1; i >= 0; i--)
        {
            thisTokenExtent = tokenControllers[i].gfxBounds.extents.x + spacing;
            tokenCenter -= thisTokenExtent + previousTokenExtent;
            Vector3 position = new(
                pos.x + tokenCenter,
                pos.y,
                pos.z);
            tokenControllers[i].gameObject.transform.position = position;
            previousTokenExtent = tokenControllers[i].gfxBounds.extents.x + spacing;
        }
    }

    public void SwapTokenControllers(TokenController tc1, TokenController tc2)
    {
        expression.SwapTokens(tc1.token, tc2.token);
        int ind1 = tokenControllers.IndexOf(tc1);
        int ind2 = tokenControllers.IndexOf(tc2);
        Debug.Log("Swapping " + ind1 + " and " + ind2);
        tokenControllers[ind1] = tc2;
        tokenControllers[ind2] = tc1;
    }

    public void AddParentheses()
    {
        expression.AddParentheses();
        foreach(Token t in expression.tokens)
        {
            if(t is CustomToken)
            {
                GameObject tokenObject = Instantiate(tokenPrefab, transform.position, Quaternion.identity, transform);
                TokenController controller = tokenObject.GetComponent<TokenController>();
                controller.token = t;
                controller.expressionController = this;
                tokenControllers.Insert(expression.tokens.IndexOf(t), controller);
            }
        }
    }

    public void RemoveParentheses()
    {
        List<TokenController> toRemove = new();
        foreach (TokenController t in tokenControllers)
        {
            if (t.token is CustomToken)
            {
                toRemove.Add(t);
            }
        }
        foreach(TokenController t in toRemove)
        {
            tokenControllers.Remove(t);
            Destroy(t.gameObject);
        }
        expression.RemoveParentheses();
    }

    #region Unity
    void Awake()
    {
        expression = new Expression(this);
        tokenControllers = new();
    }
    void Start()
    {
        BuildTokenControllers();
    }
    void Update()
    {
        PositionTokenControllers();
    }
    #endregion

    protected List<Token> ParseExpression()
    {
        List<string> tokenStrings = new();
        string bufferString = "";

        for (int i = 0; i < expressionString.Length; i++)
        {
            if (expressionString[i].Equals(' '))
            {
                tokenStrings.Add(bufferString);
                bufferString = "";
            }
            else
            {
                bufferString += expressionString[i];
            }
        }

        tokenStrings.Add(bufferString);

        List<Token> startTokens = new();

        foreach (string s in tokenStrings)
        {
            if (s.Equals("+"))
            {
                startTokens.Add(new Addition(expression));
            }
            else if (s.Equals("*"))
            {
                startTokens.Add(new Multiplication(expression));
            }
            else if (Regex.IsMatch(s, @"^[a-zA-Z]+$")) // If it's alphabet only (variable)
            {
                startTokens.Add(new Term(expression, s));
            }
            else if (Regex.IsMatch(s, @"^[0-9]+$"))
            {
                startTokens.Add(new Term(expression, float.Parse(s)));
            }
            else
            {
                float coeff = float.Parse(Regex.Replace(s, @"[^0-9]", ""));
                string var = Regex.Replace(s, @"[0-9]", "");
                startTokens.Add(new Term(expression, coeff, var));
            }
        }

        return startTokens;

    }

}