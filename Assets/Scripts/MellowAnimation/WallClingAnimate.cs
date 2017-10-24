using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClingAnimate : MonoBehaviour {
	public Sprite wallClingLeftSprite;
	public Sprite wallClingRightSprite;

	private MoveAnimate ma;

	void Start() {
		ma = GetComponent<MoveAnimate> ();
	}

	public void StartWallClingRight() {
		ma.OverrideMovementAnimation (wallClingRightSprite);
	}

	public void StartWallClingLeft() {
		ma.OverrideMovementAnimation (wallClingLeftSprite);
	}

	public void StopWallCling() {
		ma.ReturnMovementAnimation ();
	}
}
