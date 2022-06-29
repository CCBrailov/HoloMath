using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpressionController : MonoBehaviour
{
    public Expression expression;
    public List<TokenController> tokenControllers;

    public GameObject tokenObject;

    public void BuildTokenControllers()
    {
        DestroyTokenControllers();
        foreach (Token t in expression.tokens)
        {
            Vector3 position = new(expression.tokens.IndexOf(t) * 3, 0, 0);
            GameObject tokenView = Instantiate(tokenObject, position, Quaternion.identity, this.transform);
            TokenController controller = (TokenController)tokenView.GetComponent("TokenViewController");
            controller.token = t;
            controller.expressionController = this;
            tokenControllers.Add(controller);
        }
    }

    protected void PositionTokenControllers()
    {
        foreach (TokenController t in tokenControllers)
        {
            Vector3 position = new(tokenControllers.IndexOf(t) * 3, 0, 0);
            t.gameObject.transform.position = position;
        }
    }

    protected void DestroyTokenControllers()
    {
        foreach(TokenController t in tokenControllers)
        {
            Destroy(t.gameObject);
        }
        tokenControllers.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        expression = new Expression();
        tokenControllers = new List<TokenController>();
        BuildTokenControllers();
    }

    // Update is called once per frame
    void Update()
    {
        PositionTokenControllers();
    }
}