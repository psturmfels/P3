using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CycleFadeSprites : MonoBehaviour {
	public Sprite[] sprites;
	public float timeBetweenSprites;
	public float alphaFadeRate;
	public float scaleJumpRate;
	public Vector3 endScale;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		StartCoroutine (CycleThroughSprites ());
	}
	
	IEnumerator CycleThroughSprites() {
		for (int i = 0; i < sprites.Length; ++i) {
			sr.sprite = sprites [i];

			Color updatedColor = sr.color;
			updatedColor.a = Mathf.MoveTowards (updatedColor.a, 0.0f, alphaFadeRate);
			sr.color = updatedColor;

			transform.localScale = Vector3.MoveTowards (transform.localScale, endScale, scaleJumpRate);

			yield return new WaitForSeconds (timeBetweenSprites);
		}

		while (sr.color.a > 0.0 || transform.localScale != endScale) {
			Color updatedColor = sr.color;
			updatedColor.a = Mathf.MoveTowards (updatedColor.a, 0.0f, alphaFadeRate);
			sr.color = updatedColor;

			transform.localScale = Vector3.MoveTowards (transform.localScale, endScale, scaleJumpRate);
			yield return null;
		}
			
		Destroy (gameObject);
	}
}
