using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCamera : MonoBehaviour {
	public float moveFraction = 1.0f;

	private Camera mainCamera;
	private Vector2 previousCameraPosition;

	void Start () {
		mainCamera = Camera.main;	
		previousCameraPosition = mainCamera.transform.position;
	}

	void Update () {
		if (mainCamera != null) {
			Vector2 currentCameraPosition = new Vector2 (mainCamera.transform.position.x, mainCamera.transform.position.y);
			Vector2 difference = currentCameraPosition - previousCameraPosition;
			Vector3 targetPosition = transform.position + new Vector3 (difference.x, difference.y, 0.0f);
			transform.position = Vector3.Lerp (transform.position, targetPosition, moveFraction);
			previousCameraPosition = currentCameraPosition;
		}
	}
}
