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
	private float jumpDelay = 0.0f;
	private Rigidbody2D rb;
	private MoveAnimate ma;
	private InputMove im;
	private MellowStates ms;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private int playerID = 0;
	private bool shouldCountFrames = false;
	private int framesCountedTotal = 0;
	private int framesCountedInput = 0;

    public void StartJump(float forceModifier = 1.0f) {
		if (!ms.canJump || ms.canWallJumpLeft || ms.canWallJumpRight) {
			return;
		}
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

		shouldCountFrames = true;
		DidJump ();
		yield return new WaitForSeconds (jumpDelay);
		if (ms.isDead) {
			yield break;
		}

		float frameInputRatio = (float)(Mathf.Min(framesCountedInput + 1, framesCountedTotal) + 1) / (float)(framesCountedTotal + 1);
		rb.velocity = new Vector2 (rb.velocity.x, 0.0f);
		rb.AddRelativeForce (Vector2.up * jumpForce * jumpForceModifier * frameInputRatio, ForceMode2D.Impulse);
		shouldCountFrames = false;
		framesCountedInput = 0;
		framesCountedTotal = 0;

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
		jumpDelay = timeBetweenJumpSprites * positiveJumpSprites.Length;

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
}
