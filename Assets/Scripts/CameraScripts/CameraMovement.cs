using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	public Transform player1;
	public Transform player2;
	public Transform player1Transformed;
	public Transform player2Transformed;
	public float minSizeY = 5.0f;
	public float maxSizeY = 7f;

	private Camera mainCamera;
	private float playerTwiceOffset = 3.0f;

	private Vector3 lastPlayer1Position;
	private Vector3 lastPlayer2Position;

	void Start() {
		mainCamera = GetComponent<Camera> ();
	}

	void SetLastPositions() {
		if (player1.gameObject.activeSelf) {
			lastPlayer1Position = player1.position;
		} else if (player1Transformed.gameObject.activeSelf) {
			lastPlayer1Position = player1Transformed.position;
		}
		if (player2.gameObject.activeSelf) {
			lastPlayer2Position = player2.position;
		} else if (player2Transformed.gameObject.activeSelf) {
			lastPlayer2Position = player2Transformed.position;
		}
	}

	void SetCameraPos() {
		Vector3 middle = (lastPlayer1Position + lastPlayer2Position) * 0.5f;

		mainCamera.transform.position = new Vector3(
			middle.x,
			middle.y,
			mainCamera.transform.position.z
		);
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

	void ClampPlayers() {
		Vector3 player1CameraPos = Camera.main.WorldToViewportPoint (player1.position);
		player1CameraPos.x = Mathf.Clamp(player1CameraPos.x, 0.022f, 0.978f);
		player1CameraPos.y = Mathf.Clamp(player1CameraPos.y, 0.022f, 0.978f);
		player1.position = Camera.main.ViewportToWorldPoint(player1CameraPos);

		Vector3 player2CameraPos = Camera.main.WorldToViewportPoint (player2.position);
		player2CameraPos.x = Mathf.Clamp(player2CameraPos.x, 0.022f, 0.978f);
		player2CameraPos.y = Mathf.Clamp(player2CameraPos.y, 0.022f, 0.978f);
		player2.position = Camera.main.ViewportToWorldPoint(player2CameraPos);
	}

	void Update() {
		if (player1 == null || player2 == null) { 
			return;
		}
		SetLastPositions ();
		SetCameraPos ();
		SetCameraSize ();
		ClampPlayers ();
	}
}
