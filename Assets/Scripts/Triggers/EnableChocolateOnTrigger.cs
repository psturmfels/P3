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
	private Vector3 mellowCrushedOffset = new Vector3 (0.0f, -0.15f, 0.0f);
	private Vector2 targetMellowColliderSize = new Vector2 (0.37f, 0.45f);
	public string mellowName = "";
	private float chocolateFallWaitTime = 0.5f;

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
		StartCoroutine (MakeSmore ());
	}

	IEnumerator MakeSmore() {
		yield return new WaitForSeconds (chocolateFallWaitTime);
		if (mellow.GetComponentInParent<MellowCrushed> () == null || mellow.GetComponent<MoveAnimate> () == null) {
			yield break;
		}

		Sprite[] crushSprites = mellow.GetComponentInParent<MellowCrushed> ().crushSprites;
		float timeBetweenSprites = mellow.GetComponentInParent<MellowCrushed> ().timeBetweenCrushSprites;
		mellow.GetComponent<MoveAnimate> ().InterruptMovementAnimation (crushSprites, timeBetweenSprites, false);

		if (mellow == null || mellow.transform.parent == null || mellow.GetComponent<BoxCollider2D> () == null) {
			yield break;
		}
		mellow.GetComponent<BoxCollider2D> ().size = targetMellowColliderSize;
		Vector3 targetPosition = mellow.transform.parent.position + mellowCrushedOffset;
		float distance = Vector3.Distance(mellow.transform.parent.position, targetPosition);
		float updateStep = distance / crushSprites.Length;
		while (mellow.transform.parent.position != targetPosition) {
			mellow.transform.parent.position = Vector3.MoveTowards(mellow.transform.parent.position, targetPosition, updateStep);
			yield return new WaitForSeconds(timeBetweenSprites);
		}
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
