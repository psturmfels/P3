using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFitCheck : MonoBehaviour {

//    private BoxCollider2D bc;
    private int numOfCollisions = 0;
    private bool playerFits = true;

	// Use this for initialization
	void Start () {
//	    bc = GetComponent<BoxCollider2D>();
	}

    public bool playerCanFit() {
//        if (numOfCollisions != 0) {
//            Debug.Log(gameObject.name + "is in ground");
//        }
        return numOfCollisions == 0;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            numOfCollisions++;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            numOfCollisions--;
        }
    }
}
