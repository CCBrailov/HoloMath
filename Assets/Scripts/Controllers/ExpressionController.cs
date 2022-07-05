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

    protected void PositionTokenControllers(int center)
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
        foreach (TokenController t in tokenControllers)
        {
            Destroy(t.gameObject);
        }
        tokenControllers.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        //expression = new Expression(this, ParseExpression());
        expression = new Expression(this);
        tokenControllers = new List<TokenController>();
        BuildTokenControllers();
    }

    // Update is called once per frame
    void Update()
    {
        PositionTokenControllers();
    }

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