using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMovement : MonoBehaviour {
	public Transform stiltTransform;
	public Transform bridgeTransform;
	public event UnityAction reachedCheckpoint;
	public float minSizeY = 5.0f;
	public float maxSizeY = 7f;

	private Camera mainCamera;
	private float playerTwiceOffset = 3.0f;

	private Vector3 lastPlayer1Position;
	private Vector3 lastPlayer2Position;
	private float startLerpWaitTime = 1.0f;
	private float smoothSpeed = 1.5f;
	private float upOffset = 1.0f;
	private bool trackPlayers = true;
	private bool resizeCamera = true;
	private bool isLerping = false;
	private float eps = 3.0f;
	private float lerpSpeed = 0.03f;

	void Start() {
		mainCamera = GetComponent<Camera> ();
	}

	void SetLastPositions() {
		if (stiltTransform.gameObject.activeSelf) {
			lastPlayer1Position = stiltTransform.position;
		}
		if (bridgeTransform.gameObject.activeSelf) {
			lastPlayer2Position = bridgeTransform.position;
		}
	}

	void SetCameraPos() {
		Vector3 middle = (lastPlayer1Position + lastPlayer2Position) * 0.5f;

		Vector3 targetCameraPosition = new Vector3(
			middle.x,
			middle.y + upOffset,
			mainCamera.transform.position.z
		);

		mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, smoothSpeed*Time.deltaTime);
	}

	void SetCameraSize() {
		//horizontal size is based on actual screen ratio
		float minSizeX = minSizeY * Screen.width / Screen.height;

		//multiplying by 0.5, because the ortographicSize is actually half the height
		float width = Mathf.Abs (lastPlayer1Position.x - lastPlayer2Position.x) * 0.5f + playerTwiceOffset;
		float height = Mathf.Abs (lastPlayer1Position.y - lastPlayer2Position.y) * 0.5f + playerTwiceOffset;

		//computing the size
		float camSizeX = Mathf.Max(width, minSizeX);
		mainCamera.orthographicSize = Mathf.Clamp (Mathf.Max (height,   camSizeX * Screen.height / Screen.width, minSizeY), minSizeY, maxSizeY);
	}

	public void RegisterMellowRemoved (Vector3 checkpointPosition) {
		if (isLerping) {
			return;
		}
		StopAllCoroutines ();
		Vector2 checkpointTargetPos = new Vector3 (checkpointPosition.x, checkpointPosition.y); 
		StartCoroutine (LerpToTargetPosition (checkpointTargetPos));
	}

	IEnumerator LerpToTargetPosition(Vector2 targetPosition) {
		trackPlayers = false;
		resizeCamera = false;
		isLerping = true;
		yield return new WaitForSeconds (startLerpWaitTime);

		Vector2 cameraPos = new Vector2 (transform.position.x, transform.position.y);
		
		int iterationNumber = 1;
		while (Vector2.Distance (cameraPos, targetPosition) > eps || mainCamera.orthographicSize > minSizeY) {
			Vector2 newPosition = Vector2.Lerp (cameraPos, targetPosition, lerpSpeed);
			cameraPos = newPosition;
			transform.position = new Vector3 (cameraPos.x, cameraPos.y, transform.position.z);


			if (iterationNumber > 30) {
				mainCamera.orthographicSize = minSizeY;
			} else {
				mainCamera.orthographicSize = mainCamera.orthographicSize * Mathf.Pow (0.95f, iterationNumber) + minSizeY * (1.0f - Mathf.Pow (0.95f, iterationNumber));
			}

			iterationNumber += 1;
			yield return null;
		}

		trackPlayers = true;
		resizeCamera = true;
		reachedCheckpoint ();

		isLerping = false;
	}

//	void ClampPlayers() {
//		Vector3 player1CameraPos = Camera.main.WorldToViewportPoint (player1.position);
//		player1CameraPos.x = Mathf.Clamp(player1CameraPos.x, 0.022f, 0.978f);
//		//player1CameraPos.y = Mathf.Clamp(player1CameraPos.y, 0.022f, 0.978f);
//		player1.position = Camera.main.ViewportToWorldPoint(player1CameraPos);
//
//		Vector3 player2CameraPos = Camera.main.WorldToViewportPoint (player2.position);
//		player2CameraPos.x = Mathf.Clamp(player2CameraPos.x, 0.022f, 0.978f);
//		//player2CameraPos.y = Mathf.Clamp(player2CameraPos.y, 0.022f, 0.978f);
//		player2.position = Camera.main.ViewportToWorldPoint(player2CameraPos);
//	}

	void Update() {
		if (stiltTransform == null || bridgeTransform == null) { 
			return;
		}
		SetLastPositions ();
		if (trackPlayers) {
			SetCameraPos ();
		} 
		if (resizeCamera) {
			SetCameraSize ();
		}

//		ClampPlayers ();
	}
}
