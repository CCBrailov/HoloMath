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
    protected GameObject originalElement;

    protected InputDevice? device;

    protected bool gripping = false;

    void Awake()
    {
        hoveredElements = new();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hovering = hoveredElements.Count != 0;

        if (device == null)
        {
            Debug.Log("Fetching Controller");
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

        bool held = heldElement != null;

        ColorHoveredElements();
        if (hovering)
        {
            OnGrip();
        }

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
        tc.textMesh.outlineWidth = 0;
        tc.textMesh.color = new Color(255, 255, 255);
        hoveredElements.Remove(tc);
    }
    void ColorHoveredElements()
    {
        for (int i = 0; i < hoveredElements.Count; i++)
        {
            if (i == 0)
            {
                hoveredElements[i].textMesh.color = new(0, 100, 0);
                hoveredElements[i].textMesh.outlineWidth = 0.1f;
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
                originalElement = hoveredElements[0].gameObject;
                heldElement = Instantiate(expressionController.tokenPrefab, gameObject.transform.position, hoveredElements[0].transform.rotation, gameObject.transform);
                TokenController heldController = heldElement.GetComponent<TokenController>();
                heldController.token = hoveredElements[0].token;
                heldElement.transform.localScale = new(20, 20, 20);
                heldElement.GetComponent<BoxCollider>().enabled = false;
                heldController.textMesh.color = new(255, 255, 255);
                holdingElement = true;
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
                        expressionController.expression.SwapTokens(originalController.token, hoveredElements[0].token);
                        int ind1 = expressionController.tokenControllers.IndexOf(originalController);
                        int ind2 = expressionController.tokenControllers.IndexOf(hoveredElements[0]);
                        TokenController tc1 = originalController;
                        TokenController tc2 = hoveredElements[0];
                        expressionController.tokenControllers[ind1] = tc2;
                        expressionController.tokenControllers[ind2] = tc1;
                    }
                    Destroy(heldElement);
                    holdingElement = false;
                }
            }
        }
    }
}
