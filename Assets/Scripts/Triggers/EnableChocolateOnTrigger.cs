using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableChocolateOnTrigger : MonoBehaviour {
	private GameObject chocolate;
	private GameObject mellow;
	public UnityAction registeredMellow;
	public UnityAction lostMellow;

	private Vector3 mellowTargetOffset = new Vector3 (0.0f, 0.71f, 0.0f);
	public string mellowName = "";

	void Start () {
		foreach (Transform child in transform) {
			if (child.gameObject.name == "Chocolate") {
				chocolate = child.gameObject;
				break;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "BridgeMellowMove" || other.gameObject.name == "StiltMellowMove") {
			if (mellowName != "") {
				lostMellow ();
			}

			mellowName = other.gameObject.name;
			mellow = other.gameObject;
			registeredMellow ();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.name == "BridgeMellowMove" || other.gameObject.name == "StiltMellowMove") {
			mellowName = "";
			lostMellow ();
		}
	}

	public void EnableChocolate() {
		chocolate.SetActive (true);
		mellow.GetComponent<MellowStates> ().SetState (MellowStates.State.Dead, true);
		mellow.GetComponentInParent<StateMachineForJack> ().SetState (StateMachineForJack.State.Disabled);
		mellow.GetComponentInParent<StateMachineForJack> ().doesAcceptInput = false;
		StartCoroutine (LerpMellowToPosition ());
	}

	IEnumerator LerpMellowToPosition () {
		if (mellow == null || mellow.transform.parent == null) {
			yield break;
		}
		Vector3 targetPosition = transform.position + mellowTargetOffset;
		targetPosition.z = mellow.transform.parent.position.z;

		while (mellow.transform.parent.position != targetPosition) {
			mellow.transform.parent.position = Vector3.MoveTowards (mellow.transform.parent.position, targetPosition, 0.1f);
			yield return null;
		}
	}
}
