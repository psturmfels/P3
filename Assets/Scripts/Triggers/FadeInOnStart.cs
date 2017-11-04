using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnStart : MonoBehaviour {
	private SpriteRenderer sr; 
	private float fadeInSpeed = 0.05f;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();	
		StartCoroutine (FadeIn ());
	}
	
	IEnumerator FadeIn() {
		while (sr.color.a < 1.0f) {
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, Mathf.Min (sr.color.a + fadeInSpeed, 1.0f));
			yield return null;
		}
	}
}
