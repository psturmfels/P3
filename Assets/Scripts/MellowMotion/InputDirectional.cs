using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDirectional : MonoBehaviour {

    private int playerID = 0;
    private PlayerDeviceManager deviceManager;
    private float horizontalAxis;
    private float verticalAxis;

    // Use this for initialization
    void Start() {
        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
    }

    public void SetCurrentHorzAxis(float newHorzAxis) {
        horizontalAxis = newHorzAxis;
    }

    public float GetCurrentHorzAxis() {
        return horizontalAxis;
    }

    void Update() {
        //Find the controls bound to this player
        if (deviceManager != null) {
            PlayerActions controls = deviceManager.GetControls(playerID);
            if (controls != null) {
                SetCurrentHorzAxis(controls.Move.X);
            }
            else {
                SetCurrentHorzAxis(0.0f);
            }
        }
    }
}
