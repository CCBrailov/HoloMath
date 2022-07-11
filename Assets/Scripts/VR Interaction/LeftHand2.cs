using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class LeftHand2 : MonoBehaviour
{
    protected bool deviceLoaded;
    public InputDevice device;

    public bool hovering;
    public List<TokenController> hoveredElements;

    public bool GripDown;

    protected bool gripping;
    public UnityEvent squeeze;
    public UnityEvent release;

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
        if (!deviceLoaded)
        {
            Debug.Log("Fetching Left Controller");
            List<InputDevice> devices = new();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, devices);
            if (devices.Count != 0)
            {
                device = devices[0];
                deviceLoaded = true;
            }
            return;
        }
        device.TryGetFeatureValue(CommonUsages.gripButton, out GripDown);
        if (GripDown && !gripping)
        {
            gripping = true;
            squeeze.Invoke();
        }
        if (!GripDown && gripping)
        {
            gripping = false;
            release.Invoke();
        }
    }

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
}
