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
        boxCollider.size = gfxBounds.size + new Vector3(0, 0, 1);
        leftHandleCollider.center = new(-gfxBounds.extents.x, 0, 0);
        rightHandleCollider.center = new(gfxBounds.extents.x, 0, 0);
        leftHandleCollider.height = gfxBounds.size.y;
        rightHandleCollider.height = gfxBounds.size.y;
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
        gameObject.name = token.displayString;
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
            textMesh.SetText(token.displayString);
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