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

	public void InterruptMovementAnimation(Sprite[] newInterruptSprites, float timeBetweenSprites, bool trackMovementOnComplete = true) {
		CancelInvoke ();
		interruptSprites = newInterruptSprites;
		trackMovement = false;
		StartCoroutine (CycleInterruptSprites (trackMovementOnComplete, timeBetweenSprites));
	}


	IEnumerator CycleInterruptSprites(bool trackMovementOnComplete, float timeBetweenSprites) {
		int currentInterruptIndex = 0;

		while (currentInterruptIndex < interruptSprites.Length) {
			sr.sprite = interruptSprites [currentInterruptIndex];
			currentInterruptIndex += 1;
			yield return new WaitForSeconds (timeBetweenSprites);
		}

		if (trackMovementOnComplete) {
			trackMovement = true;
		}
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
