using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnExtendedChocolateContact : MonoBehaviour {
	private MellowCrushed mc;
	public float waitTime;

	void Start () {
		mc = GetComponentInParent<MellowCrushed> ();
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log (other.gameObject.name);
		if (other.gameObject.GetComponentInParent<Door> () != null) {
			StartCoroutine (KillPlayer (waitTime));
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponentInParent<Door> () != null) {
			StopAllCoroutines ();
		}
	}

	IEnumerator KillPlayer(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		mc.StartDie ();
	}
}
