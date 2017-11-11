using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeColorAfterTransform : MonoBehaviour {
	public Color snapColor;

	private Color initialColor;
	private SpriteRenderer moveSpriteRender;
	private SpriteRenderer transformSpriteRender;
	private float fadeTime = 2.0f;

	void Start () {
		foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer> (true)) {
			if (rend.gameObject.GetComponent<MellowStates> () != null) {
				moveSpriteRender = rend;
				initialColor = moveSpriteRender.color;
			} else if (rend.gameObject.GetComponent<TransformBehavior> () != null) {
				transformSpriteRender = rend;
			}
		}
		if (GetComponent<StateMachineForJack> () != null) {
			StateMachineForJack stateMachine = GetComponent<StateMachineForJack> ();
			stateMachine.InitiateTransformReload += StartSnapAndFade;
			stateMachine.BeganInputTransform += ReverseSnapAndFade;
		}
	}
		
	void ReverseSnapAndFade() {
		StopAllCoroutines ();
		StartCoroutine (SnapAndFadeBack (fadeTime, initialColor, snapColor));
	}

	void StartSnapAndFade() {
		StopAllCoroutines ();
		StartCoroutine (SnapAndFadeBack (fadeTime, snapColor, initialColor));
	}

	IEnumerator SnapAndFadeBack(float overTime, Color firstColor, Color secondColor) {
		moveSpriteRender.color = firstColor;
		transformSpriteRender.color = firstColor;
		yield return null;

		float elapsedTime = 0.0f; 
		while (moveSpriteRender.color != secondColor) {
			moveSpriteRender.color = Color.Lerp (moveSpriteRender.color, secondColor, elapsedTime / overTime);
			transformSpriteRender.color = moveSpriteRender.color;
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}
}
