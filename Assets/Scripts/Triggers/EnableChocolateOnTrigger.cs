using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableChocolateOnTrigger : MonoBehaviour {
	private GameObject chocolate;
	private GameObject mellow;
	public UnityAction registeredMellow;
	public UnityAction lostMellow;

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
		mellow.GetComponent<MellowStates> ().DisableMovementInput ();
		mellow.GetComponentInParent<StateMachineForJack> ().SetState (StateMachineForJack.State.Disabled);
	}
}
