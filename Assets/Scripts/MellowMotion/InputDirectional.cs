using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDirectional : MonoBehaviour {

    private int playerID = 0;
    private PlayerDeviceManager deviceManager;
    private float deadZone = 0.2f;
    private float horizontalAxis;
    private float verticalAxis;

    // Use this for initialization
    void Start() {
        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
    }

    public float GetCurrentHorzAxis() {
        return horizontalAxis;
    }

    public float GetCurrentVertAxis() {
        return verticalAxis;
    }

    void Update() {
        //Find the controls bound to this player
        if (deviceManager != null) {
            PlayerActions controls = deviceManager.GetControls(playerID);
            if (controls != null) {
                horizontalAxis = Mathf.Abs(controls.Move.X) > deadZone ? controls.Move.X : 0.0f;
                verticalAxis = Mathf.Abs(controls.Move.Y) > deadZone ? controls.Move.Y : 0.0f;
            }
            else {
                horizontalAxis = 0.0f;
                verticalAxis = 0.0f;
            }
        }
    }
}
