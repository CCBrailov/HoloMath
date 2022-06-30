using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenController : MonoBehaviour
{
    public Token token;
    public TextMeshPro textMesh;
    public ExpressionController expressionController;
    public BoxCollider boxCollider;

    public bool visible = true;

    [ContextMenu("Expand")]
    public void Expand()
    {
        token.Expand();
        expressionController.BuildTokenControllers();
        Destroy(gameObject);
    }

    [ContextMenu("Simplify")]
    public void Simplify()
    {
        token.Simplify();
        expressionController.BuildTokenControllers();
    }

    public void Hide()
    {
        visible = false;
    }

    public void Show()
    {
        visible = true;
    }

    public void Draw()
    {
        if (visible)
        {
            textMesh.SetText(token.displayString);
        }
        else
        {
            textMesh.SetText("_");
        }

    }

    void Start()
    {
        Show();
        Draw();
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
        boxCollider.size = new Vector3(textMesh.bounds.size.x, textMesh.bounds.size.y, 1);
        gameObject.name = token.displayString;
    }
}