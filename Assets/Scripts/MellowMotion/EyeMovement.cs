using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour {

    private Vector3 originalPosition;
    private float dispFactor = 70f;
    private float eyeMoveSpeed = 10f;

	// Use this for initialization
	void Start () {
	    originalPosition = transform.localPosition;
	}

    public void LookAt(Vector3 direction) {
        if (direction.y < 0)
            direction.y = 0;
        transform.localPosition = originalPosition + direction/dispFactor;
    }

    public void Translate(Vector3 position) {
        StartCoroutine(LerpToTransformPosition(position));
//        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + position, Time.deltaTime);
//        transform.localPosition = originalPosition + position;
    }

    IEnumerator LerpToTransformPosition(Vector3 position) {
        while (transform.localPosition != position) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition + position, eyeMoveSpeed);
            yield return null;
        }
//        ReachedTransform();
    }
}
