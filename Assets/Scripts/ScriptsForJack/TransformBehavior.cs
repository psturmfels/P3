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
	private Vector3 targetOffset;
	private bool isGoingToTransform = false;
	private bool shouldSlideFromReject = false;
	private bool isCanceling = false;
	private bool isDying = false;

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
			isCanceling = true;
		}
	}

	void ScaleToTransform () {
		if (isTransforming) {
			StopAllCoroutines ();
		}
		isTransforming = true;
		parentStateMachine.SetState (StateMachineForJack.State.InTransition);
		transformedFrom = transform.parent.position;
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
		isCanceling = false;
		isDying = false;
	}

	IEnumerator LerpToTransformScale() {
		isGoingToTransform = true;
		float transformIterations = Vector3.Distance (transform.localScale, transformScale) / transformSpeed;
		float moveLerpSpeed = Vector3.Distance (transform.parent.position, target) / transformIterations;

        while (transform.localScale != transformScale) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, transformScale, transformSpeed);
			if (canSlide || shouldSlideFromReject) {
				transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, moveLerpSpeed);
            }
            yield return null;
		}
		ReachedTransform ();
		isGoingToTransform = false;
		shouldSlideFromReject = false;
	}

	public void LerpFromRejectInDirection(float signOfReject) {
		if (!isGoingToTransform || isCanceling || isDying) {
			return;
		}

		StopAllCoroutines ();
		target = transform.parent.position + slideOffset * signOfReject;
		shouldSlideFromReject = true;
		StartCoroutine (LerpToTransformScale ());
	}

    public void TransformIntoDeath() {
		if (isTransforming) {
			StopAllCoroutines ();
		}
		isTransforming = true; 
		isDying = true;
		parentStateMachine.SetState (StateMachineForJack.State.Disabled);
		StartCoroutine (LerpToNormalScale (true));
	}

    private void UpdateTargetPosition(bool transforming) {
		if (id == null) {
			return;
		}
        
		TargetPositionUpdate (transforming);
    }

    private void TargetPositionUpdate(bool transforming) {
		float axis = 0.0f;
		if (slideOffset.normalized == Vector3.right) {
			axis = id.GetCurrentHorzAxis ();
		} else {
			axis = id.GetCurrentVertAxis ();
		}
		if (axis < 0) {
			if (!transforming && !PlayerCanTransformTo(-slideOffset)) { //!PlayerFitsAt(2)) { // Collider check for left side
                target = transformedFrom;
            }
            else {
				target = transform.parent.position - slideOffset;
            }
        }
		else if (axis > 0) {
			if (!transforming && !PlayerCanTransformTo(slideOffset)) { // Collider check for right side
                target = transformedFrom;
            }
            else {
				target = transform.parent.position + slideOffset;
            }
        }
        else {
			if (!transforming && !PlayerCanTransformTo(Vector3.zero)) { // Collider check for middle side
                target = transformedFrom;
            }
            else {
				target = transform.parent.position;
            }
        }
    }

	private bool PlayerCanTransformTo(Vector3 offset) {
		int countedIntersections = 0;
		float horzBoxOffset = 0.3f;
		float vertBoxOffset = 0.5f;
		Vector2 boxSize = new Vector2 (0.4f, 0.4f);
		Vector3 newTransformPosition = transform.parent.position + offset;

		Vector2 transformPositionTopLeft = new Vector2 (newTransformPosition.x - horzBoxOffset, newTransformPosition.y + vertBoxOffset);
		Collider2D[] TopLeftOverlap = Physics2D.OverlapBoxAll (transformPositionTopLeft, boxSize, 0.0f);
		foreach (Collider2D coll in TopLeftOverlap) {
			if (coll.gameObject.GetComponentInParent<StateMachineForJack> () == null && coll.gameObject.CompareTag ("Ground")) {
				countedIntersections += 1;
				break;
			}
		}

		Vector2 transformPositionTopRight = new Vector2 (newTransformPosition.x + horzBoxOffset, newTransformPosition.y + vertBoxOffset);
		Collider2D[] TopRightOverlap = Physics2D.OverlapBoxAll (transformPositionTopRight, boxSize, 0.0f);
		foreach (Collider2D coll in TopRightOverlap) {
			if (coll.gameObject.GetComponentInParent<StateMachineForJack> () == null && coll.gameObject.CompareTag ("Ground")) {
				countedIntersections += 1;
				break;
			}
		}

		Vector2 transformPositionBotLeft = new Vector2 (newTransformPosition.x - horzBoxOffset, newTransformPosition.y - vertBoxOffset);
		Collider2D[] BotLeftOverlap = Physics2D.OverlapBoxAll (transformPositionBotLeft, boxSize, 0.0f);
		foreach (Collider2D coll in BotLeftOverlap) {
			if (coll.gameObject.GetComponentInParent<StateMachineForJack> () == null && coll.gameObject.CompareTag ("Ground")) {
				countedIntersections += 1;
				break;
			}
		}

		Vector2 transformPositionBotRight = new Vector2 (newTransformPosition.x + horzBoxOffset, newTransformPosition.y - vertBoxOffset);
		Collider2D[] BotRightOverlap = Physics2D.OverlapBoxAll (transformPositionBotRight, boxSize, 0.0f);
		foreach (Collider2D coll in BotRightOverlap) {
			if (coll.gameObject.GetComponentInParent<StateMachineForJack> () == null && coll.gameObject.CompareTag ("Ground")) {
				countedIntersections += 1;
				break;
			}
		}

		if (countedIntersections == 4) {
			return false;
		} else {
			return true;
		}
	}
//
//    private bool PlayerFitsAt(int colliderChildIndex) {
//        return transform.GetChild(colliderChildIndex).gameObject.GetComponent<PlayerFitCheck>().playerCanFit();
//    }
}
