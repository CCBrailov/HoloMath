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
    public CapsuleCollider leftHandleCollider;
    public CapsuleCollider rightHandleCollider;

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
        //token.Hide();
        visible = false;
    }

    public void Show()
    {
        //token.Show();
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
        Draw();
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
        boxCollider.size = new Vector3(textMesh.bounds.size.x, textMesh.bounds.size.y, 1);
        leftHandleCollider.center = new(-textMesh.bounds.extents.x, 0, 0);
        rightHandleCollider.center = new(textMesh.bounds.extents.x, 0, 0);
        leftHandleCollider.height = textMesh.bounds.size.y;
        rightHandleCollider.height = textMesh.bounds.size.y;
        gameObject.name = token.displayString;
    }
}