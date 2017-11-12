using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    private PlayerDeviceManager deviceManager;
    private PlayerActions p1Controls;
    private PlayerActions p2Controls;

    // Use this for initialization
    void Start () {
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
        p1Controls = deviceManager.GetControls(0);
        p2Controls = deviceManager.GetControls(1);
    }
	
	// Update is called once per frame
	void Update () {
	    if (p1Controls.Jump.WasPressed || p2Controls.Jump.WasPressed) {
	        Button btn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            btn.onClick.Invoke();
	    }
	}
}
