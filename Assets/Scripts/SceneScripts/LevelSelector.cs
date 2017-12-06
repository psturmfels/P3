using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public int levelNo = 1;

    private bool player1InRange = false;
    private bool player2InRange = false;

//    private bool playerInRange = false;
    private PlayerDeviceManager deviceManager;
    private PlayerActions p1Controls;
    private PlayerActions p2Controls;

    // Use this for initialization
    void Start () {
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
    }
	
	// Update is called once per frame
    void Update() {
        if (player1InRange) {
            p1Controls = deviceManager.GetControls(0);
            if (p1Controls != null && p1Controls.Join.IsPressed) {
                JoinLevel();
            }
        }
        if (player2InRange) {
            p2Controls = deviceManager.GetControls(1);
            if (p2Controls != null && p2Controls.Join.IsPressed) {
                JoinLevel();
            }
        }
    }

    private void JoinLevel() {
        GameObject candyWave = GameObject.Find("CandyWave");
        if (candyWave != null) {
            ReverseDecay rd = candyWave.GetComponent<ReverseDecay>();
            if (rd != null) {
                rd.ReverseWaveDecay();
            }
        }
        Invoke("EnterLevel", 2);
    }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject otherGO = other.gameObject;
        if (otherGO.CompareTag("Player")) {
            if (otherGO.name == "BridgeMellow") {
                player1InRange = true;
            }
            else if (otherGO.name == "StiltMellow") {
                player2InRange = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        GameObject otherGO = other.gameObject;
        if (other.gameObject.CompareTag("Player")) {
            if (otherGO.name == "BridgeMellow") {
                player1InRange = false;
            }
            else if (otherGO.name == "StiltMellow") {
                player2InRange = false;
            }
        }
    }

    //this exists so it can be invoked and allow the transition animation to fire
    void EnterLevel() {
        SceneLoader.instance.LoadLevel(levelNo.ToString());
    }

}
