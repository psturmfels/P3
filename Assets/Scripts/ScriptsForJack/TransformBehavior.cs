using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformBehavior: MonoBehaviour {
	public Vector3 transformScale;
	public Vector3 normalScale;
    public Vector3 slideOffset = new Vector3(1.0f, 0, 0);
	public float transformSpeed;

	public UnityAction ReachedTransform;
	public UnityAction ReachedNormal;
	public UnityAction StartTowardsTransform;
	public UnityAction StartTowardsNormal;
	public UnityAction ReachedDeath;
	public UnityAction WasCanceled;

	private StateMachineForJack parentStateMachine;
	private bool firstCancel = false;
	private bool secondCancel = false;
	private bool isTransforming = false;

    //Sliding behavior members
    private Vector3 target;
    private float lerpSpeed;

	void Awake () {
		parentStateMachine = GetComponentInParent<StateMachineForJack> ();
		ReachedTransform += ReachedTransformScale;
		ReachedNormal += ReachedNormalScale;
		StartTowardsTransform += ScaleToTransform;
		StartTowardsNormal += ScaleToNormal;
		WasCanceled += ScaleToNormal;
	}

    void Start() {
        float numScalingFrames = (transformScale.x - normalScale.x) / transformSpeed;
        lerpSpeed = (transform.parent.position - slideOffset).magnitude / numScalingFrames;
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
		if (parentStateMachine.GetState () == StateMachineForJack.State.InTransition) {
			ResetCancelChecks ();
			StopAllCoroutines ();
			WasCanceled ();
		}
	}

	void IsNotTransforming() {
		isTransforming = false;
	}

	void ScaleToTransform () {
		if (isTransforming) {
			StopAllCoroutines ();
		}
		isTransforming = true;
		parentStateMachine.SetState (StateMachineForJack.State.InTransition);
	    UpdateTargetPosition();
        StartCoroutine(LerpToTransformScale());
    }

	void ScaleToNormal() {
		if (isTransforming) {
			StopAllCoroutines ();
		}
		isTransforming = true; 
		parentStateMachine.SetState (StateMachineForJack.State.InTransition);
        UpdateTargetPosition();
        StartCoroutine (LerpToNormalScale ());
    }

	public bool IsTransforming() {
		return isTransforming;
	}
		
	IEnumerator LerpToNormalScale(bool shouldDie = false) {
		while (transform.localScale != normalScale) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, normalScale, transformSpeed);
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, lerpSpeed);
            yield return null;
		}
		if (shouldDie) {
			ReachedDeath ();
		} else {
			ReachedNormal ();
		}
	}

	IEnumerator LerpToTransformScale() {
        while (transform.localScale != transformScale) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, transformScale, transformSpeed);
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, lerpSpeed);
            yield return null;
		}
		ReachedTransform ();
	}

    public void TransformIntoDeath() {
		if (isTransforming) {
			StopAllCoroutines ();
		}
		isTransforming = true; 
		parentStateMachine.SetState (StateMachineForJack.State.Disabled);
		StartCoroutine (LerpToNormalScale (true));
	}

    private void UpdateTargetPosition() {
        InputDirectional im = GetComponent<InputDirectional>();
        if (im == null) return;
        if (im.GetCurrentHorzAxis() > 0) {
            target = transform.position + slideOffset;
        }
        else if (im.GetCurrentHorzAxis() < 0) {
            target = transform.position - slideOffset;
        }
        else {
            target = transform.position;
        }
    }
}
