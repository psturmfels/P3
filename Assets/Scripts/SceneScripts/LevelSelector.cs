using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public int levelNo = 1;

    private bool playerInRange = false;
    private PlayerDeviceManager deviceManager;
    private PlayerActions p1Controls;
    private PlayerActions p2Controls;

    // Use this for initialization
    void Start () {
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (playerInRange) {
            p1Controls = deviceManager.GetControls(0);
            p2Controls = deviceManager.GetControls(1);
            if ((p1Controls != null && p1Controls.Jump.WasPressed) ||
                (p2Controls != null && p2Controls.Jump.WasPressed)) {
                SceneLoader.instance.LoadLevel(levelNo.ToString());
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            playerInRange = false;
        }
    }

}
