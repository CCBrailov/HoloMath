using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpressionController : MonoBehaviour
{
    public Expression expression;
    public List<TokenController> tokenControllers;
    public float spacing = 0.3f;

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

        foreach(TokenController t in tokenControllers)
        {
            expressionWidth += t.textMesh.bounds.size.x + spacing * 2;
        }

        float tokenCenter = 0 - (expressionWidth / 2);
        float thisTokenExtent;
        float previousTokenExtent = 0;

        foreach(TokenController t in tokenControllers)
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