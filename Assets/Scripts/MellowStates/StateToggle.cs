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
		ij.DidJump += StartCycleStayBehavior;

		InputWallJump iwj = GetComponentInParent<InputWallJump> ();
		iwj.DidWallJump += StartCycleStayBehavior;
	}
		
	void StartCycleStayBehavior() {
		ExitAssignedState ();
		StopAllCoroutines ();
		StartCoroutine (CycleStayBehavior ());
	}
		
	IEnumerator CycleStayBehavior() {
		RegisterStay = false;
		yield return new WaitForSeconds (0.3f);
		RegisterStay = true;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (ms != null) {
			SetAssignedState ();
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (ms != null && RegisterStay) {
			SetAssignedState ();
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
}
