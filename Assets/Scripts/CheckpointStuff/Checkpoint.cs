using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour {
    public static GameObject bridgeMellow;
    public static GameObject stiltMellow;
    public static CameraMovement cm;
	public static List<Checkpoint> currentCheckpoints = new List<Checkpoint> ();

	public Vector3 stiltOffset = new Vector3 (-1.0f, 0.0f, 0.0f);
	public Vector3 bridgeOffset = new Vector3 (1.0f, 0.0f, 0.0f);
	public bool isActive = false;

    private static Vector3 CheckpointPos;
    private static PlayerActions controllerActions;
    private static PlayerActions keyboardActions;

    private bool bridgeAtCheckpoint = false;
    private bool stiltAtCheckpoint = false;
	private AnimateShrinkScale bridgeCoin;
	private AnimateShrinkScale stiltCoin;


    // Use this for initialization
    void Start () {
		if (gameObject.name == "SceneLoader") {
			isActive = true;
		}
		currentCheckpoints.Add (this);

		foreach (AnimateShrinkScale coin in GetComponentsInChildren<AnimateShrinkScale> ()) {
			if (coin.gameObject.name == "CheckpointBridge") {
				bridgeCoin = coin;
			} else if (coin.gameObject.name == "CheckpointStilt") {
				stiltCoin = coin;
			}
		}

        bridgeMellow = GameObject.Find("BridgeMellow");
        stiltMellow = GameObject.Find("StiltMellow");
        CheckpointPos = bridgeMellow.transform.position + Vector3.left;
        cm = Camera.main.gameObject.GetComponent<CameraMovement>();

        controllerActions = PlayerActions.CreateWithControllerBindings();
        keyboardActions = PlayerActions.CreateWithKeyboardBindings();

		if (bridgeMellow.GetComponentInChildren<MellowCrushed>() != null) {
            bridgeMellow.GetComponentInChildren<MellowCrushed>().Respawn += ResetBridgeToCheckpoint;
			bridgeMellow.GetComponentInChildren<MellowCrushed>().Remove += RegisterMellowRemoved;
        }
		if (stiltMellow.GetComponentInChildren<MellowCrushed>() != null) {
			stiltMellow.GetComponentInChildren<MellowCrushed>().Respawn += ResetStiltToCheckpoint;
			stiltMellow.GetComponentInChildren<MellowCrushed>().Remove += RegisterMellowRemoved;
        }
    }
    // Update is called once per frame
    void Update() {
		if (!isActive) {
			return;
		}
        if (controllerActions.ResetCheckpoint.WasPressed || keyboardActions.ResetCheckpoint.WasPressed) {
            ResetStiltToCheckpoint();
            ResetBridgeToCheckpoint();
        }

        if (controllerActions.ResetLevel.WasPressed || keyboardActions.ResetLevel.WasPressed) {
            ResetToBeginning();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
		if (isActive) {
			return;
		}

		if (!bridgeAtCheckpoint && collision.gameObject.name.Contains("Bridge")) {
            bridgeAtCheckpoint = true;
			bridgeCoin.StartAnimation ();
            if (stiltAtCheckpoint) {
				DisableAllOtherCheckpoints ();
				isActive = true;
                SetCheckpoint(transform.position);
            }
        }
		else if (!stiltAtCheckpoint && collision.gameObject.name.Contains("Stilt")) {
            stiltAtCheckpoint = true;
			stiltCoin.StartAnimation ();
            if (bridgeAtCheckpoint) {
				DisableAllOtherCheckpoints ();
				isActive = true;
                SetCheckpoint(transform.position);
            }
        }
    }

	void DisableAllOtherCheckpoints () {
		foreach (Checkpoint check in currentCheckpoints) {
			if (check != this) {
				check.isActive = false;
			}
		}
	}

    public void SetCheckpoint(Vector3 pos) {
		if (!isActive) {
			return;
		}
        CheckpointPos = pos;
    }

    void RegisterMellowRemoved() {
		if (!isActive) {
			return;
		}
        cm.RegisterMellowRemoved(CheckpointPos);
    }

    void ResetStiltToCheckpoint() {
		if (!isActive) {
			return;
		}
		stiltMellow.transform.position = CheckpointPos + stiltOffset;
    }

    void ResetBridgeToCheckpoint() {
		if (!isActive) {
			return;
		}
		bridgeMellow.transform.position = CheckpointPos + bridgeOffset;
    }

    public void ResetToBeginning() {
		if (!isActive) {
			return;
		}
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
