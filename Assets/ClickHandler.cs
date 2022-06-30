using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public Camera mainCamera;
    private RaycastHit raycastHit;
    private TokenController originalTokenController = null;
    private TokenController heldTokenController = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get whatever Token Controller the mouse is hovering on, if null, immediately exit
        TokenController tokenController = GetTokenFromMouse();
        if (tokenController == null)
        {
            return;
        }

        // Middle Mouse Button
        // Expand and simplify (only one should be possible)
        if (Input.GetMouseButtonDown(2))
        {
            Simplify(tokenController);
            Expand(tokenController);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(!(tokenController.token is Term))
            {
                return;
            }
            originalTokenController = tokenController;
            heldTokenController = Instantiate(tokenController, tokenController.transform.position, Quaternion.identity, tokenController.transform.parent);
            heldTokenController.token = tokenController.token;
            originalTokenController.Hide();
            originalTokenController.expressionController.AddParentheses();
            originalTokenController.expressionController.BuildTokenControllers();

        }

        if (Input.GetMouseButton(0))
        {
            if (heldTokenController != null)
            {
                heldTokenController.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1);
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            originalTokenController.expressionController.RemoveParentheses();
            originalTokenController.Show();
            Destroy(heldTokenController.gameObject);
            heldTokenController = null;
            originalTokenController = null;
        }

    }

    private TokenController GetTokenFromMouse()
    {
        TokenController token = null;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            token = raycastHit.collider.gameObject.GetComponent<TokenController>();
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
  
}
