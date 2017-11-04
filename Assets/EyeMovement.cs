using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour {

    public GameObject mellowMove;
    public GameObject mellowTransformed;
    public Transform iris;

    private Rigidbody2D rb;
    private bool transformed = false;

	// Use this for initialization
	void Start () {
	    rb = GetComponentInParent<Rigidbody2D>();
        Invoke("Blink", 4.0f + Random.Range(0f, 4f));
    }
	
	// Update is called once per frame
	void Update () {
	    if (mellowMove.activeSelf) {
	        iris.position = (Vector2) transform.position + rb.velocity.normalized/15f;
	    }
	    else {
	        Vector3 targetMellow = mellowMove.name == "BridgeMellowMove" ? 
                GameObject.Find("StiltMellow").transform.position : 
                GameObject.Find("BridgeMellow").transform.position;
	        Vector3 irisRelativePos = targetMellow - transform.position;
            iris.position = transform.position + irisRelativePos.normalized / 15f;
        }
    }
    
    private void Blink() {
        Debug.Log("Blinkink " + gameObject.name + " " + Time.time);
        GetComponentInParent<Animator>().SetTrigger("blink");
        Invoke("Blink", 4.0f + Random.Range(0f, 4f));
    }
}
