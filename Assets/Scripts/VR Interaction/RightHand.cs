using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour
{
    protected ExpressionController expressionController;
    protected List<TokenController> tokenControllers;

    void Awake()
    {
        tokenControllers = new();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < tokenControllers.Count; i++)
        {
            if(i == 0)
            {
                tokenControllers[i].textMesh.color = new(0, 100, 0);
            }
            else
            {
                tokenControllers[i].textMesh.color = new(0, 0, 100);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        TokenController tc = collision.gameObject.GetComponent<TokenController>();
        tc.textMesh.color = new Color(100, 0, 0);
        tokenControllers.Insert(0, tc);
    }
    void OnCollisionExit(Collision collision)
    {
        TokenController tc = collision.gameObject.GetComponent<TokenController>();
        tc.textMesh.color = new Color(255, 255, 255);
        tokenControllers.Remove(tc);
    }
}
