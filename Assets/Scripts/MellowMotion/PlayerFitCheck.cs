using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFitCheck : MonoBehaviour {
    private int numOfCollisions = 0;
    private bool playerFits = true;

    public bool playerCanFit() {
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
