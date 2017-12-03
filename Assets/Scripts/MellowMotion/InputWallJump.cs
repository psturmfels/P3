using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using InControl;

public class InputWallJump : MonoBehaviour {
	public Vector2 jumpVector;
	public Sprite[] positiveJumpSprites;
	public Sprite[] negativeJumpSprites;
	public float timeBetweenJumpSprites;

	public UnityAction DidWallJump;

    public AudioSource wallJumpSound;
	private float jumpForceModifier = 1.0f;
	private float jumpNoStillDuration = 0.25f;
	private float jumpLockMovementDuration = 0.15f;
	private MellowStates ms;
	private Rigidbody2D rb;
	private MoveAnimate ma;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
	private InputJump jumpScript;
    private int playerID = 0;
	private bool shouldDampenFrames = false;
	private int framesCountedDampen = 0;
	private int framesDampenTotal = 7;
	private bool isJumping = false;
	private Vector2 dampenForce = Vector2.down * 50.0f;
	private bool jumpWasPressed = false;
	private bool ignoreNextFrame = true;
	private int inputBufferFrames = 8;
	private int inputFramesCounted = 0;
	private float lastJumpForceModifier = 1.0f;

    public void StartJump(float forceModifier = 1.0f) {
		if (ms.canJump) {
			return;
		}
		if (ms.canWallJumpLeft) {
			ma.InterruptMovementAnimation (negativeJumpSprites, timeBetweenJumpSprites);
			jumpForceModifier = -forceModifier;
			dampenForce = Vector2.down * 50.0f + Vector2.right * 50.0f;
		} else if (ms.canWallJumpRight) {
			ma.InterruptMovementAnimation (positiveJumpSprites, timeBetweenJumpSprites);
			jumpForceModifier = forceModifier;
			dampenForce = Vector2.down * 50.0f + Vector2.left * 50.0f;
		} else {
			return;
		}

		inputFramesCounted = 0;
		jumpWasPressed = false;
		lastJumpForceModifier = jumpForceModifier;

		CancelInvoke ();
		DidWallJump ();
		shouldDampenFrames = true;

		framesCountedDampen = 0; 
		isJumping = true;

		ms.SetState (MellowStates.State.WallJumpLeft, false);
		ms.SetState (MellowStates.State.WallJumpRight, false);
		ms.SetState (MellowStates.State.Jump, false);

		ms.SetState (MellowStates.State.StillMovement, false);
		ms.SetState (MellowStates.State.Move, false);

		Jump ();
		Invoke ("BeginStillMovement", jumpNoStillDuration);
		Invoke ("BeginMovement", jumpLockMovementDuration);
	}

	void Start () {
		ms = GetComponent<MellowStates> ();
		rb = GetComponentInParent<Rigidbody2D> ();
		ma = GetComponent<MoveAnimate> ();
		jumpScript = GetComponent<InputJump> ();
		jumpScript.DidJump += ResetBufferFrames;

		if (wallJumpSound == null)
        {
            if (this.transform.parent.name == "BridgeMellow")
            {
                wallJumpSound = GameObject.Find("GameCamera").transform.Find("SFX").Find("BridgeJump").GetComponent<AudioSource>();
            }
            else
            {
                wallJumpSound = GameObject.Find("GameCamera").transform.Find("SFX").Find("StiltJump").GetComponent<AudioSource>();
            }
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
        if (deviceManager != null)
        {
            controls = deviceManager.GetControls(playerID);
        }

        if(controls != null)
        {
			if (shouldDampenFrames) {
				if (!controls.Jump.IsPressed) {
					if (dampenForce.normalized != Vector2.up || Mathf.Sign (rb.velocity.x) == Mathf.Sign (lastJumpForceModifier)) {
						rb.AddRelativeForce (dampenForce, ForceMode2D.Force);
					}
				}
				framesCountedDampen += 1;
				if (framesCountedDampen >= framesDampenTotal) {
					shouldDampenFrames = false;
				}
			}
			if(controls.Jump.WasPressed && !ignoreNextFrame)
			{
				jumpWasPressed = true;
			}
			ignoreNextFrame = false;
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

	void ResetBufferFrames() {
		ignoreNextFrame = true;
		jumpWasPressed = false;
		inputFramesCounted = 0;
	}

	void FixedUpdate() {
		if (isJumping && rb.velocity.y < 0.1f) {
			isJumping = false;
			framesCountedDampen = 0; 
			dampenForce = Vector2.up * 15.0f;
			shouldDampenFrames = true;
		}
	}

	void Jump() {
		rb.velocity = Vector2.zero;
		Vector2 modifiedJumpVector = new Vector2 (jumpVector.x * jumpForceModifier, jumpVector.y);
		rb.AddRelativeForce (modifiedJumpVector, ForceMode2D.Impulse);
        wallJumpSound.Play();
	}

	void BeginStillMovement() {
		ms.SetState (MellowStates.State.StillMovement, true);
	}

	void BeginMovement() {
		ms.SetState (MellowStates.State.Move, true);
	}
}
