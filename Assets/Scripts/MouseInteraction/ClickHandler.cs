using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public Camera mainCamera;
    private RaycastHit raycastHit;

    private bool holdingToken = false;
    private bool hovering = false;

    private ExpressionController sourceExpressionController = null;
    private TokenController sourceTokenController = null;
    private Token sourceToken = null;
    private TokenController heldTokenController = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get whatever Token Controller the mouse is hovering on
        TokenController mouseTokenController = GetTokenFromMouse();
        hovering = mouseTokenController != null;

        // Middle Mouse Button
        // Expand and simplify (only one should be possible)
        if (Input.GetMouseButtonDown(2))
        {
            if (mouseTokenController.token is Token)
            {
                Simplify(mouseTokenController);
                Expand(mouseTokenController);
            }
            else
            {
                return;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(!hovering)
            {
                return;
            }

            // Only do if selected Token is a Term
            if (mouseTokenController.token is Term)
            {
                sourceToken = mouseTokenController.token; // Get the Token for selected TokenController

                sourceExpressionController = mouseTokenController.expressionController; // Get the ExpressionController for selected TokenController
                int index = sourceExpressionController.expression.tokens.IndexOf(sourceToken); // Get the index of the token in its Expression

                //sourceExpressionController.AddParenthesesFromIndex(index); // Add parenthesis Tokens to Expression, starting from selected Token
                //Debug.Log("Adding parentheses from position " + index);

                sourceExpressionController.expression.AddParentheses();

                sourceExpressionController.BuildTokenControllers(); // Rebuild TokenControllers (NOTE: This destroys the original selected TokenController)

                index = sourceExpressionController.expression.tokens.IndexOf(sourceToken);         // Retrieve new version of original selected TokenController by
                sourceTokenController = sourceExpressionController.tokenControllers[index];        //       referencing new index of the Token in Expression (after adding parentheses)

                heldTokenController = Instantiate(                                                 // Instantiate heldTokenController...
                    sourceTokenController,                                                         //       ...as a copy of the original selected TokenController,
                    sourceTokenController.transform.position,                                      //          at the position of the original,
                    Quaternion.identity,                                                           //          with the rotation of the original,
                    sourceTokenController.transform.parent);                                       //          as a child of the same GameObject as the ExpressionController.
                heldTokenController.token = sourceTokenController.token;
                heldTokenController.boxCollider.enabled = false;

                sourceTokenController.Hide();                                                      // Hide the original TokenController
                holdingToken = true;
            }
            else
            {
                return;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (holdingToken)
            {
                heldTokenController.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1);
                List<List<int>> lawfulRegions = sourceExpressionController.expression.LawfulPlacements(sourceToken);
                foreach(List<int> region in lawfulRegions)
                {
                    foreach(int i in region)
                    {
                        //Debug.Log(i);
                        TokenController tc = sourceExpressionController.tokenControllers[i];
                        Color color = new(100, 0, 0);
                        if(tc == mouseTokenController)
                        {
                            color.a = 0.4f;
                        }
                        sourceExpressionController.tokenControllers[i].textMesh.color = color;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (holdingToken)
            {
                if (mouseTokenController is TokenController)
                {
                    sourceExpressionController.expression.SwapTokens(sourceToken, mouseTokenController.token);
                }
                sourceExpressionController.expression.RemoveParentheses(); // Remove added parentheses from equation
                sourceExpressionController.BuildTokenControllers(); // Rebuild controllers (also reveals hidden tokens)
                Destroy(heldTokenController.gameObject); // Destroy the dragged token
                ClearDragVars(); 
            }
        }

    }

    private TokenController GetTokenFromMouse()
    {
        TokenController token = null;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            token = raycastHit.collider.gameObject.GetComponent<TokenController>();
            Debug.Log(raycastHit.collider.ToString());
        }
        return token;
    }

    private void Simplify(TokenController tc)
    {
        Debug.Log("Simplifying " + tc.gameObject.name);
        tc.Simplify();
    }

    private void Expand(TokenController tc)
    {
        Debug.Log("Expanding " + tc.gameObject.name);
        tc.Expand();
    }
  
    private void ClearDragVars()
    {
        holdingToken = false;
        sourceExpressionController = null;
        sourceTokenController = null;
        sourceToken = null;
        heldTokenController = null;
    }
}
