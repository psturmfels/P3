using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour {

    private PlayerDeviceManager deviceManager;
    private PlayerActions p1Controls;
    private PlayerActions p2Controls;

    private bool paused = false;
    private Animator anim;

    // Use this for initialization
    void Start () {
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        p1Controls = deviceManager.GetControls(0);
        p2Controls = deviceManager.GetControls(1);
	    if (p1Controls != null && p1Controls.Menu.WasPressed ||
            p2Controls != null && p2Controls.Menu.WasPressed) {
            if (paused) {
                Unpause();
            }
            else {
                Pause();
            }
        }
        anim.SetBool("Paused", paused);
    }

    public void Pause() {
        paused = true;
    }

    public void Unpause() {
        paused = false;
    }
}
