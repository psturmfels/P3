using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class Checkpointer : MonoBehaviour {
    public GameObject bridgeMellow;
    public GameObject stiltMellow;
	public CameraMovement cm;

    Vector3 CheckpointPos;
    PlayerActions controllerActions;
    PlayerActions keyboardActions;

	// Use this for initialization
	void Start () {
        CheckpointPos = this.transform.position;
        controllerActions = PlayerActions.CreateWithControllerBindings();
        keyboardActions = PlayerActions.CreateWithKeyboardBindings();

		if (bridgeMellow.GetComponent<MellowCrushed> () != null) {
			bridgeMellow.GetComponent<MellowCrushed> ().Respawn += ResetBridgeToCheckpoint;
			bridgeMellow.GetComponent<MellowCrushed> ().Remove += RegisterMellowRemoved;
		}
		if (stiltMellow.GetComponent<MellowCrushed> () != null) {
			stiltMellow.GetComponent<MellowCrushed> ().Respawn += ResetStiltToCheckpoint;
			stiltMellow.GetComponent<MellowCrushed> ().Remove += RegisterMellowRemoved;
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (controllerActions.ResetCheckpoint.WasPressed || keyboardActions.ResetCheckpoint.WasPressed) {
			ResetStiltToCheckpoint ();
			ResetBridgeToCheckpoint ();
        }

        if (controllerActions.ResetLevel.WasPressed || keyboardActions.ResetLevel.WasPressed) {
            ResetToBeginning();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint") {
            CheckpointPos = collision.transform.position;
        }
    }

    public void SetCheckpoint(Vector3 pos) {
        CheckpointPos = pos;
    }

	void RegisterMellowRemoved() {
		cm.RegisterMellowRemoved (CheckpointPos);
	}

    void ResetStiltToCheckpoint() {
		stiltMellow.transform.position = CheckpointPos - new Vector3(1,0,0);
    }

	void ResetBridgeToCheckpoint() {
		bridgeMellow.transform.position = CheckpointPos + new Vector3(1,0,0);
	}


    public void ResetToBeginning() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
