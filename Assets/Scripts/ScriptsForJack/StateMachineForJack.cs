using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class StateMachineForJack : MonoBehaviour {
	public enum State {
		Normal,
		Transformed,
		InTransition,
		Disabled
	};

	public GameObject normalObject;
	public GameObject transformedObject;

	public State currentState = State.Normal;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private MellowStates ms;
	private Rigidbody2D rb;

    private int playerID = 0;

    public void SetState(State newState) {
		currentState = newState;
	}

	public StateMachineForJack.State GetState() {
		return currentState;
	}

	public void TransitionToState(State newState) {
		if (newState == currentState || currentState == State.InTransition) {
			return;
		}

		if (newState == State.Transformed) {
			EnableTransformedObject ();
			if (transformedObject.GetComponent <TransformBehavior> () != null) {
				transformedObject.GetComponent <TransformBehavior> ().StartTowardsTransform ();
			}
		} else if (newState == State.Normal) {
			if (transformedObject.GetComponent <TransformBehavior> () != null) {
				transformedObject.GetComponent <TransformBehavior> ().StartTowardsNormal ();
			}
		}
	}

    void Start()
    {
        ms = GetComponentInChildren<MellowStates>();
		rb = GetComponent<Rigidbody2D> ();

        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();

        //Grab playerID for controller purposes.
        if(ms)
        {
            playerID = ms.playerID;
        }

		if (GetComponentInChildren<MellowCrushed> () != null) {
			GetComponentInChildren<MellowCrushed> ().DisableTransform += DisableTransform;
			GetComponentInChildren<MellowCrushed> ().Respawn += EnableTransform;
		}
		if (transformedObject.GetComponent <TransformBehavior> () != null) {
			TransformBehavior transBeh = transformedObject.GetComponent <TransformBehavior> ();
			transBeh.ReachedNormal += EnableMovementObject;
			
		}
    }

	void EnableTransformedObject() {
		normalObject.SetActive (false);
		transformedObject.SetActive (true);
		rb.velocity = Vector2.zero;
		rb.isKinematic = true;
	}

	void EnableMovementObject() {
		normalObject.SetActive (true);
		transformedObject.SetActive (false);
		rb.isKinematic = false;
		rb.velocity = Vector2.zero;
	}

	void DisableTransform() {
		currentState = State.Disabled;
	}

	void EnableTransform() {
		currentState = State.Normal;
	}

    void Update () {
        //Find the controls bound to this player
        if (deviceManager != null)
        {
            controls = deviceManager.GetControls(playerID);
        }

        if(controls != null)
        {
            if(controls.Transform.WasPressed)
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
