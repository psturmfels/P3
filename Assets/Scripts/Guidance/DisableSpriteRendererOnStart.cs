using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSpriteRendererOnStart : MonoBehaviour {
	void Start () {
		if (GetComponent<SpriteRenderer> () != null) {
			GetComponent<SpriteRenderer> ().enabled = false;
		}
		if (GetComponentInChildren<MeshRenderer> () != null) {
			GetComponentInChildren<MeshRenderer> ().enabled = false;
		}
	}
}
