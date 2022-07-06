using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Expression
{
    public List<Token> tokens;
    public ExpressionController controller;

    protected bool parenthetized = false;

    public Expression(ExpressionController c)
    {
        controller = c;
        Test5();
    }

    public Expression(ExpressionController c, List<Token> t)
    {
        controller = c;
        tokens = t;
    }

    public List<List<int>> LawfulPlacements(Token t)
    {
        if (!parenthetized)
        {
            AddParentheses();
        }

        List<List<int>> ranges = new();

        int startIndex = tokens.IndexOf(t);

        ranges.Add(new List<int>());

        // If the token is enclosed in parentheses
        if (IsEnclosed(t))
        {
            //Start from the token, iterate left until you find starting parenthesis
            for (int i = tokens.IndexOf(t); i >= 0; i--)
            {
                if (tokens[i].symbolString.Equals("("))
                {
                    // Iterate right until you find close parenthesis, adding each index to the ranges list
                    for(int j = i + 1; j < tokens.Count; j++)
                    {
                        if (tokens[j].symbolString.Equals(")"))
                        {
                            break;
                        }
                        ranges[0].Add(j);
                    }
                    break;
                }
            }
            return ranges;
        }

        bool allowed = true;

        // Iterate left from start
        for (int i = startIndex; i >= 0; i--)
        {

            if(tokens[i] is CustomToken)
            {
                allowed = !allowed;
            }

            if (allowed & !(tokens[i] is CustomToken))
            {
                ranges[0].Add(i);
            }
        }

        allowed = true;

        // Iterate right from start
        for (int i = startIndex; i < tokens.Count; i++)
        {

            if(tokens[i] is CustomToken)
            {
                allowed = !allowed;
            }

            if (allowed & !(tokens[i] is CustomToken))
            {
                ranges[0].Add(i);
            }
        }

        return ranges;
    }

    private bool IsEnclosed(Token t)
    {
        for(int i = tokens.IndexOf(t); i >= 0; i--)
        {
            if (tokens[i].symbolString.Equals("("))
            {
                return true;
            }
            if (tokens[i].symbolString.Equals(")"))
            {
                return false;
            }
        }
        return false;
    }


    public void AddParentheses()
    {
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

            this.tokens.Insert(first + tokensAdded, new CustomToken(this, "("));
            tokensAdded++;
            this.tokens.Insert(last + tokensAdded, new CustomToken(this, ")"));
            tokensAdded++;
        }

        parenthetized = true;
    }

    public void RemoveParentheses()
    {
        List<int> removeList = new();

        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i] is CustomToken)
            {
                removeList.Add(i);
            }
        }

        int tokensRemoved = 0;

        foreach (int i in removeList)
        {
            tokens.RemoveAt(i - tokensRemoved);
            tokensRemoved++;
        }
    }

    public void SwapTokens(Token token1, Token token2)
    {
        if(!(token1 is Term & token2 is Term))
        {
            return;
        }

        int index1 = tokens.IndexOf(token1);
        int index2 = tokens.IndexOf(token2);

        tokens[index1] = token2;
        tokens[index2] = token1;
    }

    public void MoveToken(Token token, Token target)
    {
        if (!(token is Term & target is Operator))
        {
            return;
        }

        int tokenIndex = tokens.IndexOf(token);
        int targetIndex = tokens.IndexOf(target);

        Token operatorToMove;

        if (tokenIndex == 0)
        {
            operatorToMove = null;
        }
        else
        {
            operatorToMove = tokens[tokenIndex - 1];
        }
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

    // Test Expression 4
    // 3 + 4 + x
    protected void Test4()
    {
        tokens = new List<Token>
        {
            new Term(this, 3),
            new Addition(this),
            new Term(this, 4),
            new Addition(this),
            new Term(this, "x")
        };
    }

    // Test Expression 5
    // 3 * 5 + 2 * 3 * 2 + 5x * 9
    protected void Test5()
    {
        tokens = new List<Token>
        {
            new Term(this, 3),
            new Multiplication(this),
            new Term(this, 5),
            new Addition(this),
            new Term(this, 2),
            new Multiplication(this),
            new Term(this, 3),
            new Multiplication(this),
            new Term(this, 2),
            new Addition(this),
            new Term(this, 5, "x"),
            new Multiplication(this),
            new Term(this, 9)
        };
    }
}