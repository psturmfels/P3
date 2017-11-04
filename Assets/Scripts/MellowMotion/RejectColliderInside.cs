using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectColliderInside : MonoBehaviour {
	public Vector3 rejectVector;
	private BoxCollider2D parentCollider;
	private Vector3 oppositeVector;
	private TransformBehavior tb;
	private Transform grandparentTransform;

	void Awake() {
		parentCollider = transform.parent.GetComponent<BoxCollider2D> ();
		tb = transform.parent.GetComponent<TransformBehavior> (); 
		grandparentTransform = transform.parent.parent;
	}

	void OnTriggerEnter2D(Collider2D other) {
		RejectOther (other.gameObject);
	}

	void OnTriggerStay2D(Collider2D other) {
		RejectOther (other.gameObject);
	}

	void RejectOther(GameObject other) {
		if (tb == null || grandparentTransform == null || parentCollider == null) {
			return;
		}
		if (!tb.IsTransforming ()) {
			return;
		}
		if (other.CompareTag ("Ground") && transform.parent != null && parentCollider != null) {
			float otherSize = 1.0f;
			if (other.GetComponent<SpriteRenderer> () != null) {
				otherSize = Vector3.Dot (other.GetComponent<SpriteRenderer> ().bounds.size, rejectVector);
			} else if (other.GetComponent<BoxCollider2D> () != null) {
				otherSize = Vector3.Dot (other.GetComponent<BoxCollider2D> ().bounds.size, rejectVector);
			}
			float parentSize = Vector3.Dot (parentCollider.bounds.size, rejectVector);
			float difference = Vector3.Dot  (grandparentTransform.position - other.transform.position, rejectVector);
			float moveUpAbs = (otherSize + parentSize) * 0.5f - Mathf.Abs (difference);
			grandparentTransform.position +=  moveUpAbs * rejectVector * Mathf.Sign(difference);

		}
	}
}
