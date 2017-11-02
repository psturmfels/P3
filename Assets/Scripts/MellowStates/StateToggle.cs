using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateToggle : MonoBehaviour {
	public MellowStates.State toggleState;
	public bool OnTriggerEnable;
	public bool RegisterStay;
	public bool DebugCollisions = false;

	private MellowStates ms;

	void Start () {
		ms = GetComponentInParent<MellowStates> ();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (ms != null) {
			ms.SetState (toggleState, OnTriggerEnable);
		}
		if (DebugCollisions) {
			Debug.Log ("Entered trigger with: " + other.gameObject.name);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (ms != null && RegisterStay) {
			ms.SetState (toggleState, OnTriggerEnable);
		}
		if (DebugCollisions) {
			Debug.Log ("Stayed trigger with: " + other.gameObject.name);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (ms != null) {
			ms.SetState (toggleState, !OnTriggerEnable);
		}
		if (DebugCollisions) {
			Debug.Log ("Left trigger with: " + other.gameObject.name);
		}
	}
}
