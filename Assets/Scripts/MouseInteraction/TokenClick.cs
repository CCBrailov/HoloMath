using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenClick : MonoBehaviour
{
    public TokenController tokenController;

    private Camera mainCamera;
    private RaycastHit raycastHit;


    void Start()
    {
        mainCamera = (Camera)FindObjectOfType(typeof(Camera));
        //tokenController = (TokenController)gameObject.GetComponent(typeof(TokenController));
        GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
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

        if (Input.GetMouseButtonDown(0))
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
