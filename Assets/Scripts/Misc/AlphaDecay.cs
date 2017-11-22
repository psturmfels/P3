using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaDecay : MonoBehaviour {

    public float decayRate = .01f;
    public bool decaying = true;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Color newColor = spriteRenderer.color;
        if (newColor.a > 0 && decaying == true) {
            newColor.a -= decayRate;
            
        }
        if (newColor.a < 1 && decaying == false) {
            newColor.a += decayRate;
        }

        spriteRenderer.color = newColor;
    }

    public void ReverseDecay() {
        decaying = false;
    }
}
