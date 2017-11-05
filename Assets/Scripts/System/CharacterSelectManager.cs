using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

//Character Select Manager. Watches for join to be pressed and activates players when this occurs.
public class CharacterSelectManager : MonoBehaviour
{
    private GameObject BridgeMellow;
    private GameObject StiltMellow;
    private GameObject Directions;

    private PlayerDeviceManager PlayerDeviceManager;

    private const int maxPlayers = 2;

    private void Start ()
    {
        BridgeMellow = GameObject.Find("BridgeMellow");
        StiltMellow = GameObject.Find("StiltMellow");
        PlayerDeviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
        Directions = GameObject.Find("Directions");

        //Hide both characters after finding them.
        BridgeMellow.SetActive(false);
        StiltMellow.SetActive(false);
    }
	
	private void Update ()
    {
        int checkPlayers = 0;

        //Check how many players are spawned in.
        for(int i = 0; i < maxPlayers; ++i)
        {
            if(PlayerDeviceManager.GetControls(i) != null)
            {
                ++checkPlayers;
            }
        }

        //If 1 player has bound their controls, add bridge.
        if(checkPlayers == 1)
        {
            BridgeMellow.SetActive(true);
        }


        //If 2 players have bound, add stilt.
        else if(checkPlayers == 2)
        {
            BridgeMellow.SetActive(true);
            StiltMellow.SetActive(true);
            Directions.SetActive(false);
        }

        //Check if both players are ready
        if((BridgeMellow.GetComponent<PlayerReadyUp>().ready == true) &&
            (StiltMellow.GetComponent<PlayerReadyUp>().ready == true))
        {
            //TODO: Scene Transition
            Debug.Log("Both players ready.");
            SceneLoader.instance.LoadMenu();
        }
    }
}
