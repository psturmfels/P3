using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToUnder : MonoBehaviour {
	private Rigidbody2D rb;
	private Transform topLevelTransform;
	public GameObject current = null;
	private int numberInContactWithCurrent = 0;
	private float previousX = 0.0f;
	private float eps = 0.1f;

	void Start () {
		if (transform.parent != null && transform.parent.parent != null) {
			topLevelTransform = transform.parent.parent;
			rb = topLevelTransform.gameObject.GetComponent<Rigidbody2D> ();
		}
	}

	void Update() {
		if (rb != null && rb.gameObject.activeSelf && current != null) {
			if (Mathf.Abs(rb.velocity.x) < eps) {
				if (previousX != current.transform.position.x) {
					float difference = current.transform.position.x - previousX;
					topLevelTransform.position += Vector3.right * difference;
				}
			}
			previousX = current.transform.position.x;
		} 
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (current == null && other.gameObject.CompareTag ("Ground") && other.gameObject.GetComponentInParent<OscillatingObjectTranslate> () != null) {
			current = other.gameObject.GetComponentInParent<OscillatingObjectTranslate> ().gameObject;
			previousX = current.transform.position.x;
			numberInContactWithCurrent = 1;
		} else if (other.gameObject.GetComponentInParent<OscillatingObjectTranslate> () != null && current == other.gameObject.GetComponentInParent<OscillatingObjectTranslate> ().gameObject) {
			numberInContactWithCurrent += 1;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.GetComponentInParent<OscillatingObjectTranslate> () != null && current == other.gameObject.GetComponentInParent<OscillatingObjectTranslate> ().gameObject) {
			numberInContactWithCurrent -= 1;
			if (numberInContactWithCurrent == 0) {
				previousX = 0.0f;
				current = null;
			}
		}
	}
}
