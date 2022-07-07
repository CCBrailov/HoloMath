using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RightHand : MonoBehaviour
{
    public ExpressionController expressionController;

    protected bool hovering = false;
    protected List<TokenController> hoveredElements;

    protected bool holdingElement = false;
    protected GameObject heldElement;
    protected bool selectedElement = false;
    protected GameObject originalElement;

    protected InputDevice? device;

    protected LeftHand leftHand;

    public bool gripping = false;

    protected Vector3? pullStart = null;
    public float pullDistance = 0;

    void Awake()
    {
        hoveredElements = new();
    }
    // Start is called before the first frame update
    void Start()
    {
        leftHand = FindObjectsOfType<LeftHand>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        hovering = hoveredElements.Count != 0;

        if (device == null)
        {
            Debug.Log("Fetching Right Controller");
            List<InputDevice> devices = new();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, devices);
            if(devices.Count != 0)
            {
                device = devices[0];
            }
            return;
        }

        if (holdingElement)
        {
            heldElement.transform.position = gameObject.transform.position;
        }

        ColorHoveredElements();

        OnGrip();
        OnHold();
        OnRelease();
    }
    private void OnCollisionEnter(Collision collision)
    {
        TokenController tc = collision.gameObject.GetComponent<TokenController>();
        hoveredElements.Insert(0, tc);
    }
    void OnCollisionExit(Collision collision)
    {
        TokenController tc = collision.gameObject.GetComponent<TokenController>();
        //tc.textMesh.outlineWidth = 0;
        //tc.textMesh.color = new Color(255, 255, 255);
        hoveredElements.Remove(tc);
    }
    void ColorHoveredElements()
    {
        foreach(TokenController t in expressionController.tokenControllers)
        {
            t.textMesh.outlineWidth = 0;
            t.textMesh.color = new(255, 255, 255);
        }
        for (int i = 0; i < hoveredElements.Count; i++)
        {
            if (i == 0 && leftHand.hovering && hoveredElements[i] == leftHand.hoveredElements[0])
            {
                hoveredElements[i].textMesh.outlineWidth = 0.1f;
            }

            if(i == 0)
            {
                hoveredElements[i].textMesh.color = new(0, 100, 0);
            }
            else
            {
                hoveredElements[i].textMesh.color = new(0, 0, 100);
            }
        }
    }
    void OnGrip()
    {
        device.Value.TryGetFeatureValue(CommonUsages.gripButton, out bool GripDown);
        if (GripDown)
        {
            if (!gripping)
            {
                gripping = true;
                if (hovering)
                {
                    originalElement = hoveredElements[0].gameObject;
                    pullStart = originalElement.transform.position;

                    //heldElement = Instantiate(expressionController.tokenPrefab, gameObject.transform.position, hoveredElements[0].transform.rotation, gameObject.transform);
                    //TokenController heldController = heldElement.GetComponent<TokenController>();
                    //heldController.token = hoveredElements[0].token;
                    //heldElement.transform.localScale = new(20, 20, 20);
                    //heldElement.GetComponent<BoxCollider>().enabled = false;
                    //heldController.textMesh.color = new(255, 255, 255);
                    //expressionController.AddParentheses();
                    //holdingElement = true;
                }
            }
        }
    }

    void OnHold()
    {
        if (gripping)
        {
            if (pullDistance > 0.15f && !holdingElement)
            {
                heldElement = Instantiate(expressionController.tokenPrefab, gameObject.transform.position, originalElement.transform.rotation, gameObject.transform);
                TokenController heldController = heldElement.GetComponent<TokenController>();
                heldController.token = originalElement.GetComponent<TokenController>().token;
                heldElement.transform.localScale = new(20, 20, 20);
                heldElement.GetComponent<BoxCollider>().enabled = false;
                heldController.textMesh.color = new(255, 255, 255);
                expressionController.AddParentheses();
                holdingElement = true;
            }
            if (pullStart != null)
            {
                pullDistance = Vector3.Distance(pullStart.Value, gameObject.transform.position);
            }
        }
    }

    void OnRelease()
    {
        device.Value.TryGetFeatureValue(CommonUsages.gripButton, out bool GripDown);
        if (!GripDown)
        {
            if (gripping)
            {
                gripping = false;
                if (holdingElement)
                {
                    TokenController originalController = originalElement.GetComponent<TokenController>();
                    TokenController heldController = heldElement.GetComponent<TokenController>();
                    if (hovering && heldController.token is Term && hoveredElements[0].token is Term)
                    {
                        expressionController.SwapTokenControllers(originalController, hoveredElements[0]);
                    }
                    expressionController.RemoveParentheses();
                    Destroy(heldElement);
                    holdingElement = false;
                }
                pullDistance = 0;
                pullStart = null;
            }
        }
    }
}
