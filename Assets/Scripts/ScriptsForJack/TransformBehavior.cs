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

	private InputDirectional id;
	private StateMachineForJack parentStateMachine;
	private bool firstCancel = false;
	private bool secondCancel = false;
	private bool isTransforming = false;

    //Sliding behavior members
    public bool canSlide;
    public Vector3 slideOffset = new Vector3(1.0f, 0, 0);
    private Vector3 transformedFrom;
    private Vector3 target;

	void Awake () {
		id = GetComponent<InputDirectional> ();
		parentStateMachine = GetComponentInParent<StateMachineForJack> ();
		ReachedTransform += ReachedTransformScale;
		ReachedNormal += ReachedNormalScale; 
		StartTowardsTransform += ScaleToTransform;
		StartTowardsNormal += ScaleToNormal;
		WasCanceled += ScaleToNormal;
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
		float transformIterations = Vector3.Distance (transform.localScale, normalScale) / transformSpeed;
		float moveLerpSpeed = Vector3.Distance (transform.parent.position, target) / transformIterations;

		while (transform.localScale != normalScale) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, normalScale, transformSpeed);
		    if (canSlide) {
				transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, moveLerpSpeed);
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
		float transformIterations = Vector3.Distance (transform.localScale, transformScale) / transformSpeed;
		float moveLerpSpeed = Vector3.Distance (transform.parent.position, target) / transformIterations;

        while (transform.localScale != transformScale) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, transformScale, transformSpeed);
            if (canSlide) {
				transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, moveLerpSpeed);
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
		if (id == null) {
			return;
		}
		if (id != null) {
			Debug.Log ("Horizontal: " + id.GetCurrentHorzAxis ().ToString() + " Vertical: " + id.GetCurrentVertAxis ().ToString ());
		}
        
		if (gameObject.name == "BridgeMellowTransformed") {
            BridgeTargetPositionUpdate(transforming);
        }
        else {
            StiltTargetPositionUpdate(transforming);
        }
    }

    private void BridgeTargetPositionUpdate(bool transforming) {
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

    private void StiltTargetPositionUpdate(bool transforming) {
        float vertAxis = id.GetCurrentVertAxis();
        if (vertAxis > 0) {
            if (!transforming && !PlayerFitsAt(2)) { // Collider check for top side
                target = transformedFrom;
            }
            else {
                target = transform.position + new Vector3(0, 3.1f, 0);
            }
        }
        else if (vertAxis < 0) {
            if (!transforming && !PlayerFitsAt(3)) { // Collider check for bottom side
                target = transformedFrom;
            }
            else {
                target = transform.position - new Vector3(0, 3.1f, 0);
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
