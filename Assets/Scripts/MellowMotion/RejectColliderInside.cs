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
		if (Mathf.Abs (rejectVector.x) > 0.0f) {
			tb.LerpFromRejectInDirection (rejectVector.x * 0.45f);
		} else if (Mathf.Abs (rejectVector.y) > 0.0f) {
			tb.LerpFromRejectInDirection (rejectVector.y * 0.45f);
		}
	}
}
