﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpointer : MonoBehaviour {

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

    public void ResetToCheckpoint() {
        this.transform.SetPositionAndRotation(CheckpointPos, Quaternion.identity);
    }

    public void ResetToBeginning() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
