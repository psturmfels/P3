using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelTransformOnCollision : MonoBehaviour {
	private TransformBehavior tranBeh;
	public int cancelID;

	void Awake () {
		tranBeh = GetComponentInParent<TransformBehavior> ();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		tranBeh.RegisterCancelContact (cancelID);
	}
}
