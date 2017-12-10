using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTextMeshUp : MonoBehaviour {
	public float alphaFadeRate;
	public float xMoveSpeed;
	public float scaleSpeed;

	private Vector3 finalScale = Vector3.one;
	private float finalXOffset = 2.0f;
	private TextMesh tm;

	void Start () {
		tm = GetComponent<TextMesh> ();
		StartCoroutine (FadeUpParabola ());
	}
	
	IEnumerator FadeUpParabola() {
		yield return new WaitForSeconds (1.0f); 
		float initialYPosition = transform.position.y;
		float initialXPosition = transform.position.x;
		float finalXPosition = transform.position.x + finalXOffset;

		while (tm.color.a != 0.0f) {
			Color newColor = tm.color;
			newColor.a = Mathf.MoveTowards (newColor.a, 0.0f, alphaFadeRate);
			tm.color = newColor;

			float adjustedX = Mathf.MoveTowards (transform.position.x, finalXPosition, xMoveSpeed);
			transform.position = new Vector3 (adjustedX, 2.0f * Mathf.Pow(initialXPosition - adjustedX, 2.0f) + initialYPosition, transform.position.z);

			transform.localScale = Vector3.Lerp (transform.localScale, finalScale, scaleSpeed);
			yield return null;
		}

		Destroy (gameObject);
	}
}
