using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchLatch : MonoBehaviour {

    public UnityAction OnSwitchTrigger;
    public Sprite switchTriggeredSprite;
    
    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
	    sr = GetComponent<SpriteRenderer>();
	    OnSwitchTrigger += SwitchTriggered;
	}

    private void OnTriggerEnter2D(Collider2D other) {
        OnSwitchTrigger();
    }

    private void SwitchTriggered()
    {
        sr.sprite = switchTriggeredSprite;
    }
}
