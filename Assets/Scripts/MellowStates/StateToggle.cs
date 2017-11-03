using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateToggle : MonoBehaviour {
	public MellowStates.State toggleState;
	public bool OnTriggerEnable;
	public bool RegisterStay;
	public float DelayBeforeExit = 0.0f;

	private MellowStates ms;

	void Start () {
		ms = GetComponentInParent<MellowStates> ();
		InputJump ij = GetComponentInParent<InputJump> ();
		ij.DidJump += DisableRegisterStay;
	}

	void DisableRegisterStay() {
		RegisterStay = false;
		Invoke ("EnableRegisterStay", 0.5f);
	}

	void EnableRegisterStay() {
		RegisterStay = true;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (ms != null) {
			ms.SetState (toggleState, OnTriggerEnable);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (ms != null && RegisterStay) {
			ms.SetState (toggleState, OnTriggerEnable);
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
}
