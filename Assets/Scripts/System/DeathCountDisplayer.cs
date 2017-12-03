using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCountDisplayer : MonoBehaviour {
    public bool followBridge = true;
    private Text deathText;

    private void Start() {
        deathText = GetComponent<Text>();
    }

    private void Update() {
        int numDeaths = 0;
        if (followBridge) {
            numDeaths = GameObject.Find("StiltMellow").GetComponent<MellowCrushed>().numberOfDeaths;
        }
        else {
            numDeaths = GameObject.Find("BridgeMellow").GetComponent<MellowCrushed>().numberOfDeaths;
        }
        if (deathText != null) {
            deathText.text = "You let your teammate die\n" + numDeaths + " times!";
        }
    }
}
