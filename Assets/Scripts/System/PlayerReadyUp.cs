using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class PlayerReadyUp : MonoBehaviour
{
    public GameObject readyPanel;
    public GameObject sleepZZZ;
    public bool ready = false;
    int playerID;

    //When this is enabled, find this players controls and enable their ready up sign.
    public void Enable () {
        if (!ready) {
            playerID = GetComponentInChildren<MellowStates>().playerID;
            readyPanel.SetActive(true);
            sleepZZZ.SetActive(false);
            ready = true;
        }
    }

    public void Disable() {
        if (ready) {
            if (readyPanel != null) {
                readyPanel.SetActive(false);
            }
            if (sleepZZZ != null) {
                sleepZZZ.SetActive(true);
            }
            ready = false;
        }
    }
}
