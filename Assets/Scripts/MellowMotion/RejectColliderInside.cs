using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectColliderInside : MonoBehaviour {
	public Vector3 rejectVector;
	private BoxCollider2D parentCollider;
	private Vector3 oppositeVector;
	private TransformBehavior tb;
	private Transform grandparentTransform;
	private int maxIterations = 3;

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
		if ((other.CompareTag ("Ground") || other.CompareTag("Player")) && 
			transform.parent != null && parentCollider != null) {
			grandparentTransform.position += 0.1f * rejectVector;

			int numIterations = 0;
			if (other.GetComponent<BoxCollider2D> () != null) {
				BoxCollider2D otherColl = other.GetComponent<BoxCollider2D> ();
				while (otherColl.IsTouching(parentCollider) && numIterations < maxIterations) {					
					grandparentTransform.position += 0.1f * rejectVector;
					numIterations += 1;
				}
			} else if (other.GetComponent<PolygonCollider2D> () != null) {
				PolygonCollider2D otherColl = other.GetComponent<PolygonCollider2D> ();
				while (otherColl.IsTouching(parentCollider) && numIterations < maxIterations) {
					grandparentTransform.position += 0.1f * rejectVector;
					numIterations += 1; 
				}
			}
		}
	}
}
