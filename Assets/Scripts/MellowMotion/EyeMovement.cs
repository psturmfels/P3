using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour {

    public GameObject mellowMove;
    public MellowCrushed mellowCrushed;
    public Transform iris;
    public Transform smallIris;

    private Rigidbody2D rb;
    private bool transformed = false;
    private float irisDisplacementFactor = 20f;

	// Use this for initialization
	void Start () {
	    rb = GetComponentInParent<Rigidbody2D>();
        Invoke("Blink", 4.0f + Random.Range(0f, 4f));
        mellowCrushed.Remove += DisableEyes;
        mellowCrushed.Respawn += EnableEyes;
    }
	
	// Update is called once per frame
	void Update () {
	    if (mellowMove.activeSelf) {
	        iris.position = (Vector2) transform.position + new Vector2(0.03f, 0.03f) + rb.velocity.normalized / irisDisplacementFactor;
            if (smallIris != null)
                smallIris.position = (Vector2)transform.position + new Vector2(-0.05f, -0.05f) - rb.velocity.normalized / irisDisplacementFactor;
        }
	    else {
	        Vector3 targetMellow = mellowMove.name == "BridgeMellowMove" ? 
                GameObject.Find("StiltMellow").transform.position : 
                GameObject.Find("BridgeMellow").transform.position;
	        Vector3 irisRelativePos = targetMellow - transform.position;
            iris.position = transform.position + irisRelativePos.normalized / irisDisplacementFactor;
//            smallIris.position = (Vector2)transform.position - rb.velocity.normalized / irisDisplacementFactor;
        }
    }
    
    private void Blink() {
        GetComponentInParent<Animator>().SetTrigger("blink");
        Invoke("Blink", 4.0f + Random.Range(0f, 4f));
    }

    private void DisableEyes() {
        gameObject.SetActive(false);
    }

    private void EnableEyes() {
        gameObject.SetActive(true);
    }
}
