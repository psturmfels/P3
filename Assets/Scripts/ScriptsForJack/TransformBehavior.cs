using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBehavior: MonoBehaviour {
	public Vector3 transformScale;
	public Vector3 normalScale;
	public float transformSpeed;
	public Sprite standing;
	public TransformBehavior otherBehavior;
	public StateMachineForJack parentStateMachine;
	public StateMachineForJack.State normalSetState;

	private MoveAnimate ma;
	private MellowStates ms;
	private bool scaleToNormal = false;
	private bool scaleToTransform = false;
	private float eps = 0.1f;
	private Vector3 largestScaleDirection;
	private bool firstCancel = false;
	private bool secondCancel = false;

	void Start () {
		ms = GetComponent<MellowStates> ();
		ma = GetComponent<MoveAnimate> ();
	}

	void FixedUpdate () {
		if (scaleToNormal) {
			transform.localScale = Vector3.Lerp (transform.localScale, normalScale, transformSpeed);
			if (Vector3.SqrMagnitude(transform.localScale - normalScale) < eps) {
				ReachedNormalScale ();
			}
		} else if (scaleToTransform) {
			transform.localScale = Vector3.Lerp (transform.localScale, transformScale, transformSpeed);
			if (Vector3.SqrMagnitude(transform.localScale - transformScale) < eps) {
				ReachedTransformScale ();
			}
		}
	}

	void ReachedTransformScale() {
		ResetCancelChecks ();

		if (ma != null) {
			ma.DisableRenderer ();
		}
		otherBehavior.gameObject.SetActive (true);
		otherBehavior.transform.position = transform.position;
		otherBehavior.ScaleToNormal ();

		transform.localScale = transformScale;
		scaleToTransform = false;
		gameObject.SetActive (false);
	}

	void ReachedNormalScale() {
		ResetCancelChecks ();

		transform.localScale = normalScale;
		scaleToNormal = false;
		parentStateMachine.SetState (normalSetState);
		if (ms != null) {
			ms.SetState (MellowStates.State.Transformed, false);
		}
		if (ma != null) {
			ma.ReturnMovementAnimation ();
		}
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
		if (!scaleToNormal && !scaleToTransform) {
			return;
		}
		if (normalSetState == StateMachineForJack.State.Normal) {
			ScaleToNormal ();
		} else if (normalSetState == StateMachineForJack.State.Transformed){
			ScaleToTransform ();
		}
	}

	

	public void ScaleToTransform () {
		if (ma != null) {
			ma.OverrideMovementAnimation (standing);
		}
		if (GetComponent<PickUpAction> () != null) {
			GetComponent<PickUpAction> ().DropItem ();
		}

		scaleToNormal = false;
		scaleToTransform = true;
	}

	public void ScaleToNormal() {
		if (ma != null) {
			ma.ReturnMovementAnimation ();
		}

		scaleToTransform = false;
		scaleToNormal = true;
	}
}
