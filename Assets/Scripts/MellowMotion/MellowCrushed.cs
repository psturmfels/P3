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

	public event UnityAction DisableOther;
	public event UnityAction DisableTransform;
	public event UnityAction Respawn;
	public event UnityAction Remove;

    public AudioSource deathSound;
    public AudioSource spawnSound;
	private float removeDelay;
	private StateMachineForJack stateMachine; 
	private MoveAnimate ma;
	private MellowStates ms;
    private PlayerActions controls;
	private BoxCollider2D normalStateCollider;
	private PickUpAction normalStatePickup;
	private SpriteRenderer normalStateRenderer;
	private TransformBehavior transformStateTransform;
    private PlayerDeviceManager deviceManager;
	private Rigidbody2D rb;
    private int playerID = 0;
	private float fadeInSpeed = 0.1f;
	private float shrinkInSpeed = 0.5f;

    public void StartDie() {
		if (ms.isDead) {
			return;
		}

		BeginDieRoutine ();
		DisableOther ();
	}

	void BeginDieRoutine() {
		StartCoroutine (DieRoutine ());
	}

	IEnumerator DieRoutine() {
		DisableInput ();

		if (stateMachine != null && stateMachine.GetState () != StateMachineForJack.State.Normal && transformStateTransform != null) {
			transformStateTransform.TransformIntoDeath ();
			while (stateMachine.GetState () != StateMachineForJack.State.Dead) {
				yield return null;
			}
		} else if (DisableTransform != null) {
			DisableTransform ();
		}

		rb.velocity = Vector2.zero;
		ma.InterruptMovementAnimation (crushSprites, timeBetweenCrushSprites);

		yield return new WaitForSeconds (removeDelay);

        deathSound.Play();

		ma.DisableRenderer ();
		if (deathExplosion != null) {
			Instantiate (deathExplosion, transform.position, Quaternion.identity);
		}
		Remove ();
	}

	void Start () {
		stateMachine = GetComponent<StateMachineForJack> (); 
		ma = GetComponentInChildren<MoveAnimate> ();
		ms = GetComponentInChildren<MellowStates> ();
		rb = GetComponent<Rigidbody2D> ();
		normalStatePickup = GetComponentInChildren<PickUpAction> ();
		foreach (BoxCollider2D box in GetComponentsInChildren<BoxCollider2D> ()) {
			if (box.gameObject.name.Contains ("Move")) {
				normalStateCollider = box;
			}
		}
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer> ()) {
			if (sr.gameObject.name.Contains ("Move")) {
				normalStateRenderer = sr;
			}
		}
		transformStateTransform = GetComponentInChildren<TransformBehavior> (true);

        if (spawnSound == null || deathSound == null)
        {
            var cam = GameObject.Find("GameCamera");
            spawnSound = cam.transform.Find("SFX").Find("Spawn").GetComponent<AudioSource>();
            deathSound = cam.transform.Find("SFX").Find("Death").GetComponent<AudioSource>();
        }

		removeDelay = (crushSprites.Length - 1) * timeBetweenCrushSprites;
		if (isStilt) {
			GameObject bridgeMellow = GameObject.Find ("BridgeMellow");
			if (bridgeMellow != null && bridgeMellow.GetComponent<MellowCrushed> () != null) {
				bridgeMellow.GetComponent<MellowCrushed> ().DisableOther += BeginDieRoutine;
			}
		} else {
			GameObject stiltMellow = GameObject.Find ("StiltMellow");
			if (stiltMellow != null && stiltMellow.GetComponent<MellowCrushed> () != null) {
				stiltMellow.GetComponent<MellowCrushed> ().DisableOther += BeginDieRoutine;
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
	}

	private void DisableInput() {
		if (rb != null) {
			rb.velocity = Vector2.zero;
			rb.simulated = false;
		}
		if (normalStateCollider != null) {
			normalStateCollider.enabled = false;
		}
		if (normalStatePickup != null) {
			normalStatePickup.DropItem ();
		}
		ms.SetState (MellowStates.State.Dead, true);
	}

	private void EnableInput() {
		if (rb != null) {
			rb.simulated = true;
			rb.velocity = Vector2.zero;
		}
		if (normalStateCollider != null) {
			normalStateCollider.enabled = true;
		}
		ms.SetState (MellowStates.State.Dead, false);
		ms.EnableMovementInput (); 
	}

	void RespawnSelf() {
		Respawn ();
        spawnSound.Play();
		StartCoroutine (FadeAndScaleIn ());
	}

	IEnumerator FadeAndScaleIn() {
		ma.ReturnMovementAnimation ();
		transform.localScale = new Vector3 (3.0f, 3.0f, 1.0f);
		Vector3 targetScale = Vector3.one;
		Color targetColor = Color.white;
		if (normalStateRenderer != null) {
			targetColor = normalStateRenderer.color;
			normalStateRenderer.color = new Color (normalStateRenderer.color.r, normalStateRenderer.color.g, normalStateRenderer.color.b, 0.0f);
		}

		while (transform.localScale != targetScale || normalStateRenderer.color.a != targetColor.a) {
			transform.localScale = Vector3.MoveTowards (transform.localScale, targetScale, shrinkInSpeed);
			float newAlpha = Mathf.MoveTowards (normalStateRenderer.color.a, targetColor.a, fadeInSpeed);
			normalStateRenderer.color = new Color (normalStateRenderer.color.r, normalStateRenderer.color.g, normalStateRenderer.color.b, newAlpha);
			yield return null;
		}

		EnableInput ();
	}
}
