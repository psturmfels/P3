using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

//Character Select Manager. Watches for join to be pressed and activates players when this occurs.
public class CharacterSelectManager : MonoBehaviour {
    private GameObject BridgeMellow;
    private GameObject StiltMellow;
    private GameObject Directions;

    private EyeController bridgeEyes;
    private EyeController stiltEyes;
    private MouthMovement bridgeMouth;
    private MouthMovement stiltMouth;
    private PlayerReadyUp bridgePRU;
    private PlayerReadyUp stiltPRU;


    private PlayerDeviceManager PlayerDeviceManager;

    private const int maxPlayers = 2;

    private void Start () {
        BridgeMellow = GameObject.Find("BridgeMellow");
        StiltMellow = GameObject.Find("StiltMellow");
        PlayerDeviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
        Directions = GameObject.Find("HelperText Join");

        //Hide both characters after finding them.
        bridgePRU = BridgeMellow.GetComponent<PlayerReadyUp>();
        stiltPRU = StiltMellow.GetComponent<PlayerReadyUp>();
        bridgePRU.Disable();
        stiltPRU.Disable();

        // Change facial expressions
        bridgeEyes = BridgeMellow.GetComponentInChildren<EyeController>();
        stiltEyes = StiltMellow.GetComponentInChildren<EyeController>();
        bridgeEyes.ChangeToSleepyEyes();
        stiltEyes.ChangeToSleepyEyes();

        bridgeMouth = BridgeMellow.GetComponentInChildren<MouthMovement>();
        stiltMouth = StiltMellow.GetComponentInChildren<MouthMovement>();
        bridgeMouth.Sleepy();
        stiltMouth.Sleepy();
    }
	
	private void Update () {
	    if (BridgeMellow != null && StiltMellow != null) {
            int checkPlayers = 0;

            //Check how many players are spawned in.
            for (int i = 0; i < maxPlayers; ++i)
            {
                if (PlayerDeviceManager.GetControls(i) != null)
                {
                    ++checkPlayers;
                }
            }

            //If 1 player has bound their controls, add bridge.
            if (checkPlayers == 1)
            {
                BridgeMellow.SetActive(true);
                bridgeEyes.ChangeToNormalEyes();
                bridgeMouth.Default();
                bridgePRU.Enable();
            }


            //If 2 players have bound, add stilt.
            else if (checkPlayers == 2)
            {
                BridgeMellow.SetActive(true);
                bridgeEyes.ChangeToNormalEyes();
                bridgeMouth.Default();
                bridgePRU.Enable();
                StiltMellow.SetActive(true);
                stiltEyes.ChangeToNormalEyes();
                stiltMouth.Default();
                stiltPRU.Enable();
                Directions.SetActive(false);
            }

            //Check if both players are ready
            if (BridgeMellow.GetComponent<PlayerReadyUp>().ready &&
               StiltMellow.GetComponent<PlayerReadyUp>().ready)
            {
                GameObject spBarrier = GameObject.Find("SPBarrier");
                if (spBarrier != null)
                {
                    for (int i = 0; i < spBarrier.transform.childCount; ++i)
                    {
                        spBarrier.transform.GetChild(i).gameObject.GetComponent<FadeOutAndDie>().StartFadeOut();
                    }
                    spBarrier.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}
