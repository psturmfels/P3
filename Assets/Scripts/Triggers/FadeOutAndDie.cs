using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAndDie : MonoBehaviour {
	public bool justFadeOut = false;

	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private BoxCollider2D bc;
	private float fadeOutSpeed = 0.03f;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		bc = GetComponent<BoxCollider2D> (); 
	}

	public void StartFadeOut() {
		if (justFadeOut) {
			StartCoroutine (FadeOut ());
		} else {
			if (bc != null) {
				bc.enabled = false;
			}
			rb.isKinematic = false;
			rb.gravityScale = 1.0f;
			rb.AddForce (Random.Range (-3.0f, 3.0f) * Vector2.right, ForceMode2D.Impulse);
			rb.angularVelocity = Random.Range (-90.0f, 90.0f);
			StartCoroutine (FadeOutFancy ());
		}
	}

	IEnumerator FadeOutFancy() {
		while (sr.color.a > 0.0f) {
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, Mathf.Max (0.0f, sr.color.a - fadeOutSpeed));
			yield return null;
		}
		if (transform.parent != null && transform.parent.gameObject.activeSelf && GetComponent<Door> () == null) {
			Destroy (transform.parent.gameObject);
		}
		Destroy (gameObject);
	}

	IEnumerator FadeOut() {
		while (sr.color.a > 0.0f) {
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, Mathf.Max (0.0f, sr.color.a - fadeOutSpeed));
			yield return null;
		}
		Destroy (gameObject);
	}

}
