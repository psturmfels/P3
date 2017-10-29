using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpointer : MonoBehaviour {

    public GameObject bridgeMello;
    public GameObject stiltMello;

    Vector3 CheckpointPos;

	// Use this for initialization
	void Start () {
        CheckpointPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            ResetToCheckpoint();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
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
    }

    public void ResetToBeginning() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
