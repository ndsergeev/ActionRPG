using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class InputDeviceHandler : MonoBehaviour
{
    
    // Player Input
    PlayerInput playerInput;

    public enum Devices
    {
        none,
        keyboard,
        gamepad,
    }

    public Devices device;

    string currentDevice;

    // Start is called before the first frame update
    void Start()
    {
        SetUpDeviceChangeDetection();
    }

    void SetUpDeviceChangeDetection()
    {
        InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;

                //Debug.Log($"device: {lastDevice.displayName}");
                WorkOutScheme(lastDevice.displayName);
            }
        };
    }

    void WorkOutScheme(string displayName)
    {
        Devices tempDevice = Devices.none;

        if (displayName == "Mouse" || displayName == "Keyboard")
        {
            tempDevice = Devices.keyboard;
        }
        else if (displayName == "Wireless Controller" || displayName == "Xbox Controller")
        {
            tempDevice = Devices.gamepad;
        }
        else
        {
            tempDevice = Devices.gamepad;
        }


        if (tempDevice == device)
        {
            // Device hasn't changed
        }
        else
        {
            // Device has changed
            device = tempDevice;
            InputManager.Singleton.SetCurrentDevice(device.ToString());
            PlayerEvents.TriggerInputDeviceChangeEvent();
        }
    }
}
