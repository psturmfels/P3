using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathOnContact : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
		if ((other.GetComponent<MellowStates> () != null || other.GetComponent <TransformBehavior> () != null)
			&& other.gameObject.GetComponentInParent<MellowCrushed>() != null)
		{
			other.gameObject.GetComponentInParent<MellowCrushed>().StartDie();
        }
    }
}
