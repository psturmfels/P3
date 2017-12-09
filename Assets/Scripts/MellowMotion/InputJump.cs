using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using InControl;

public class InputJump : MonoBehaviour {
	public float jumpForce;
	public Sprite[] positiveJumpSprites;
	public Sprite[] negativeJumpSprites;
	public float timeBetweenJumpSprites;

	public event UnityAction DidJump;
    public AudioSource jumpSound;
	private float jumpForceModifier = 1.0f;
	private Rigidbody2D rb;
	private MoveAnimate ma;
	private InputMove im;
	private MellowStates ms;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private Animator anim;
    private int playerID = 0;
	private Vector2 dampenForce = Vector2.down * 50.0f;
	private bool shouldDampenFrames = false;
	private int framesCountedDampen = 0;
	private int framesDampenTotal = 7;
	private bool isJumping = false;
	private bool jumpWasPressed = false;
	private int inputBufferFrames = 6;
	private int inputFramesCounted = 0;

    public void StartJump(float forceModifier = 1.0f) {
		if (!ms.canJump || ms.canWallJumpLeft || ms.canWallJumpRight) {
			return;
		}
        if (anim != null) {
//            Debug.Log("Jump anim disabled");
            anim.SetBool("walking", false);
        }
        DidJump ();
		SpawnDust ();
		inputFramesCounted = 0;
		jumpWasPressed = false;
		StopAllCoroutines ();
		StartCoroutine (JumpRoutine (forceModifier));
	}

	IEnumerator JumpRoutine(float forceModifier) {
		ms.canJump = false;

		jumpForceModifier = forceModifier;
		if (im.GetCurrentFaceDirection () > 0.0f) {
			ma.InterruptMovementAnimation (positiveJumpSprites, timeBetweenJumpSprites);
		} else {
			ma.InterruptMovementAnimation (negativeJumpSprites, timeBetweenJumpSprites);
		}
			
		if (ms.isDead) {
			yield break;
		}

		rb.velocity = new Vector2 (rb.velocity.x, 0.0f);
		rb.AddRelativeForce (Vector2.up * jumpForce * jumpForceModifier, ForceMode2D.Impulse);
		shouldDampenFrames = true;
		dampenForce = Vector2.down * 50.0f;
		framesCountedDampen = 0; 
		isJumping = true;

        jumpSound.Play();

		ms.canJump = false;
		ms.canWallJumpLeft = false;
		ms.canWallJumpRight = false;
		if (ms.rightSideInContact) {
			ms.SetState (MellowStates.State.WallJumpLeft, true);
		} else if (ms.leftSideInContact) {
			ms.SetState (MellowStates.State.WallJumpRight, true);
		}
	}

	void Start () {
		ms = GetComponent<MellowStates> ();
		rb = GetComponentInParent<Rigidbody2D> ();
		ma = GetComponent<MoveAnimate> ();
		im = GetComponent<InputMove> ();
	    anim = GetComponentInParent<Animator>();
        if (jumpSound == null) {
            if (this.transform.parent.name == "BridgeMellow") {
                jumpSound = GameObject.Find("GameCamera").transform.Find("SFX").Find("BridgeJump").GetComponent<AudioSource>();
            }
            else {
                jumpSound = GameObject.Find("GameCamera").transform.Find("SFX").Find("StiltJump").GetComponent<AudioSource>();
            }
        }
        
        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();

        //Grab playerID for controller purposes.
        if(ms) {
            playerID = ms.playerID;	
        }
    }

	void FixedUpdate() {
		if (isJumping && rb.velocity.y < 0.1f) {
			isJumping = false;
			framesCountedDampen = 0; 
			dampenForce = Vector2.up * 17.0f;
			shouldDampenFrames = true;
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
			if (shouldDampenFrames) {
				if (!controls.Jump.IsPressed) {
					rb.AddRelativeForce (dampenForce, ForceMode2D.Force);
				}
				framesCountedDampen += 1;
				if (framesCountedDampen >= framesDampenTotal) {
					shouldDampenFrames = false;
				}
			}
			if(controls.Jump.WasPressed)
			{
				jumpWasPressed = true;
            }
			if (jumpWasPressed) {
				StartJump ();
				inputFramesCounted += 1;
				if (inputFramesCounted >= inputBufferFrames) {
					jumpWasPressed = false;
					inputFramesCounted = 0;
				}
			}
        }
	}

	void SpawnDust() {
		if (GetComponentInChildren<StickToUnder> () != null) {
			GameObject BottomCollider = GetComponentInChildren<StickToUnder> ().gameObject;
			if (BottomCollider.GetComponent<BoxCollider2D> () != null && !BottomCollider.GetComponent<BoxCollider2D> ().IsTouchingLayers(1 << LayerMask.NameToLayer("Ground"))) {
				return;
			}
		}
		Vector3 RightOffset = new Vector3 (0.5f, -0.554f, 0.0f);
		Vector3 LeftOffset = new Vector3 (-0.5f, -0.554f, 0.0f);
		Instantiate (Resources.Load ("DustCloud") as GameObject, transform.position + RightOffset, Quaternion.identity);
		Instantiate (Resources.Load ("ReverseDustCloud") as GameObject, transform.position + LeftOffset, Quaternion.identity);
	}
}
