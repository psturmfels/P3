using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using InControl;

public class MellowCrushed : MonoBehaviour {
	public Sprite[] crushSprites;
	public float timeBetweenCrushSprites;
	public GameObject deathExplosion;
	public bool isStilt;

	public event UnityAction Die;
	public event UnityAction Respawn;
	public event UnityAction Remove;

	private float removeDelay;
	private MoveAnimate ma;
	private MellowStates ms;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private int playerID = 0;
	private float fadeInSpeed = 0.1f;

    public void StartDie() {
		CollaborativeDie ();
		Die ();
	}

	public void CollaborativeDie() {
		if (GetComponentInParent<StateMachineForJack> () != null && GetComponentInParent<StateMachineForJack> ().GetState() == StateMachineForJack.State.Transformed) {
			GetComponentInParent<StateMachineForJack> ().TransitionToState (StateMachineForJack.State.Normal);
			Invoke ("CollaborativeDie", 0.2f);
			return;
		} else if (GetComponent<TransformBehavior> () != null) {
			GetComponent<TransformBehavior> ().RegisterCancelContact (1);
			GetComponent<TransformBehavior> ().RegisterCancelContact (2);
		}

		DisableInput ();
		ma.InterruptMovementAnimation (crushSprites, timeBetweenCrushSprites);
		Invoke ("RemoveSelf", removeDelay);
	}

	void Start () {
		ma = GetComponent<MoveAnimate> ();
		ms = GetComponent<MellowStates> ();
		removeDelay = (crushSprites.Length - 1) * timeBetweenCrushSprites;
		if (isStilt) {
			GameObject bridgeMellowMove = GameObject.Find ("BridgeMellowMove");
			if (bridgeMellowMove != null && bridgeMellowMove.GetComponent<MellowCrushed> () != null) {
				bridgeMellowMove.GetComponent<MellowCrushed> ().Die += CollaborativeDie;
			}
		} else {
			GameObject bridgeMellowMove = GameObject.Find ("StiltMellowMove");
			if (bridgeMellowMove != null && bridgeMellowMove.GetComponent<MellowCrushed> () != null) {
				bridgeMellowMove.GetComponent<MellowCrushed> ().Die += CollaborativeDie;
			}
		}

		GameObject mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		if (mainCamera != null && mainCamera.GetComponent<CameraMovement> () != null) {
			mainCamera.GetComponent<CameraMovement> ().reachedCheckpoint += RespawnSelf;
		}

        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();

        //Grab playerID for controller purposes.
        if(ms) {
            playerID = ms.playerID;
        }
    }

	void Update () {
        //Find the controls bound to this player
        if((deviceManager != null) && (controls == null))
        {
            controls = deviceManager.GetControls(playerID);
        }

//        if(controls != null) WHY WAS THIS HERE? LOL
//        {
//            if(controls == null)
//            {
//				StartDie();
//            }
//        }
	}

	private void DisableInput() {
		if (GetComponent<Rigidbody2D> () != null) {
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			GetComponent<Rigidbody2D> ().simulated = false;
		}
		if (GetComponent<BoxCollider2D> () != null) {
			GetComponent<BoxCollider2D> ().enabled = false;
		}
		if (GetComponent<PickUpAction> () != null) {
			GetComponent<PickUpAction> ().DropItem ();
		}
		ms.SetState (MellowStates.State.Dead, true);
	}

	private void EnableInput() {
		if (GetComponent<Rigidbody2D> () != null) {
			GetComponent<Rigidbody2D> ().simulated = true;
		}
		if (GetComponent<BoxCollider2D> () != null) {
			GetComponent<BoxCollider2D> ().enabled = true;
		}
		ms.SetState (MellowStates.State.Dead, false);
		ms.EnableMovementInput (); 
	}

	void RemoveSelf() {
		ma.DisableRenderer ();
		if (deathExplosion != null) {
			Instantiate (deathExplosion, transform.position, Quaternion.identity);
		}
		Remove ();
	}

	void RespawnSelf() {
		Respawn ();

		StartCoroutine (FadeAndScaleIn ());
	}

	IEnumerator FadeAndScaleIn() {
		ma.ReturnMovementAnimation ();
		transform.localScale = new Vector3 (3.0f, 3.0f, 1.0f);
		Vector3 targetScale = Vector3.one;
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		Color targetColor = Color.white;
		if (sr != null) {
			targetColor = sr.color;
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0.0f);
		}

		int iterationNumber = 1;

		while (transform.localScale != targetScale || sr.color != targetColor) {
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, fadeInSpeed * iterationNumber);
			sr.color = Color.Lerp (sr.color, targetColor, fadeInSpeed * iterationNumber);
			yield return null;
			iterationNumber += 1;
		}

		EnableInput ();
	}
}
