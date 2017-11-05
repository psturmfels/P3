using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class PlayerReadyUp : MonoBehaviour
{
    private PlayerActions playerControls;
    public GameObject Player1Ready;
    public GameObject Player2Ready;
    public GameObject YButton;
    public bool ready = false;
    int playerID;
    private float delay = 0;

    //When this is enabled, find this players controls and enable their ready up sign.
    private void OnEnable ()
    {
        playerID = this.GetComponentInChildren<MellowStates>().playerID;
        PlayerDeviceManager InstancePlayerDeviceManger = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
        playerControls = InstancePlayerDeviceManger.GetControls(playerID);

        if(playerID == 0)
        {
            Player1Ready.SetActive(true);
        }

        else if(playerID == 1)
        {
            Player2Ready.SetActive(true);
        }
	}

    private void OnDisable()
    {
        if(playerID == 0)
        {
            if(Player1Ready != null)
                Player1Ready.SetActive(false);
        }

        else if(playerID == 1)
        {
            if(Player2Ready != null)
                Player2Ready.SetActive(false);
        }
    }

    //If this player presses join, make them as ready.
    private void Update()
    {
        if(delay < .3f)
        {
            delay += Time.deltaTime;
            return;
        }

        if((playerID == 0) && (playerControls.Join.WasPressed))
        {
            Player1Ready.GetComponentInChildren<Text>().text = "Player 1 Ready!";
            YButton.SetActive(false);
            ready = true;
        }

        else if((playerID == 1) && (playerControls.Join.WasPressed))
        {
            Player2Ready.GetComponentInChildren<Text>().text = "Player 2 Ready!";
            YButton.SetActive(false);
            ready = true;
        }
    }
}
