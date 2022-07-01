using System.Collections.Generic;
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

        //foreach (TokenController t in tokenControllers)
        //{
        //    Vector3 position = new(
        //        gameObject.transform.position.x + (tokenControllers.IndexOf(t) * 3),
        //        gameObject.transform.position.y,
        //        gameObject.transform.position.z);
        //    t.gameObject.transform.position = position;
        //}
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
        expression = new Expression(this);
        tokenControllers = new List<TokenController>();

        List<string> tokenStrings = new();
        string bufferString = "";

        for(int i = 0; i < expressionString.Length; i++)
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

        foreach(string s in tokenStrings)
        {
            // TODO: PARSE STRINGS TO TOKENS
        }


        BuildTokenControllers();
    }

    // Update is called once per frame
    void Update()
    {
        PositionTokenControllers();
    }

    //
    //
    //
    //
    //
    //
    //
    //
    // DEPRICATED METHODS (which I am too scared to delete for the moment)

    public void AddParentheses()
    {
        List<Token> tokens = expression.tokens;
        List<List<int>> ranges = new();

        // Iterate through tokens
        for (int i = 0; i < tokens.Count; i++)
        {
            // If Token is Multiplication
            if (tokens[i] is Multiplication)
            {
                // Start a new range list, add the previous index (token before the multiplication)
                List<int> range = new();
                range.Add(i - 1);

                // Iterate until no more Multiplication
                for (int j = i; j < tokens.Count; j++)
                {
                    if (tokens[j] is Operator & !(tokens[j] is Multiplication))
                    {
                        i = j;
                        break;
                    }
                    range.Add(j);
                    if (j == tokens.Count - 1)
                    {
                        i = j;
                        break;
                    }
                }

                // Add the range to the list of ranges
                ranges.Add(range);
            }
        }

        int tokensAdded = 0;

        foreach (List<int> r in ranges)
        {
            int first = r[0];
            int last = r[^1] + 1;

            expression.tokens.Insert(first + tokensAdded, new CustomToken(expression, "("));
            tokensAdded++;
            expression.tokens.Insert(last + tokensAdded, new CustomToken(expression, ")"));
            tokensAdded++;
        }
    }

    public void RemoveParentheses()
    {
        List<int> removeList = new();

        for (int i = 0; i < expression.tokens.Count; i++)
        {
            if (expression.tokens[i] is CustomToken)
            {
                removeList.Add(i);
            }
        }

        int tokensRemoved = 0;

        foreach (int i in removeList)
        {
            expression.tokens.RemoveAt(i - tokensRemoved);
            tokensRemoved++;
        }
    }

    public void AddParenthesesFromIndex(int index)
    {
        List<Token> tokens = expression.tokens;
        List<Token> neighbors = new();

        // Get the one or two neighboring tokens
        if (index == 0)
        {
            neighbors.Add(tokens[1]);
        }
        else if (index == tokens.Count - 1)
        {
            neighbors.Add(tokens[tokens.Count - 2]);
        }
        else
        {
            neighbors.Add(tokens[index - 1]);
            neighbors.Add(tokens[index + 1]);
        }

        // If none of the neighboring tokens are Multiplication, exit
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] is Multiplication)
            {
                break;
            }
            if (i == neighbors.Count - 1)
            {
                return;
            }
        }

        int open = 0;
        int close = tokens.Count + 1; // Count + 1, not Count - 1, because 

        // Iterate left from index until reaching non-Multiplication Operator or start of expression, save index for OpenParenthesis
        for (int i = index; i >= 0; i--)
        {
            if (tokens[i] is Operator & !(tokens[i] is Multiplication))
            {
                open = i + 1;
                break;
            }
        }

        index++;

        // Iterate right from index until reaching non-Multiplication Operator or end of expression, save index for CloseParenthesis
        for (int i = index; i < tokens.Count; i++)
        {
            if (tokens[i] is Operator & !(tokens[i] is Multiplication))
            {
                close = i + 1;
                break;
            }
        }

        tokens.Insert(open, new CustomToken(expression, "("));
        tokens.Insert(close, new CustomToken(expression, ")"));
    }
}