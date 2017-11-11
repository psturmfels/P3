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
	public UnityAction ReachedDeath;
	public UnityAction WasCanceled;

	private StateMachineForJack parentStateMachine;
	private bool firstCancel = false;
	private bool secondCancel = false;
	private bool isTransforming = false;

    //Sliding behavior members
    public bool canSlide;
    public Vector3 slideOffset = new Vector3(1.0f, 0, 0);
    private Vector3 transformedFrom;
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
        transformedFrom = transform.position;
        UpdateTargetPosition(true);
        StartCoroutine(LerpToTransformScale());
    }

	void ScaleToNormal() {
		if (isTransforming) {
			StopAllCoroutines ();
		}
		isTransforming = true; 
		parentStateMachine.SetState (StateMachineForJack.State.InTransition);
	    UpdateTargetPosition(false);
        StartCoroutine (LerpToNormalScale ());
    }

    public bool IsTransforming() {
		return isTransforming;
	}
		
	IEnumerator LerpToNormalScale(bool shouldDie = false) {
		while (transform.localScale != normalScale) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, normalScale, transformSpeed);
		    if (canSlide) {
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, lerpSpeed);
            }
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
            if (canSlide) {
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, lerpSpeed);
            }
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

    private void UpdateTargetPosition(bool transforming) {
        InputDirectional id = GetComponent<InputDirectional>();
        if (id == null) return;
        if (gameObject.name == "BridgeMellowTransformed") {
            BridgeTargetPositionUpdate(id, transforming);
        }
        else {
            StiltTargetPositionUpdate(id, transforming);
        }
    }

    private void BridgeTargetPositionUpdate(InputDirectional id, bool transforming) {
        float horzAxis = id.GetCurrentHorzAxis();
        if (horzAxis < 0) {
            if (!transforming && !PlayerFitsAt(2)) { // Collider check for left side
                target = transformedFrom;
            }
            else {
                target = transform.position - new Vector3(3.3f, 0, 0);
            }
        }
        else if (horzAxis > 0) {
            if (!transforming && !PlayerFitsAt(3)) { // Collider check for right side
                target = transformedFrom;
            }
            else {
                target = transform.position + new Vector3(3.3f, 0, 0);
            }
        }
        else {
            if (!transforming && !PlayerFitsAt(4)) { // Collider check for middle side
                target = transformedFrom;
            }
            else {
                target = transform.position;
            }
        }
    }

    private void StiltTargetPositionUpdate(InputDirectional id, bool transforming) {
        float vertAxis = id.GetCurrentVertAxis();
        if (vertAxis > 0) {
            if (!transforming && !PlayerFitsAt(2)) { // Collider check for top side
                target = transformedFrom;
            }
            else {
                target = transform.position - new Vector3(0, 3.1f, 0);
            }
        }
        else if (vertAxis < 0) {
            if (!transforming && !PlayerFitsAt(3)) { // Collider check for bottom side
                target = transformedFrom;
            }
            else {
                target = transform.position + new Vector3(0, 3.1f, 0);
            }
        }
        else {
            if (!transforming && !PlayerFitsAt(4)) { // Collider check for middle side
                target = transformedFrom;
            }
            else {
                target = transform.position;
            }
        }
    }

    private bool PlayerFitsAt(int colliderChildIndex) {
        return transform.GetChild(colliderChildIndex).gameObject.GetComponent<PlayerFitCheck>().playerCanFit();
    }
}
