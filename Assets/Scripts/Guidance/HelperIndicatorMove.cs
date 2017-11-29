using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HelperIndicatorMove : MonoBehaviour {
	public GameObject helperTrailPrefab;
	public GameObject[] movePositionIndicators;
	public float speed;

	public UnityAction ReachedFinalPosition;
	public UnityAction BeganHelp;
	private SpriteRenderer sr;
	private Vector3 originalPosition; 

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		sr.enabled = false;
		originalPosition = transform.position;
	}

	public void StartMoveRoutine() {
		StopAllCoroutines ();
		StartCoroutine (MoveOverPositions (movePositionIndicators));
	}

	IEnumerator MoveOverPositions(GameObject[] targetObjects) {
		if (BeganHelp != null) {
			BeganHelp ();
		}

		GameObject helperTrail = Instantiate (helperTrailPrefab, transform.position, Quaternion.identity);
		helperTrail.transform.SetParent (transform);

		int index = 0;
		while (index < targetObjects.Length) {
			if (targetObjects [index] != null) {
				Vector3 targetPosition = targetObjects [index].transform.position;
				targetPosition.z = 0.0f;

				if (targetObjects [index].GetComponent<HelperMotionModifier> () != null) {
					HelperMotionModifier hmm = targetObjects [index].GetComponent<HelperMotionModifier> ();

					if (hmm.useParabolicMotion) {
						float x1 = transform.position.x;
						float y1 = transform.position.y;
						float x2 = targetPosition.x;
						float y2 = targetPosition.y;

						float[] coeffs = hmm.CoefficientsFromPoints (x1, x2, y1, y2);
						float A = coeffs [0];
						float B = coeffs [1];
						float C = coeffs [2];

						if (hmm.clampParabolaHalf) {
							Vector2 thirdPosition = hmm.GetThirdPoint (x1, x2, y1, y2);

							float speedFraction = speed * Mathf.Sqrt(Mathf.Pow(transform.position.x - thirdPosition.x, 2.0f)) / Vector3.Distance (transform.position, thirdPosition);
							while (transform.position.x != thirdPosition.x) {
								float movedX = Mathf.MoveTowards (transform.position.x, thirdPosition.x, speedFraction);
								float calculatedY = A * movedX * movedX + B * movedX + C;
								transform.position = new Vector3 (movedX, calculatedY, transform.position.z);
								yield return null;
							}

						} else {
							float speedFraction = speed * Mathf.Sqrt(Mathf.Pow(transform.position.x - targetPosition.x, 2.0f)) / Vector3.Distance (transform.position, targetPosition);
							while (transform.position.x != targetPosition.x) {
								float movedX = Mathf.MoveTowards (transform.position.x, targetPosition.x, speedFraction);
								float calculatedY = A * movedX * movedX + B * movedX + C;
								transform.position = new Vector3 (movedX, calculatedY, transform.position.z);
								yield return null;
							}
						}
					} else {
						while (transform.position != targetPosition) {
							transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed);
							yield return null;
						}
					}
				} else {
					while (transform.position != targetPosition) {
						transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed);
						yield return null;
					}
				}

			}
			index += 1;
		}

		if (ReachedFinalPosition != null) {
			ReachedFinalPosition ();
		}

		while (helperTrail != null) {	
			yield return null;
		}
		sr.enabled = false;
		transform.position = originalPosition;
	} 
}
