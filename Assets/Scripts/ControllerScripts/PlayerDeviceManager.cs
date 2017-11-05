using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

//Player Device manager that handles assigning devices to action sets and providing them
//upon request. Handles intermittent connection of controllers and supports keyboard as well.
public class PlayerDeviceManager : MonoBehaviour
{
    public static PlayerDeviceManager instance;
    private int currentPlayers = 0;
    private const int maxPlayers = 2;

    List<PlayerActions> playerDevices = new List<PlayerActions>(maxPlayers);

    //Default control profiles for all keyboard/controller users
    PlayerActions keyboardListener;
    PlayerActions controllerListener;

    //Interface for retreiving player controls. Needs to be checked in Update in case of controller connnection/disconnection.
    public PlayerActions GetControls(int playerID)
    {
        return playerDevices[playerID];
    }

    private void OnEnable()
    {
        InputManager.OnDeviceDetached += OnDeviceDetached;
        keyboardListener = PlayerActions.CreateWithKeyboardBindings();
        controllerListener = PlayerActions.CreateWithControllerBindings();

        for(int i = currentPlayers; i < maxPlayers; ++i)
        {
            playerDevices.Add(null);
        }

        Debug.Log("PDM Started.");
    }

    private void OnDisable()
    {
        InputManager.OnDeviceDetached -= OnDeviceDetached;
        if(keyboardListener != null)
            keyboardListener.Destroy();
        if(controllerListener != null)
            controllerListener.Destroy();
    }

    void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }

        else
        {
            Debug.Log("Instance already exists.");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
	}

	void Update ()
    {
        //Checks join button on all controllers.
        if(ListenerJoins(controllerListener))
        {
            Debug.Log("Join Pressed");
            //Most recently Active Device.
            InputDevice lastActive = InputManager.ActiveDevice;

            //If this controller isn't being used, add it
            if(FindController(lastActive) == null)
            {
                AddPlayerDevice(lastActive);
            }
        }

        //Checks join button on the keyboard
        if(ListenerJoins(keyboardListener))
        {
            Debug.Log("Join Pressed");
            if(FindKeyboard() == null)
            {
                AddPlayerDevice(null);
            }
        }

		//Check if the swap button was pressed
		if (SwitchPressed (controllerListener) || SwitchPressed (keyboardListener))
		{
			Debug.Log ("Swap Pressed");
			PlayerActions temp = playerDevices [0];
			playerDevices [0] = playerDevices [1];
			playerDevices [1] = temp;
		}
	}

    //Returns true if the join button has been pressed.
    bool ListenerJoins(PlayerActions actions)
    {
        return actions.Join.WasPressed;
    }

	//Returns true if the swap button is pressed.
	bool SwitchPressed(PlayerActions actions)
	{
		return actions.SwitchCharacters.WasPressed;
	}

    //Checks all devices in use to see if targetDevice is in use.
    PlayerActions FindController(InputDevice targetDevice)
    {
        foreach(PlayerActions controller in playerDevices)
        {
            if((controller != null) && (controller.Device == targetDevice))
            {
                return controller;
            }
        }

        return null;
    }

    //Checks all devices to see if keyboard is in use.
    PlayerActions FindKeyboard()
    {
        foreach(PlayerActions keyboard in playerDevices)
        {
            if(keyboard == keyboardListener)
            {
                return keyboard;
            }
        }

        return null;
    }

    //When a controller is detached, check if its one that's being used and destroy it's set if so.
    //TODO: Define better behavior on controller disconnect. 
    void OnDeviceDetached(InputDevice detachedDevice)
    {
        PlayerActions targetDevice = FindController(detachedDevice);
        if(targetDevice != null)
        {
            playerDevices.Remove(targetDevice);
            --currentPlayers;
        }

        return;
    }

    PlayerActions AddPlayerDevice(InputDevice targetDevice)
    {
        if(currentPlayers < maxPlayers)
        {
            PlayerActions targetDeviceActionSet;

            //If its the keyboard, we can just add it. There's only one.
            if(targetDevice == null)
            {
                Debug.Log("KB Added");
                targetDeviceActionSet = keyboardListener;
            }

            //If its a controller, instantiate a new action set bound to the target device.
            else
            {
                targetDeviceActionSet = PlayerActions.CreateWithControllerBindings();
                targetDeviceActionSet.Device = targetDevice;
            }

            for(int i = 0; i < maxPlayers; ++i)
            {
                if(playerDevices[i] == null)
                {
                    Debug.Log("New set added");
                    playerDevices[i] = targetDeviceActionSet;
                    break;
                }
            }

            ++currentPlayers;
            return targetDeviceActionSet;
        }

        return null;
    }
}
