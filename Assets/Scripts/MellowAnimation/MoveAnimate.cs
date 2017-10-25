using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimate : MonoBehaviour {
	public Sprite standingFrame;
	public Sprite[] positiveMoveFrames;
	public Sprite[] negativeMoveFrames;
	public bool trackMovement = true; 

	private InputMove im;
	private SpriteRenderer sr;
	private Sprite[] interruptSprites;
	private float timeBetweenInterruptSprites = 0.0f;
	private int currentInterruptIndex = 0;

	public void ReturnMovementAnimation() {
		sr.enabled = true;
		sr.sprite = standingFrame;
		trackMovement = true;
	}

	public void OverrideMovementAnimation(Sprite OverrideSprite) {
		sr.sprite = OverrideSprite;
		trackMovement = false;
	}

	public void DisableRenderer() {
		trackMovement = false;
		sr.enabled = false;
	}

	public void InterruptMovementAnimation(Sprite[] newInterruptSprites, float timeBetweenSprites) {
		CancelInvoke ();
		interruptSprites = newInterruptSprites;
		trackMovement = false;
		timeBetweenInterruptSprites = timeBetweenSprites;
		currentInterruptIndex = 0;
		CycleInterruptSprites ();
	}

	void CycleInterruptSprites() {
		if (currentInterruptIndex == interruptSprites.Length) {
			trackMovement = true;
			currentInterruptIndex = 0;
			return;
		}

		sr.sprite = interruptSprites [currentInterruptIndex];
		currentInterruptIndex += 1;
		Invoke ("CycleInterruptSprites", timeBetweenInterruptSprites);
	}

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		im = GetComponent<InputMove> ();
	}

	void Update () {
		if (trackMovement) {
			float currentHorzSpeed = im.GetCurrentHorzSpeed ();
			int frameIndex = (int)Mathf.Abs((float)(positiveMoveFrames.Length - 1) * im.GetCurrentHorzAxis() * currentHorzSpeed / im.maxMoveSpeed);
			frameIndex = Mathf.Min (frameIndex, positiveMoveFrames.Length - 1); 
			if (im.GetCurrentHorzAxis () > 0.0f) {
				sr.sprite = positiveMoveFrames [frameIndex];
			} else if (im.GetCurrentHorzAxis () < 0.0f) {
				sr.sprite = negativeMoveFrames [frameIndex];
			} else {
				sr.sprite = standingFrame;
			}
		}
	}

}
