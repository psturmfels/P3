using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectColliderInside : MonoBehaviour {
	public Vector3 rejectVector;
	private BoxCollider2D parentCollider;
	private Vector3 oppositeVector;
	private TransformBehavior tb;

	void Awake() {
		parentCollider = transform.parent.GetComponent<BoxCollider2D> ();
		tb = transform.parent.GetComponent<TransformBehavior> (); 
	}

	void OnTriggerEnter2D(Collider2D other) {
		RejectOther (other.gameObject);
	}

	void OnTriggerStay2D(Collider2D other) {
		RejectOther (other.gameObject);
	}

	void RejectOther(GameObject other) {
		if (!tb.IsTransforming ()) {
			return;
		}
		if (other.CompareTag ("Ground") && transform.parent != null && parentCollider != null) {
			if (other.GetComponent<SpriteRenderer> () != null) {
				float otherSize = Vector3.Dot (other.GetComponent<SpriteRenderer> ().bounds.size, rejectVector);
				float parentSize = Vector3.Dot (parentCollider.bounds.size, rejectVector);
				float difference = Vector3.Dot  (transform.parent.position - other.transform.position, rejectVector);
				float moveUpAbs = (otherSize + parentSize) * 0.5f - Mathf.Abs (difference);
				transform.parent.position +=  moveUpAbs * rejectVector * Mathf.Sign(difference);
			}
		}
	}
}
