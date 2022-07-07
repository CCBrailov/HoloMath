using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeftHand : MonoBehaviour
{
    public ExpressionController expressionController;

    public bool hovering = false;
    public List<TokenController> hoveredElements;

    public InputDevice? device;

    private void OnCollisionEnter(Collision collision)
    {
        TokenController tc = collision.gameObject.GetComponent<TokenController>();
        hoveredElements.Insert(0, tc);
    }
    void OnCollisionExit(Collision collision)
    {
        TokenController tc = collision.gameObject.GetComponent<TokenController>();
        hoveredElements.Remove(tc);
    }

    void Awake()
    {
        hoveredElements = new();
    }
    void Start()
    {
        
    }
    void Update()
    {
        hovering = hoveredElements.Count != 0;

        if (device == null)
        {
            Debug.Log("Fetching Left Controller");
            List<InputDevice> devices = new();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, devices);
            if (devices.Count != 0)
            {
                device = devices[0];
            }
            return;
        }
    }
}
