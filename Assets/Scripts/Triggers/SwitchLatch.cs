using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLatch : MonoBehaviour {

    public GameObject TriggeredObject;

    Renderer render;
    Color activeColor = Color.cyan;

	// Use this for initialization
	void Start () {
        render = this.GetComponent<Renderer>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TriggeredObject.SetActive(false);
        render.material.color = activeColor;
    }
}
