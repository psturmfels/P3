using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateToggle : MonoBehaviour {
	public MellowStates.State toggleState;
	public bool OnTriggerEnable;
	public float DelayBeforeExit = 0.0f;
	private float timeSinceLastJump = 1.0f;
	private float jumpTimeWait = 0.3f;

	private MellowStates ms;
	private bool isJumper = false;

	void Start () {
		ms = GetComponentInParent<MellowStates> ();
		if (toggleState == MellowStates.State.Jump ||
		    toggleState == MellowStates.State.WallJumpLeft ||
		    toggleState == MellowStates.State.WallJumpRight) {
			isJumper = true;
			InputJump ij = GetComponentInParent<InputJump> ();
			ij.DidJump += SetTimeSinceLastJumpToZero;
			InputWallJump iwj = GetComponentInParent<InputWallJump> ();
			iwj.DidWallJump += SetTimeSinceLastJumpToZero;
		}
	}
		
	void SetTimeSinceLastJumpToZero() {
		timeSinceLastJump = 0.0f;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (ms != null) {
			SetAssignedState ();
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (ms != null) {
			if ((isJumper && timeSinceLastJump > jumpTimeWait) || !isJumper) {
				SetAssignedState ();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (ms != null) {
			if (DelayBeforeExit > 0.0f) {
				Invoke ("ExitAssignedState", DelayBeforeExit);
			} else {
				ExitAssignedState ();
			}
		}
	}

	void ExitAssignedState() {
		ms.SetState (toggleState, !OnTriggerEnable);
	}

	void SetAssignedState() {
		CancelInvoke ();
		ms.SetState (toggleState, OnTriggerEnable);
	}

	void Update() {
		timeSinceLastJump += Time.deltaTime;
	}
}
