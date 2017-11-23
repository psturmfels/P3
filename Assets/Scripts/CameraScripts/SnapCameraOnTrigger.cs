using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCameraOnTrigger : MonoBehaviour {
	public bool snapX = true;
	public bool snapY = false;
	private bool bridgeTriggered = false;
	private bool stiltTriggered = false;
	private bool isTriggered = false;
	
	private void OnTriggerEnter2D(Collider2D collision) {
		if (isTriggered) {
			return;
		}
		if (!bridgeTriggered && collision.gameObject.name.Contains("Bridge")) {
			bridgeTriggered = true;
			if (stiltTriggered) {
				if (snapX) {
					SnapCameraX ();
				}
				if (snapY) {
					SnapCameraY ();
				}
				isTriggered = true;
			}
		}
		else if (!stiltTriggered && collision.gameObject.name.Contains("Stilt")) {
			stiltTriggered = true;
			if (bridgeTriggered) {
				if (snapX) {
					SnapCameraX ();
				}
				if (snapY) {
					SnapCameraY ();
				}
				isTriggered = true;
			}
		}
	}

	void SnapCameraY() {
		if (isTriggered) {
			return;
		}
		if (Camera.main != null && Camera.main.gameObject.GetComponent<CameraMovement> () != null) {
			CameraMovement cm = Camera.main.gameObject.GetComponent<CameraMovement> ();
			cm.SetNewFixedYPosition (transform.position.y);
		}
	}

	void SnapCameraX() {
		if (isTriggered) {
			return;
		}
		if (Camera.main != null && Camera.main.gameObject.GetComponent<CameraMovement> () != null) {
			CameraMovement cm = Camera.main.gameObject.GetComponent<CameraMovement> ();
			cm.SetNewFixedXPosition (transform.position.x);
		}
	}
}
