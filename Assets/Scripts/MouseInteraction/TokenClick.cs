using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenClick : MonoBehaviour
{
    private TokenController tokenController;
    private Camera mainCamera;
    private RaycastHit raycastHit;


    void Start()
    {
        mainCamera = (Camera)FindObjectOfType(typeof(Camera));
        tokenController = gameObject.GetComponent<TokenController>();
    }

    // Update is called once per frame
    void Update()
    {
        OnCtrlLeftClick();
        OnCtrlRightClick();
    }

    private void OnCtrlLeftClick()
    {
        if (Input.GetKey(KeyCode.LeftControl) & Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.gameObject == this.gameObject)
                {
                    Debug.Log("Expanding " + gameObject.name);
                    tokenController.Expand();
                }
            }
        }
    }

    private void OnCtrlRightClick()
    {
        if (Input.GetKey(KeyCode.LeftControl) & Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.gameObject == gameObject)
                {
                    Debug.Log("Simplifying " + gameObject.name);
                    tokenController.Simplify();
                }
            }
        }
    }
}
