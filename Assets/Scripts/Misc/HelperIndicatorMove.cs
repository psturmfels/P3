using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperIndicatorMove : MonoBehaviour {
	public GameObject helperTrailPrefab;
	public GameObject[] movePositionIndicators;
	public float speed;

	void Start () {
		StartMoveRoutine (movePositionIndicators);
	}

	public void StartMoveRoutine(GameObject[] targetObjects) {
		StartCoroutine (MoveOverPositions (targetObjects));
	}

	IEnumerator MoveOverPositions(GameObject[] targetObjects) {
		GameObject helperTrail = Instantiate (helperTrailPrefab, transform.position, Quaternion.identity);
		helperTrail.transform.SetParent (transform);

		int index = 0;
		while (index < targetObjects.Length) {
			if (targetObjects [index] != null) {
				Vector3 targetPosition = targetObjects [index].transform.position;
				targetPosition.z = 0.0f;
				while (transform.position != targetPosition) {
					transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed);
					yield return null;
				}
			}
			index += 1;
		}

		while (helperTrail != null) {	
			yield return null;
		}

		Destroy (gameObject);
	}
}
