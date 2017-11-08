using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformBehavior: MonoBehaviour {
	public Vector3 transformScale;
	public Vector3 normalScale;
	public float transformSpeed;

	public UnityAction ReachedTransform;
	public UnityAction ReachedNormal;
	public UnityAction StartTowardsTransform;
	public UnityAction StartTowardsNormal;

	private StateMachineForJack parentStateMachine;
	private bool firstCancel = false;
	private bool secondCancel = false;
	private bool isTransforming = false;

	void Awake () {
		parentStateMachine = GetComponentInParent<StateMachineForJack> ();
		ReachedTransform += ReachedTransformScale;
		ReachedNormal += ReachedNormalScale;
		StartTowardsTransform += ScaleToTransform;
		StartTowardsNormal += ScaleToNormal;
	}

	void ReachedTransformScale() {
		isTransforming = false;
		ResetCancelChecks ();
		parentStateMachine.SetState (StateMachineForJack.State.Transformed);
	}

	void ReachedNormalScale() {
		isTransforming = false;
		ResetCancelChecks ();
		parentStateMachine.SetState (StateMachineForJack.State.Normal);
	}

	void ResetCancelChecks() {
		firstCancel = false;
		secondCancel = false;
	}

	public void RegisterCancelContact(int cancelID) {
		if (cancelID == 1) {
			firstCancel = true;
		} else if (cancelID == 2) {
			secondCancel = true;
		}
		if (firstCancel && secondCancel) {
			CancelTransform ();
		}
	}

	void CancelTransform() {
		ResetCancelChecks ();
		StopAllCoroutines ();
		ScaleToNormal ();
	}

	void IsNotTransforming() {
		isTransforming = false;
	}

	void ScaleToTransform () {
		isTransforming = true;
		parentStateMachine.SetState (StateMachineForJack.State.InTransition);
		StartCoroutine (LerpToTransformScale ());
	}

	void ScaleToNormal() {
		isTransforming = true; 
		parentStateMachine.SetState (StateMachineForJack.State.InTransition);
		StartCoroutine (LerpToNormalScale ());
	}

	public bool IsTransforming() {
		return isTransforming;
	}
		
	IEnumerator LerpToNormalScale() {
		while (transform.localScale != normalScale) {
			transform.localScale = Vector3.MoveTowards (transform.localScale, normalScale, transformSpeed);
			yield return null;
		}
		ReachedNormal ();
	}

	IEnumerator LerpToTransformScale() {
		while (transform.localScale != transformScale) {
			transform.localScale = Vector3.MoveTowards (transform.localScale, transformScale, transformSpeed);
			yield return null;
		}
		ReachedTransform ();
	}

}
