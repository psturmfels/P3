using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateHelpOnPlayerEnter : MonoBehaviour {
	public float timeBetweenRepeatedHelp = 15.0f;
	public float timeBeforeFirstHelp = 8.0f; 
	public bool delayBeforeHelp = true;
	private bool shouldHelp = true;
	private bool neverSeen = true;

	private HelperIndicatorMove him;

	void Start() {
		foreach (HelperIndicatorMove childHim in GetComponentsInChildren<HelperIndicatorMove> ()) {
			if (childHim.gameObject.GetComponent<TriggerHelpAfterHelp> () == null) {
				him = childHim;
				break;
			}
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			
			if (neverSeen && delayBeforeHelp) {
				neverSeen = false;
				RunResetHelpSequence (timeBeforeFirstHelp);
			}

			else if (shouldHelp) {
				RunResetHelpSequence (timeBetweenRepeatedHelp);
				him.StartMoveRoutine ();
			}
		}
	}

	void RunResetHelpSequence(float waitTime) {
		StartCoroutine (ResetHelp (waitTime));
	}

	IEnumerator ResetHelp(float waitTime) {
		shouldHelp = false;
		yield return new WaitForSeconds (waitTime);
		shouldHelp = true;
	}
}
