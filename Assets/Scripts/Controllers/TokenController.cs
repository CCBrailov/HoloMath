using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenController : MonoBehaviour
{
    #region Control
    public Token token;
    public ExpressionController expressionController;
    public BoxCollider boxCollider;
    public CapsuleCollider leftHandleCollider;
    public CapsuleCollider rightHandleCollider;

    public enum gfxTypes { FlatText, SolidText }
    public gfxTypes gfxType;

    public void Expand()
    {
        token.Expand();
        expressionController.BuildTokenControllers();
        Destroy(gameObject);
    }
    public void Simplify()
    {
        token.Simplify();
        expressionController.BuildTokenControllers();
    }
    protected void ResizeColliders()
    {
        boxCollider.size = gfxBounds.size + new Vector3(0, 0, 0.02f);
    }
    #endregion

    #region Unity
    void Awake()
    {
        
    }
    void Start()
    {
        RefreshGFXBounds();
        Draw();
    }
    void Update()
    {
        RefreshGFXBounds();
        Draw();
        ResizeColliders();
        gameObject.name = token.symbolString;
    }
    #endregion

    #region Graphics
    public TextMeshPro textMesh;
    public Bounds gfxBounds;
    public bool visible = true;
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
            textMesh.SetText(token.symbolString);
        }
        else
        {
            textMesh.SetText("_");
        }
    }
    protected void RefreshGFXBounds()
    {
        gfxBounds = textMesh.bounds;
    }
    #endregion
}