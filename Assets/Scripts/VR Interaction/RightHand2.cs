using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class RightHand2 : MonoBehaviour
{
    protected bool deviceLoaded;
    protected InputDevice device;

    public bool hovering;
    public List<TokenController> hoveredElements;

    public bool GripDown;

    protected bool gripping = false;
    public UnityEvent squeeze;
    public UnityEvent release;

    protected bool triggerState = false;
    public UnityEvent trigger;
    public UnityEvent triggerUp;

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
            Debug.Log("Fetching Right Controller");
            List<InputDevice> devices = new();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, devices);
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

        device.TryGetFeatureValue(CommonUsages.triggerButton, out bool TriggerDown);
        if(TriggerDown && !triggerState)
        {
            triggerState = true;
            trigger.Invoke();
        }
        if(!TriggerDown && triggerState)
        {
            triggerState = false;
            triggerUp.Invoke();
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
