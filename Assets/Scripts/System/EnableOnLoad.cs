using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnLoad : MonoBehaviour {
    public GameObject[] objects;

	// Use this for initialization
	void Start () {
	    foreach (var obj in objects) {
	        obj.SetActive(true);
	    }
	}
}
