using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputJump : MonoBehaviour {
	public float jumpForce;
	public Sprite[] positiveJumpSprites;
	public Sprite[] negativeJumpSprites;
	public float timeBetweenJumpSprites;

	private float jumpForceModifier = 1.0f;
	private float jumpDelay = 0.0f;
	private Rigidbody2D rb;
	private MoveAnimate ma;
	private InputMove im;
	private MellowStates ms;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private int playerID = 0;

    public void StartJump(float forceModifier = 1.0f) {
		if (!ms.canJump || ms.canWallJumpLeft || ms.canWallJumpRight) {
			return;
		}

		ms.canJump = false;

		jumpForceModifier = forceModifier;
		if (im.GetCurrentFaceDirection () > 0.0f) {
			ma.InterruptMovementAnimation (positiveJumpSprites, timeBetweenJumpSprites);
		} else {
			ma.InterruptMovementAnimation (negativeJumpSprites, timeBetweenJumpSprites);
		}
		Invoke ("Jump", jumpDelay);
	}

	void Start () {
		ms = GetComponent<MellowStates> ();
		rb = GetComponent<Rigidbody2D> ();
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
		
	void Jump() {
		rb.velocity = new Vector2 (rb.velocity.x, 0.0f);
		rb.AddRelativeForce (Vector2.up * jumpForce * jumpForceModifier, ForceMode2D.Impulse);	
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
