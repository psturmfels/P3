using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateHelpOnPlayerEnter : MonoBehaviour {
	public float timeBetweenRepeatedHelp = 15.0f;
	public bool shouldHelp = true;

	private HelperIndicatorMove him;

	void Start() {
		him = GetComponentInChildren<HelperIndicatorMove> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Player") && shouldHelp) {
			RunResetHelpSequence ();
			him.StartMoveRoutine ();
		}
	}

	void RunResetHelpSequence() {
		StartCoroutine (ResetHelp ());
	}

	IEnumerator ResetHelp() {
		shouldHelp = false;
		yield return new WaitForSeconds (timeBetweenRepeatedHelp);
		shouldHelp = true;
	}
}
