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
        // EMPTY
        if (Input.GetKey(KeyCode.LeftControl) & Input.GetMouseButtonDown(0))
        {

        }

        // Ctrl + Right Mouse Button
        // EMPTY
        if (Input.GetKey(KeyCode.LeftControl) & Input.GetMouseButtonDown(1))
        {

        }

        // Middle Mouse Button
        // Expand and simplify (only one should be possible)
        if (Input.GetMouseButtonDown(2))
        {
            Simplify(tokenController);
            Expand(tokenController);
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
