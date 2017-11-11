using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDirectional : MonoBehaviour {

    public int playerID = 0;
    private PlayerDeviceManager deviceManager;
    private float deadZone = 0.2f;
//    private float horizontalAxis;
//    private float verticalAxis;
    private PlayerActions controls;

    // Use this for initialization
    void Start() {
        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
        if (deviceManager != null) {
            controls = deviceManager.GetControls(playerID);
        }
    }

    public float GetCurrentHorzAxis() {
        if (controls != null && Mathf.Abs(controls.Move.X) > deadZone) {
            return controls.Move.X;
        }
        return 0.0f;
    }

    public float GetCurrentVertAxis() {
        if (controls != null && Mathf.Abs(controls.Move.Y) > deadZone) {
            return controls.Move.Y;
        }
        return 0.0f;
    }
}
