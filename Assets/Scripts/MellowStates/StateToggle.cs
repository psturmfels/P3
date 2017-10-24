using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateToggle : MonoBehaviour {
	public MellowStates.State toggleState;
	public bool OnTriggerEnable;
	public bool RegisterStay;

	private MellowStates ms;

	void Start () {
		ms = GetComponentInParent<MellowStates> ();
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
			ms.SetState (toggleState, !OnTriggerEnable);
		}
	}
}
