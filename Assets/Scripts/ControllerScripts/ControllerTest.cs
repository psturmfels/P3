using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    PlayerDeviceManager deviceManager;
    PlayerActions controls;

    public GameObject Jump;
    public GameObject Transform;
    public GameObject Interact;
    public GameObject DPadDown;
    public GameObject DPadUp;
    public GameObject DPadLeft;
    public GameObject DPadRight;
    public GameObject Join;

    public GameObject Stick;

    public Material PressedMaterial;
    public Material UnpressedMaterial;
    public int playerID = 0;

    //Find the player device manager
    void Start ()
    {
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();
	}
	
	//Get controls if bound, update colors if possible
	void Update ()
    {
        if((deviceManager != null) && (controls == null))
        {
            controls = deviceManager.GetControls(playerID);
        }

        if(controls != null)
        {
            changeColors();
            drawStick();

            if(controls.Menu.WasPressed)
            {
                Debug.Log("menu was pressed");
            }
        }
	}

    void changeColors()
    {
        if(controls.Jump.IsPressed)
        {
            Jump.GetComponent<MeshRenderer>().material = PressedMaterial;
        }

        if(!controls.Jump.IsPressed)
        {
            Jump.GetComponent<MeshRenderer>().material = UnpressedMaterial;
        }

        if(controls.Transform.IsPressed)
        {
            Transform.GetComponent<MeshRenderer>().material = PressedMaterial;
        }

        if(!controls.Transform.IsPressed)
        {
            Transform.GetComponent<MeshRenderer>().material = UnpressedMaterial;
        }

        if(controls.Interact.IsPressed)
        {
            Interact.GetComponent<MeshRenderer>().material = PressedMaterial;
        }

        if(!controls.Interact.IsPressed)
        {
            Interact.GetComponent<MeshRenderer>().material = UnpressedMaterial;
        }

        if(controls.ResetLevel.IsPressed)
        {
            DPadUp.GetComponent<MeshRenderer>().material = PressedMaterial;
        }

        if(!controls.ResetLevel.IsPressed)
        {
            DPadUp.GetComponent<MeshRenderer>().material = UnpressedMaterial;
        }

        if(controls.ResetCheckpoint.IsPressed)
        {
            DPadDown.GetComponent<MeshRenderer>().material = PressedMaterial;
        }

        if(!controls.ResetCheckpoint.IsPressed)
        {
            DPadDown.GetComponent<MeshRenderer>().material = UnpressedMaterial;
        }

        if(controls.AdvanceLevel.IsPressed)
        {
            DPadRight.GetComponent<MeshRenderer>().material = PressedMaterial;
        }

        if(!controls.AdvanceLevel.IsPressed)
        {
            DPadRight.GetComponent<MeshRenderer>().material = UnpressedMaterial;
        }

        if(controls.BackLevel.IsPressed)
        {
            DPadLeft.GetComponent<MeshRenderer>().material = PressedMaterial;
        }

        if(!controls.BackLevel.IsPressed)
        {
            DPadLeft.GetComponent<MeshRenderer>().material = UnpressedMaterial;
        }

        if(controls.Join.IsPressed)
        {
            Join.GetComponent<MeshRenderer>().material = PressedMaterial;
        }

        if(!controls.Join.IsPressed)
        {
            Join.GetComponent<MeshRenderer>().material = UnpressedMaterial;
        }
    }

    void drawStick()
    {
        Vector3 spherePosition = Stick.GetComponent<Transform>().position;
        Vector3 stickPosition = controls.Move.Vector;
        spherePosition.z += 2;
        Debug.DrawRay(spherePosition, stickPosition, Color.red);
    }
}
