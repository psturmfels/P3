using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathOnContact : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
		if ((other.CompareTag("Player") || other.CompareTag("StiltCollider") || other.CompareTag("BridgeCollider"))
			&& other.gameObject.GetComponentInParent<MellowCrushed>() != null)
		{
            other.gameObject.GetComponentInParent<MellowCrushed>().StartDie();
        }
    }
}
