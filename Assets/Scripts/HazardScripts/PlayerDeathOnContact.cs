using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathOnContact : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.CompareTag("Player") && other.gameObject.GetComponentInParent<MellowCrushed>() != null)
		{
            other.gameObject.GetComponentInParent<MellowCrushed>().StartDie();
        }
    }
}
