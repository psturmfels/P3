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
    public AudioSource shiftSound;
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

    void Start() {
        ms = GetComponentInChildren<MellowStates>();
		rb = GetComponent<Rigidbody2D> ();

        if (shiftSound == null) {
            if (this.gameObject.name == "BridgeMellow") {
                shiftSound = GameObject.Find("GameCamera").transform.Find("SFX").Find("BridgeShift").GetComponent<AudioSource>();
            }
            else {
                shiftSound = GameObject.Find("GameCamera").transform.Find("SFX").Find("StiltShift").GetComponent<AudioSource>();
            }
        }

        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();

        //Grab playerID for controller purposes.
        if(ms) {
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
//        GetComponent<Animator>().SetBool("walking", false);
        shiftSound.Play();
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
		if (gameObject.name.Contains ("Stilt")) {
			rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		} else if (gameObject.name.Contains("Bridge")) {
			rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
		}

	}

	void ReachedFullTransform () {
	    rb.velocity = Vector2.zero;
//		rb.isKinematic = true;
//		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	void EnableMovementObject() {
		normalObject.SetActive (true);
		transformedObject.SetActive (false);
		rb.isKinematic = false;
		rb.gravityScale = 3.0f; 
		rb.velocity = Vector2.zero;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
