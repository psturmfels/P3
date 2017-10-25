using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {
	public Transform target;
	public bool shouldFollowTarget = true;

	void Update () {
		if (target != null && shouldFollowTarget) {
			transform.position = new Vector3 (target.position.x, target.position.y, transform.position.z);
		}
	}
}
