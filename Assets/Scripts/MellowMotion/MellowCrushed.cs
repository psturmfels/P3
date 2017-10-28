using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class MellowCrushed : MonoBehaviour {
	public Sprite[] crushSprites;
	public float timeBetweenCrushSprites;
	public GameObject deathExplosion;

	private float removeDelay;
	private MoveAnimate ma;
	private MellowStates ms;
    private PlayerActions controls;
    private PlayerDeviceManager deviceManager;
    private int playerID = 0;

    public void Die() {
		if (GetComponent<Rigidbody2D> () != null) {
			GetComponent<Rigidbody2D> ().simulated = false;
		}
		if (GetComponent<BoxCollider2D> () != null) {
			GetComponent<BoxCollider2D> ().enabled = false;
		}
		if (GetComponent<PickUpAction> () != null) {
			GetComponent<PickUpAction> ().DropItem ();
		}
 		ms.SetState (MellowStates.State.Dead, true);
		ma.InterruptMovementAnimation (crushSprites, timeBetweenCrushSprites);
		Invoke ("RemoveSelf", removeDelay);
	}

	void Start () {
		ma = GetComponent<MoveAnimate> ();
		ms = GetComponent<MellowStates> ();
		removeDelay = (crushSprites.Length - 1) * timeBetweenCrushSprites;

        //Find PlayerDeviceManager
        deviceManager = GameObject.Find("PlayerDeviceManager").GetComponent<PlayerDeviceManager>();

        //Grab playerID for controller purposes.
        if(ms) {
            playerID = ms.playerID;
        }
    }

	void Update () {
        //Find the controls bound to this player
        if((deviceManager != null) && (controls == null))
        {
            controls = deviceManager.GetControls(playerID);
        }

        if(controls != null)
        {
            if(controls == null)
            {
                Die();
            }
        }
	}

	void RemoveSelf() {
		if (deathExplosion != null) {
			Instantiate (deathExplosion, transform.position, Quaternion.identity);
		}
		Destroy (gameObject);
	}
}
