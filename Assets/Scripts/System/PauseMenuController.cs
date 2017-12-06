using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour {

    private PlayerDeviceManager deviceManager;
    private PlayerActions p1Controls;
    private PlayerActions p2Controls;

    // Use this for initialization
    void Start () {
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
    }
	
	// Update is called once per frame
	void Update () {
        p1Controls = deviceManager.GetControls(0);
        p2Controls = deviceManager.GetControls(1);
	    if (p1Controls != null && p1Controls.Menu.WasPressed ||
            p2Controls != null && p2Controls.Menu.WasPressed) {
            SceneLoader.instance.LoadMenu();
        }
    }
}
