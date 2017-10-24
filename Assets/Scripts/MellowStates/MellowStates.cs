using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MellowStates : MonoBehaviour {
	public enum State {
		Move,
		Jump,
		WallJumpLeft,
		WallJumpRight,
		StillMovement,
		Dead
	};

	public bool canMove = true;
	public bool canJump = false;
	public bool canWallJumpLeft = false;
	public bool canWallJumpRight = false;
	public bool shouldStillMovement = true;
	public bool isDead = false;

	private WallClingAnimate wca;

	void Start() {
		wca = GetComponent<WallClingAnimate> ();
	}

	public void SetState(State SwitchState, bool newValue) {
		if (isDead) {
			return;
		}

		switch (SwitchState) {
		case State.Move:
			canMove = newValue;
			break;

		case State.Jump:
			canJump = newValue;
			break;

		case State.WallJumpLeft:
			canWallJumpLeft = newValue;
			if (canWallJumpLeft) {
				wca.StartWallClingRight ();
			} else {
				wca.StopWallCling ();
			}
			break;

		case State.WallJumpRight:
			canWallJumpRight = newValue;
			if (canWallJumpRight) {
				wca.StartWallClingLeft ();
			} else {
				wca.StopWallCling ();
			}
			break;

		case State.StillMovement:
			shouldStillMovement = newValue;
			break;

		case State.Dead:
			isDead = newValue;
			if (isDead) {
				canMove = false;
				canJump = false;
				canWallJumpLeft = false;
				canWallJumpRight = false;
				shouldStillMovement = false;
			}
			break;
		}
	}
}
