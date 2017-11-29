using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHelpAfterHelp : MonoBehaviour {
	public HelperIndicatorMove otherMove;
	public bool onBegin = true;
	private HelperIndicatorMove thisMove;

	void Start () {
		thisMove = GetComponent<HelperIndicatorMove> ();
		if (otherMove != null) {
			if (onBegin) {
				otherMove.BeganHelp += TriggerHelp;
			} else {
				otherMove.ReachedFinalPosition += TriggerHelp;
			}
		}
	}

	void TriggerHelp() {
		if (thisMove != null) {
			thisMove.StartMoveRoutine ();
		}
	}
}
