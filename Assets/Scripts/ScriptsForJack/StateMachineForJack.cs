using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
	public UnityAction InitiateTransformReload;
	public UnityAction BeganInputTransform;

	public State currentState = State.Normal;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private MellowStates ms;
	private Rigidbody2D rb;
	private float reloadAfterTransformTime = 0.75f;

    private int playerID = 0;

    public void SetState(State newState) {
		currentState = newState;
	}

	public StateMachineForJack.State GetState() {
		return currentState;
	}

	public int GetPlayerID () {
		return playerID;
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
			GetComponentInChildren<MellowCrushed> ().Respawn += RefreshTransform;
		}
		if (transformedObject.GetComponent <TransformBehavior> () != null) {
			TransformBehavior transBeh = transformedObject.GetComponent <TransformBehavior> ();
			transBeh.ReachedNormal += EnableMovementObject;
			transBeh.ReachedDeath += ReachedDeath;
			transBeh.ReachedDeath += EnableMovementObject;
			transBeh.ReachedTransform += ReachedFullTransform;
			transBeh.ReachedNormal += RefreshTransform;
		}
    }

	public void GoToTransform() {
		if (currentState != State.Normal) {
			return;
		}
		BeganInputTransform ();
		EnableTransformedObject ();
		if (transformedObject.GetComponent <TransformBehavior> () != null) {
			transformedObject.GetComponent <TransformBehavior> ().StartTowardsTransform ();
		}
	}

	public void GoToNormal() {
		if (currentState != State.Transformed) {
			return;
		}

		if (transformedObject.GetComponent <TransformBehavior> () != null) {
			transformedObject.GetComponent <TransformBehavior> ().StartTowardsNormal ();
		}
	}

	void RefreshTransform() {
		InitiateTransformReload ();
		doesAcceptInput = false;
		StartCoroutine (EnableAcceptInputAfterTime (reloadAfterTransformTime));
	}
		
	IEnumerator EnableAcceptInputAfterTime(float timeToWait) {
		yield return new WaitForSeconds (timeToWait);
		doesAcceptInput = true;
	}

	void EnableTransformedObject() {
		normalObject.SetActive (false);
		transformedObject.SetActive (true);
		rb.velocity = Vector2.zero;
		rb.gravityScale = 0.0f;
	}

	void ReachedFullTransform () {
		rb.isKinematic = true;
	}

	void EnableMovementObject() {
		normalObject.SetActive (true);
		transformedObject.SetActive (false);
		rb.isKinematic = false;
		rb.gravityScale = 3.0f; 
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
			} else if (!controls.Transform.IsPressed) {
				GoToNormal ();
			}
        }
	}
}
