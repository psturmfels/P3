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

	private GameObject cameraCanvas;
	private GameObject stiltCamera;
	private GameObject bridgeCamera;

	private Vector3 lastStiltPosition;
	private Vector3 lastBridgePosition;
	private float startLerpWaitTime = 1.0f;
	private float smoothSpeed = 1.5f;
	private float upOffset = 1.0f;
	private bool trackPlayers = true;
	private bool resizeCamera = true;
	private bool isLerping = false;
	private float eps = 3.0f;
	private float lerpSpeed = 0.03f;
	private float viewportHalf = 0.43f;
	private float viewportMax = 1.02f;
	private float viewportMin = -0.02f;


	void Start() {
		mainCamera = GetComponent<Camera> ();
		if (cameraCanvas == null) {
			cameraCanvas = Instantiate (Resources.Load ("CameraCanvas") as GameObject);
			foreach (Transform child in cameraCanvas.transform) {
				if (child.gameObject.name == "StiltCamera") {
					stiltCamera = child.gameObject;
					stiltCamera.SetActive (false);
				} else if (child.gameObject.name == "BridgeCamera") {
					bridgeCamera = child.gameObject;
					bridgeCamera.SetActive (false);
				}
			}
		}
	}

	void SetLastPositions() {
		if (stiltTransform.gameObject.activeSelf) {
			lastStiltPosition = stiltTransform.position;
		}
		if (bridgeTransform.gameObject.activeSelf) {
			lastBridgePosition = bridgeTransform.position;
		}
	}

	void SetCameraPos() {
		Vector3 middle = (lastBridgePosition + lastStiltPosition) * 0.5f;

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
		float width = Mathf.Abs (lastStiltPosition.x - lastBridgePosition.x) * 0.5f + playerTwiceOffset;
		float height = Mathf.Abs (lastStiltPosition.y - lastBridgePosition.y) * 0.5f + playerTwiceOffset;

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


	void SetBubbles() {
		Vector3 stiltCameraPos = Camera.main.WorldToViewportPoint (lastStiltPosition);
		Vector3 bridgeCameraPos = Camera.main.WorldToViewportPoint (lastBridgePosition);

//		Debug.Log (stiltCameraPos.ToString ());
		if (stiltCameraPos.x < viewportMin ||
			stiltCameraPos.x > viewportMax ||
			stiltCameraPos.y > viewportMax ||
			stiltCameraPos.y < viewportMin) {

			if (stiltCamera != null) {
				stiltCamera.SetActive (true);
			}


			if (stiltCameraPos.y > stiltCameraPos.x && stiltCameraPos.y > 1.0f - stiltCameraPos.x || 
				stiltCameraPos.y < stiltCameraPos.x && stiltCameraPos.y < 1.0f - stiltCameraPos.x) {
				Vector3 transformedCirclePos = new Vector3 (stiltCameraPos.x, 0.5f + viewportHalf * Mathf.Sign(stiltCameraPos.y), 0);
				stiltCamera.GetComponent<RectTransform>().anchorMin = new Vector2(transformedCirclePos.x, transformedCirclePos.y);
				stiltCamera.GetComponent<RectTransform>().anchorMax = new Vector2(transformedCirclePos.x, transformedCirclePos.y);
			} else {
				Vector3 transformedCirclePos = new Vector3 (0.5f + viewportHalf * Mathf.Sign (stiltCameraPos.x), stiltCameraPos.y, 0);
				stiltCamera.GetComponent<RectTransform>().anchorMin = new Vector2(transformedCirclePos.x, transformedCirclePos.y);
				stiltCamera.GetComponent<RectTransform>().anchorMax = new Vector2(transformedCirclePos.x, transformedCirclePos.y);			
			}
		} else {
			if (stiltCamera != null) {
				stiltCamera.SetActive (false);
			}
		}


		if (bridgeCameraPos.x < viewportMin ||
			bridgeCameraPos.x > viewportMax ||
			bridgeCameraPos.y > viewportMax ||
			bridgeCameraPos.y < viewportMin) {

			if (bridgeCamera != null) {
				bridgeCamera.SetActive (true);
			}

			if (bridgeCameraPos.y > bridgeCameraPos.x && bridgeCameraPos.y > 1.0f - bridgeCameraPos.x || 
				bridgeCameraPos.y < bridgeCameraPos.x && bridgeCameraPos.y < 1.0f - bridgeCameraPos.x) {
				Vector3 transformedCirclePos = new Vector3 (bridgeCameraPos.x, 0.5f + viewportHalf * Mathf.Sign(bridgeCameraPos.y), 0);
				bridgeCamera.GetComponent<RectTransform>().anchorMin = new Vector2(transformedCirclePos.x, transformedCirclePos.y);
				bridgeCamera.GetComponent<RectTransform>().anchorMax = new Vector2(transformedCirclePos.x, transformedCirclePos.y);
			} else {
				Vector3 transformedCirclePos = new Vector3 (0.5f + viewportHalf * Mathf.Sign(bridgeCameraPos.x), bridgeCameraPos.y, 0);
				bridgeCamera.GetComponent<RectTransform>().anchorMin = new Vector2(transformedCirclePos.x, transformedCirclePos.y);
				bridgeCamera.GetComponent<RectTransform>().anchorMax = new Vector2(transformedCirclePos.x, transformedCirclePos.y);
			}
		} else {
			if (bridgeCamera != null) {
				bridgeCamera.SetActive (false);
			}
		}
	}

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

		SetBubbles ();


//		ClampPlayers ();
	}
}
