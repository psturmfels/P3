using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrap : MonoBehaviour {

    public float activeTime = 2.0f;
    public float inactiveTime = 2.0f;
    public GameObject flames;

    private float shrinkDuration = 0.15f;
    private BoxCollider2D bc2d;

	// Use this for initialization
	void Start () {
	    bc2d = GetComponent<BoxCollider2D>();
        Invoke("Activate", inactiveTime);
	}

    private void Activate() {
        bc2d.enabled = true;
        StartCoroutine(Shrink(false));
        Invoke("Deactivate", activeTime);
    }

    private void Deactivate() {
        bc2d.enabled = false;
        StartCoroutine(Shrink(true));
        Invoke("Activate", inactiveTime);
    }

    IEnumerator Shrink(bool shrinking)
    {
        float start = Time.time;
        float elapsed = 0;
        while (elapsed < shrinkDuration)
        {
            // calculate how far through we are
            elapsed = Time.time - start;
            float normalisedTime = Mathf.Clamp(elapsed / shrinkDuration, 0, 1);
            if (shrinking) {
                flames.transform.localScale = new Vector3(1 - normalisedTime, 1 - normalisedTime, 1);
            }
            else {
                flames.transform.localScale = new Vector3(normalisedTime, normalisedTime, 1);
            }

            // wait for the next frame
            yield return null;
        }
    }


}
