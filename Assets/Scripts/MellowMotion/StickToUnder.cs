using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToUnder : MonoBehaviour {
	private Rigidbody2D rb;
	private Transform topLevelTransform;
	private GameObject current = null;
	private float previousX = 0.0f;

	void Start () {
		if (transform.parent != null && transform.parent.parent != null) {
			topLevelTransform = transform.parent.parent;
			rb = topLevelTransform.gameObject.GetComponent<Rigidbody2D> ();
		}
	}

	void Update() {
		if (rb != null && rb.gameObject.activeSelf && rb.velocity.x == 0.0f && current != null) {
			if (previousX != current.transform.position.x) {
				float difference = current.transform.position.x - previousX;
				topLevelTransform.position += Vector3.right * difference;
			}
			previousX = current.transform.position.x;
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (current == null && other.gameObject.CompareTag("Ground") && other.gameObject.GetComponentInParent<AutoObjectTranslate> () != null) {
			current = other.gameObject;
			previousX = current.transform.position.x;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (current == other.gameObject) {
			previousX = 0.0f;
			current = null;
		}
	}
}
