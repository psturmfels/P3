using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class StateMachineForJack : MonoBehaviour {
	public bool doesAcceptInput = true;
	public enum State {
		Normal,
		Transformed,
		InTransition,
		Disabled,
		Dead
	};

	public GameObject normalObject;
	public GameObject transformedObject;

	public State currentState = State.Normal;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private MellowStates ms;
	private Rigidbody2D rb;
	private float reloadAfterCancelTime = 0.5f;

    private int playerID = 0;

    public void SetState(State newState) {
		currentState = newState;
	}

	public StateMachineForJack.State GetState() {
		return currentState;
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
			transBeh.ReachedDeath += ReachedDeath;
			transBeh.ReachedDeath += EnableMovementObject;
			transBeh.WasCanceled += ReloadTransform;
		}
    }

	public void GoToTransform() {
		if (currentState == State.Disabled || currentState == State.Dead || currentState == State.Transformed) {
			return;
		}

		EnableTransformedObject ();
		if (transformedObject.GetComponent <TransformBehavior> () != null) {
			transformedObject.GetComponent <TransformBehavior> ().StartTowardsTransform ();
		}
	}

	public void GoToNormal() {
		if (currentState == State.Disabled || currentState == State.Dead || currentState == State.Normal) {
			return;
		}

		if (transformedObject.GetComponent <TransformBehavior> () != null) {
			transformedObject.GetComponent <TransformBehavior> ().StartTowardsNormal ();
		}
	}

	void ReloadTransform() {
		doesAcceptInput = false;
		StartCoroutine (EnableAcceptInputAfterTime ());
	}

	IEnumerator EnableAcceptInputAfterTime() {
		yield return new WaitForSeconds (reloadAfterCancelTime);
		doesAcceptInput = true;
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

	void ReachedDeath() {
		currentState = State.Dead;
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
			if (controls.Transform.IsPressed && doesAcceptInput) {
				GoToTransform ();
			} else {
				GoToNormal ();
			}
        }
	}
}
