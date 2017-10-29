using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputWallJump : MonoBehaviour {
	public Vector2 jumpVector;
	public Sprite[] positiveJumpSprites;
	public Sprite[] negativeJumpSprites;
	public float timeBetweenJumpSprites;

	private float jumpForceModifier = 1.0f;
	private float jumpNoStillDuration = 0.25f;
	private float jumpLockMovementDuration = 0.15f;
	private float jumpDelay = 0.0f;
	private MellowStates ms;
	private Rigidbody2D rb;
	private MoveAnimate ma;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;

    private int playerID = 0;

    public void StartJump(float forceModifier = 1.0f) {
		if (ms.canWallJumpLeft) {
			ma.InterruptMovementAnimation (negativeJumpSprites, timeBetweenJumpSprites);
			jumpForceModifier = -forceModifier;
		} else if (ms.canWallJumpRight) {
			ma.InterruptMovementAnimation (positiveJumpSprites, timeBetweenJumpSprites);
			jumpForceModifier = forceModifier;
		} else {
			return;
		}

		ms.SetState (MellowStates.State.WallJumpLeft, false);
		ms.SetState (MellowStates.State.WallJumpRight, false);
		ms.SetState (MellowStates.State.Jump, false);

		ms.SetState (MellowStates.State.StillMovement, false);
		ms.SetState (MellowStates.State.Move, false);


		Invoke ("Jump", jumpDelay);
		Invoke ("BeginStillMovement", jumpNoStillDuration);
		Invoke ("BeginMovement", jumpLockMovementDuration);
	}

	void Start () {
		ms = GetComponent<MellowStates> ();
		rb = GetComponent<Rigidbody2D> ();
		ma = GetComponent<MoveAnimate> ();
		jumpDelay = timeBetweenJumpSprites * positiveJumpSprites.Length;

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
            if(controls.Jump.IsPressed)
            {
                StartJump();
            }
        }
            
	}

	void Jump() {
		rb.velocity = Vector2.zero;
		Vector2 modifiedJumpVector = new Vector2 (jumpVector.x * jumpForceModifier, jumpVector.y);
		rb.AddRelativeForce (modifiedJumpVector, ForceMode2D.Impulse);	
	}

	void BeginStillMovement() {
		ms.SetState (MellowStates.State.StillMovement, true);
	}

	void BeginMovement() {
		ms.SetState (MellowStates.State.Move, true);
	}
}
