using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MellowCrushed : MonoBehaviour {
	public KeyCode selfDestructKey;
	public Sprite[] crushSprites;
	public float timeBetweenCrushSprites;
	public GameObject deathExplosion;

	private float removeDelay;
	private MoveAnimate ma;
	private MellowStates ms;

	public void Die() {
		if (GetComponent<Rigidbody2D> () != null) {
			GetComponent<Rigidbody2D> ().simulated = false;
		}
		if (GetComponent<BoxCollider2D> () != null) {
			GetComponent<BoxCollider2D> ().enabled = false;
		}
		ms.SetState (MellowStates.State.Dead, true);
		ma.InterruptMovementAnimation (crushSprites, timeBetweenCrushSprites);
		Invoke ("RemoveSelf", removeDelay);
	}

	void Start () {
		ma = GetComponent<MoveAnimate> ();
		ms = GetComponent<MellowStates> ();
		removeDelay = (crushSprites.Length - 1) * timeBetweenCrushSprites;
	}

	void Update () {
		if (Input.GetKeyDown (selfDestructKey) && !ms.isDead) {
			Die ();
		}
	}

	void RemoveSelf() {
		if (deathExplosion != null) {
			Instantiate (deathExplosion, transform.position, Quaternion.identity);
		}
		Destroy (gameObject);
	}
}
