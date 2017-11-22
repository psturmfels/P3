using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchLatch : MonoBehaviour {

    public UnityAction OnSwitchTrigger;
    public Sprite switchTriggeredSprite;

    public AudioSource leverSound;

    private SpriteRenderer sr;
	private bool hasBeenSwitched = false;

	// Use this for initialization
	void Start () {
	    sr = GetComponent<SpriteRenderer>();

        if (leverSound == null)
        {
            leverSound = GameObject.Find("GameCamera").transform.Find("SFX").Find("Lever").GetComponent<AudioSource>();
        }

	    OnSwitchTrigger += SwitchTriggered;
		if (leverSound == null) {
			GameObject leverAudioObject = GameObject.Find ("Lever");
			if (leverAudioObject.GetComponent<AudioSource> () != null) {
				leverSound = leverAudioObject.GetComponent<AudioSource> ();
			}
		}
	}

    private void OnTriggerEnter2D(Collider2D other) {
		if (!hasBeenSwitched) {
			OnSwitchTrigger ();
		}
    }

    private void SwitchTriggered()
    {
		if (GetComponent<FadeOutAndDie> () != null) {
			GetComponent<FadeOutAndDie> ().StartFadeOut ();
		}
        sr.sprite = switchTriggeredSprite;
        leverSound.Play();
		hasBeenSwitched = true;
    }
}
