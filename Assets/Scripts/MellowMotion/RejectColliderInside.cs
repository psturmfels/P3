using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectColliderInside : MonoBehaviour {
	public Vector3 rejectVector;
	private BoxCollider2D parentCollider;
	private Vector3 oppositeVector;
	private TransformBehavior tb;
	private Transform grandparentTransform;
	private int maxIterations = 6;

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
				Bounds otherBounds = other.GetComponent<BoxCollider2D> ().bounds;
				while (otherBounds.Intersects (parentCollider.bounds) && numIterations < maxIterations) {
					grandparentTransform.position += 0.1f * rejectVector;
					numIterations += 1;
				}
			} else if (other.GetComponent<PolygonCollider2D> () != null) {
				PolygonCollider2D otherColl = other.GetComponent<PolygonCollider2D> ();
				float sizeOffset = 0.5f * Vector3.Dot (parentCollider.bounds.size, rejectVector);
				Vector2 parentExtrema = grandparentTransform.position - Mathf.Abs (sizeOffset) * rejectVector; 
				while (otherColl.OverlapPoint (parentExtrema) && numIterations < maxIterations) {
					grandparentTransform.position += 0.1f * rejectVector;
					sizeOffset = 0.5f * Vector3.Dot (parentCollider.bounds.size, rejectVector);
					parentExtrema = grandparentTransform.position + Mathf.Abs (sizeOffset) * rejectVector;  
					numIterations += 1; 
				}
			}
		}
	}
}
