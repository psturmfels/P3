using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class PlayerReadyUp : MonoBehaviour
{
//    private PlayerActions playerControls;
    public GameObject readyPanel;
    public GameObject sleepZZZ;
//    public GameObject YButton;
    public bool ready = false;
    int playerID;
//    private float delay = 0;

    //When this is enabled, find this players controls and enable their ready up sign.
    public void Enable () {
        playerID = this.GetComponentInChildren<MellowStates>().playerID;
//        PlayerDeviceManager InstancePlayerDeviceManger = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
//        playerControls = InstancePlayerDeviceManger.GetControls(playerID);
        readyPanel.SetActive(true);
        sleepZZZ.SetActive(false);
        ready = true;
    }

    public void Disable() {
        if (readyPanel != null) {
            readyPanel.SetActive(false);
        }
        if (sleepZZZ != null) {
            sleepZZZ.SetActive(true);
        }
        ready = false;
    }
}
