using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMove : MonoBehaviour {
	private Rigidbody2D rb;
	private MellowStates ms;
	private float currentHorzAxis;
	private float previousHorzAxis;
	private float currentFaceDirection;

	public string horzAxisName;
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
		rb = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		SetCurrentHorzAxis (Input.GetAxisRaw (horzAxisName));
	}

	void FixedUpdate() {
		if ((previousHorzAxis != currentHorzAxis || currentHorzAxis == 0.0f) && ms.shouldStillMovement) {
			rb.velocity = new Vector2 (0.0f, rb.velocity.y);
		}

		if (ms.canMove) {
			if (currentHorzAxis > 0.0f) {
				rb.velocity = new Vector2 (Mathf.Min (rb.velocity.x + moveIncrement, maxMoveSpeed), rb.velocity.y);
			} else if (currentHorzAxis < 0.0f) {
				rb.velocity = new Vector2 (Mathf.Max (rb.velocity.x - moveIncrement, -maxMoveSpeed), rb.velocity.y);
			}
		}
	}
}
