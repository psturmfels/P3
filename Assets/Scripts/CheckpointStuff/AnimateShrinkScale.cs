using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateShrinkScale : MonoBehaviour {
	private SpriteRenderer goldSR;
	private SpriteRenderer chocolateSR;
	private Vector3 originalScale;
	private Vector3 reducedScale;
	private float eps = 0.005f;
	private float maxAnimateSpeed = 0.01f;
	private float minAnimateSpeed = 0.15f;


	void Start () {
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer> ()) {
			if (sr.name == "ChocolateSprite") {
				chocolateSR = sr;
			} else if (sr.name == "GoldSprite") {
				goldSR = sr;
			}
		}
		originalScale = transform.localScale;
		reducedScale = new Vector3 (0.0f, transform.localScale.y, transform.localScale.z);
	}

	public void StartAnimation() {
		StartCoroutine (ChocolateToGold (originalScale, reducedScale, 12, minAnimateSpeed, maxAnimateSpeed));
	}

	IEnumerator ChocolateToGold(Vector3 originalScale, Vector3 targetScale, int numTimes, float startSpeed, float endSpeed) {
		float animateSpeed = startSpeed;
		float colorIncrement = 1.0f / (float)(numTimes / 2);
		for (int i = 0; i < numTimes; ++i) {
			while (Vector3.SqrMagnitude (transform.localScale - targetScale) > eps) {
				transform.localScale = Vector3.MoveTowards (transform.localScale, targetScale, animateSpeed);
				yield return null;
			}
			transform.localScale = targetScale;

			if (i < numTimes / 2) {
				goldSR.color = new Color (goldSR.color.r, goldSR.color.b, goldSR.color.b, goldSR.color.a + colorIncrement);
				chocolateSR.color = new Color (chocolateSR.color.r, chocolateSR.color.b, chocolateSR.color.b, chocolateSR.color.a - colorIncrement);
			}

			while (Vector3.SqrMagnitude (transform.localScale - originalScale) > eps) {
				transform.localScale = Vector3.MoveTowards (transform.localScale, originalScale, animateSpeed);
				yield return null;
			}
			transform.localScale = originalScale;
			animateSpeed += (endSpeed - startSpeed) / (float)numTimes;
		}
	}
}
