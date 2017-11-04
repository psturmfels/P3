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
		Dead, 
		PickUp,
		Transformed
	};

	public bool canMove = true;
	public bool canJump = false;
	public bool canWallJumpLeft = false;
	public bool canWallJumpRight = false;
	public bool shouldStillMovement = true;
	public bool canPickup = true;
	public bool isDead = false;
	public bool isTransformed = false;

	public bool leftSideInContact = false;
	public bool rightSideInContact = false;

    public int playerID = 0;

	private WallClingAnimate wca;

	void Start() {
		wca = GetComponent<WallClingAnimate> ();
	}
		
	public void DisableMovementInput() {
		canMove = false;
		canJump = false;
		canWallJumpLeft = false;
		canWallJumpRight = false;
		canPickup = false;
	}

	public void EnableMovementInput() {
		canMove = true;
		canPickup = true;
	}

	public void SetState(State SwitchState, bool newValue) {
		if (isDead && SwitchState != State.Dead) {
			return;
		}
		if (isTransformed && SwitchState != State.Transformed) {
			return;
		}

		switch (SwitchState) {
		case State.Move:
			canMove = newValue;
			break;

		case State.Jump:
			canJump = newValue;
			if (canJump) {
				canWallJumpLeft = false;
				canWallJumpRight = false;
				wca.StopWallCling ();
			}
			break;

		case State.WallJumpLeft:
			rightSideInContact = newValue;
			if (canJump && newValue) {
				break;
			}
			canWallJumpLeft = newValue;
			if (canWallJumpLeft) {
				wca.StartWallClingRight ();
			} else {
				wca.StopWallCling ();
			}
			break;

		case State.WallJumpRight:
			leftSideInContact = newValue;
			if (canJump && newValue) {
				break;
			}
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
				DisableMovementInput ();
			}
			break;

		case State.PickUp:
			canPickup = newValue;
			break;

		case State.Transformed:
			isTransformed = newValue;
			if (isTransformed) {
				DisableMovementInput ();
			} else {
				EnableMovementInput ();
			}
			break;
		}
	}
}
