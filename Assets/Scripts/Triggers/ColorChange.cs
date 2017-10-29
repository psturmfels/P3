using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour {

    Checkpointer checkpointer;

    Color activeColor = Color.green;

    private void Awake()
    {
        var checkpointerObj = GameObject.Find("Checkpointer");
        if (checkpointerObj == null) {
            Debug.LogError("Checkpointer not found!");
        }

        checkpointer = checkpointerObj.GetComponent<Checkpointer>();
        if (checkpointer == null)
        {
            Debug.LogError("Checkpointer not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            this.GetComponent<Renderer>().material.color = activeColor;
            checkpointer.SetCheckpoint(this.transform.position);
        }
    }
}
