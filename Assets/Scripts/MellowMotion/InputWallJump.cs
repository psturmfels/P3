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
	private float jumpDelay = 0.0f;
	private MellowStates ms;
	private Rigidbody2D rb;
	private MoveAnimate ma;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
	private bool shouldCountFrames = false;
	private int framesCountedTotal = 0;
	private int framesCountedInput = 0;

    private int playerID = 0;

    public void StartJump(float forceModifier = 1.0f) {
		if (ms.canJump) {
			return;
		}
		if (ms.canWallJumpLeft) {
			ma.InterruptMovementAnimation (negativeJumpSprites, timeBetweenJumpSprites);
			jumpForceModifier = -forceModifier;
		} else if (ms.canWallJumpRight) {
			ma.InterruptMovementAnimation (positiveJumpSprites, timeBetweenJumpSprites);
			jumpForceModifier = forceModifier;
		} else {
			return;
		}
		CancelInvoke ();
		DidWallJump ();

		ms.SetState (MellowStates.State.WallJumpLeft, false);
		ms.SetState (MellowStates.State.WallJumpRight, false);
		ms.SetState (MellowStates.State.Jump, false);

		ms.SetState (MellowStates.State.StillMovement, false);
		ms.SetState (MellowStates.State.Move, false);


		shouldCountFrames = true;
		Invoke ("Jump", jumpDelay);
		Invoke ("BeginStillMovement", jumpNoStillDuration);
		Invoke ("BeginMovement", jumpLockMovementDuration);
	}

	void Start () {
		ms = GetComponent<MellowStates> ();
		rb = GetComponentInParent<Rigidbody2D> ();
		ma = GetComponent<MoveAnimate> ();
		jumpDelay = timeBetweenJumpSprites * positiveJumpSprites.Length;

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

	void FixedUpdate() {
		if (shouldCountFrames) {
			framesCountedTotal += 1;
			if (controls != null && controls.Jump.IsPressed) {
				framesCountedInput += 1;
			}
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
			if(controls.Jump.WasPressed)
            {
                StartJump();
            }
        }
            
	}

	void Jump() {
		float frameInputRatio = (float)(Mathf.Min(framesCountedInput + 1, framesCountedTotal) + 1) / (float)(framesCountedTotal + 1);
		rb.velocity = Vector2.zero;
		Vector2 modifiedJumpVector = new Vector2 (jumpVector.x * jumpForceModifier, jumpVector.y) * frameInputRatio;
		rb.AddRelativeForce (modifiedJumpVector, ForceMode2D.Impulse);
        wallJumpSound.Play();
        shouldCountFrames = false;
		framesCountedInput = 0;
		framesCountedTotal = 0;
	}

	void BeginStillMovement() {
		ms.SetState (MellowStates.State.StillMovement, true);
	}

	void BeginMovement() {
		ms.SetState (MellowStates.State.Move, true);
	}
}
