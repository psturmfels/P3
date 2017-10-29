using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class Checkpointer : MonoBehaviour {

    public GameObject bridgeMello;
    public GameObject stiltMello;
    public Transform cameraTransform;

    Vector3 CheckpointPos;
    PlayerActions controllerActions;
    PlayerActions keyboardActions;

	// Use this for initialization
	void Start () {
        CheckpointPos = this.transform.position;
        controllerActions = PlayerActions.CreateWithControllerBindings();
        keyboardActions = PlayerActions.CreateWithKeyboardBindings();
	}
	
	// Update is called once per frame
	void Update () {
        if (controllerActions.ResetCheckpoint.WasPressed || keyboardActions.ResetCheckpoint.WasPressed) {
            ResetToCheckpoint();
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

    public void ResetToCheckpoint() {
        stiltMello.GetComponentInParent<StateMachineForJack>().TransitionToState(StateMachineForJack.State.Normal);
        bridgeMello.GetComponentInParent<StateMachineForJack>().TransitionToState(StateMachineForJack.State.Normal);
        stiltMello.transform.position = CheckpointPos + new Vector3(1,0,0);
        bridgeMello.transform.position = CheckpointPos - new Vector3(1,0,0);
        cameraTransform.position = CheckpointPos + new Vector3(0, 0, cameraTransform.position.z);
    }

    public void ResetToBeginning() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
