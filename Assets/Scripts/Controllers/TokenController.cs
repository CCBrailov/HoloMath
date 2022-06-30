using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenController : MonoBehaviour
{
    public Token token;
    public TextMeshPro textMesh;
    public ExpressionController expressionController;
    public SphereCollider sphereCollider;

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

    void Start()
    {
        Collider collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.SetText(token.displayString);
        sphereCollider.radius = textMesh.bounds.extents.x;
        gameObject.name = token.displayString;
    }
}