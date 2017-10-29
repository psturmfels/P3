using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class StateMachineForJack : MonoBehaviour {
	public enum State {
		Normal,
		Transformed,
		InTransition
	};

	public GameObject normalObject;
	public GameObject transformedObject;

	private State currentState = State.Normal;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private MellowStates ms;

    private int playerID = 0;

    public void SetState(State newState) {
		currentState = newState;
	}

	void TransitionToState(State newState) {
		if (newState == currentState || currentState == State.InTransition) {
			return;
		}

		currentState = State.InTransition;
		if (newState == State.Transformed) {
			if (normalObject.GetComponent<TransformBehavior> () != null) {
				normalObject.GetComponent<TransformBehavior> ().ScaleToTransform ();
			}
		} else if (newState == State.Normal) {
			if (transformedObject.GetComponent <TransformBehavior> () != null) {
				transformedObject.GetComponent <TransformBehavior> ().ScaleToTransform ();
			}
		}
	}

    void Start()
    {
        ms = GetComponentInChildren<MellowStates>();

        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();

        //Grab playerID for controller purposes.
        if(ms)
        {
            playerID = ms.playerID;
        }

    }

    void Update () {
        //Find the controls bound to this player
        if (deviceManager != null)
        {
            controls = deviceManager.GetControls(playerID);
        }

        if(controls != null)
        {
            if(controls.Transform.IsPressed)
            {
                if(currentState == State.Normal)
                {
                    TransitionToState(State.Transformed);
                }
                else if(currentState == State.Transformed)
                {
                    TransitionToState(State.Normal);
                }
            }
        }
	}
}
