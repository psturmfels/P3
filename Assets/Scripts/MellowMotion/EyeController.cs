using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour {
    public GameObject mellowMove;
    public EyeMovement leftEye;
    public EyeMovement rightEye;

    private MellowCrushed mellowCrushed;
    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private bool transformed = false;

    // Use this for initialization
    void Start() {
        rb = GetComponentInParent<Rigidbody2D>();
        Invoke("Blink", 4.0f + Random.Range(0f, 4f));
        mellowCrushed = mellowMove.GetComponentInParent<MellowCrushed>();
        mellowCrushed.Remove += DisableEyes;
        mellowCrushed.Respawn += EnableEyes;
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update() {
        Vector3 lookingDirection = transform.position;
        if (mellowMove.activeSelf) {
            transform.localPosition = originalPosition;
            lookingDirection = rb.velocity;
        }
        else {
            Vector3 targetMellow = Vector3.zero;
            if (mellowMove.name == "BridgeMellowMove") {
                targetMellow = GameObject.Find("StiltMellow").transform.position;
            }
            else {
                targetMellow = GameObject.Find("BridgeMellow").transform.position;
            }
            lookingDirection = targetMellow - transform.position;
        }
        leftEye.LookAt(lookingDirection);
        rightEye.LookAt(lookingDirection);
    }

    private void Blink() {
        GetComponent<Animator>().SetTrigger("blink");
        Invoke("Blink", 4.0f + Random.Range(0f, 4f));
    }

    private void DisableEyes() {
        gameObject.SetActive(false);
    }

    private void EnableEyes() {
        gameObject.SetActive(true);
    }
}
