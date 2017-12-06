using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputMove : MonoBehaviour {
	private Rigidbody2D rb;
	private MellowStates ms;
    private PlayerDeviceManager deviceManager;
	private float currentHorzAxis;
	private float previousHorzAxis;
	private float currentFaceDirection;
    private int playerID = 0;
    private PlayerActions controls;
	private float deadZone = 0.2f;
	private float currentMaxSpeed;
    private Animator anim;

	public float moveIncrement;
	public float maxMoveSpeed;

	public void SetCurrentHorzAxis(float newHorzAxis) {
		if (newHorzAxis != 0.0f) {
			currentFaceDirection = newHorzAxis;
		}

		if (!ms.canMove) {
			currentHorzAxis = 0.0f; 
			return;
		}

		previousHorzAxis = currentHorzAxis;
		currentHorzAxis = newHorzAxis;
	}

	public float GetCurrentHorzAxis() {
		return currentHorzAxis;
	}

	public float GetCurrentHorzSpeed() {
		return rb.velocity.x;
	}

	public float GetCurrentFaceDirection() {
		return currentFaceDirection;
	}

	void Start () {
		ms = GetComponent<MellowStates> ();
		rb = GetComponentInParent<Rigidbody2D> ();
        anim = GetComponentInParent<Animator>();

        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();

        //Grab playerID for controller purposes.
        if(ms) {
            playerID = ms.playerID;
        }

		currentMaxSpeed = maxMoveSpeed;
	}
	
	void Update () {
        //Find the controls bound to this player
        if (deviceManager != null) {
            controls = deviceManager.GetControls(playerID);
        }

		if (controls != null && Mathf.Abs(controls.Move.X) > deadZone && ms.canMove) {
			float currentInputModifier = Mathf.Min (1.0f, (Mathf.Abs(controls.Move.X) + deadZone));
			currentMaxSpeed = maxMoveSpeed * currentInputModifier;
			SetCurrentHorzAxis (Mathf.Sign(controls.Move.X));
		    if (anim != null) {
                anim.SetBool("walking", true);
            }
        } else {
			SetCurrentHorzAxis (0.0f);
		    if (anim != null) {
                anim.SetBool("walking", false);
            }
        }
	}

	void FixedUpdate() {
		if ((previousHorzAxis != currentHorzAxis || currentHorzAxis == 0.0f) && ms.shouldStillMovement) {
			rb.velocity = new Vector2 (0.0f, rb.velocity.y);
		}

		if (ms.canMove) {
			if (currentHorzAxis > 0.0f) {
				rb.velocity = new Vector2 (Mathf.Min (rb.velocity.x + moveIncrement, currentMaxSpeed), rb.velocity.y);
			} else if (currentHorzAxis < 0.0f) {
				rb.velocity = new Vector2 (Mathf.Max (rb.velocity.x - moveIncrement, -currentMaxSpeed), rb.velocity.y);
			}
		}
	}
}
