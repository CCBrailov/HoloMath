using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public Camera mainCamera;
    private RaycastHit raycastHit;
    private TokenController heldTokenController;

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

        // Ctrl + Left Mouse Button
        if (Input.GetKey(KeyCode.LeftControl) & Input.GetMouseButtonDown(0))
        {
            OnCtrlLeftClick(tokenController);
            return;
        }

        // Ctrl + Right Mouse Button
        if (Input.GetKey(KeyCode.LeftControl) & Input.GetMouseButtonDown(1))
        {
            OnCtrlRightClick(tokenController);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClick(tokenController);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            OnLeftDrag(heldTokenController);
            return;
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

    private void OnCtrlLeftClick(TokenController tc)
    {
        if (Input.GetKey(KeyCode.LeftControl) & Input.GetMouseButtonDown(0))
        {
            Debug.Log("Simplifying " + tc.gameObject.name);
            tc.Simplify();
        }
    }

    private void OnCtrlRightClick(TokenController tc)
    {
        Debug.Log("Expanding " + tc.gameObject.name);
        tc.Expand();
    }

    private void OnLeftClick(TokenController tc)
    {
        //tc.expressionController.tokenControllers.Remove(tc);
        heldTokenController = tc;
    }

    private void OnLeftDrag(TokenController tc)
    {
        tc.transform.position = Input.mousePosition;
    }

  
}
