using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectColliderInside : MonoBehaviour {
	public Vector3 rejectVector;
	private float eps = 0.1f;

	void OnTriggerEnter2D(Collider2D other) {
		RejectOther (other.gameObject);
	}

	void OnTriggerStay2D(Collider2D other) {
		RejectOther (other.gameObject);
	}

	void RejectOther(GameObject other) {
		if (other.CompareTag ("Ground") && transform.parent != null) {
			Vector3 difference = transform.parent.position - other.transform.position;
			transform.parent.position += Vector3.Dot(difference.normalized, rejectVector) * eps * rejectVector;
		}
	}
}
