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

	private InputDirectional id;
	private StateMachineForJack parentStateMachine;
	private bool isTransforming = false;

    //Sliding behavior members
    public bool canSlide;
    public Vector3 slideOffset = new Vector3(1.0f, 0, 0);
    private Vector3 transformedFrom;
    private Vector3 target;
	private Vector3 targetOffset;
	private bool isCanceling = false;
	private float distanceToCancel = 0.0f;

	void Awake () {
		id = GetComponent<InputDirectional> ();
		parentStateMachine = GetComponentInParent<StateMachineForJack> ();
		ReachedTransform += ReachedTransformScale;
		ReachedNormal += ReachedNormalScale; 
		StartTowardsTransform += ScaleToTransform;
		StartTowardsNormal += ScaleToNormal;
	}

	void ReachedTransformScale() {
		isTransforming = false;
		parentStateMachine.SetState (StateMachineForJack.State.Transformed);
	}

	void ReachedNormalScale() {
		isTransforming = false;
		parentStateMachine.SetState (StateMachineForJack.State.Normal);
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
	}

	IEnumerator LerpToTransformScale() {
		float transformIterations = Vector3.Distance (transform.localScale, transformScale) / transformSpeed;
		float moveLerpSpeed = Vector3.Distance (transform.parent.position, target) / transformIterations;
		Vector2 cancelDistanceIndicator = new Vector2 (slideOffset.x, slideOffset.y).normalized;
		float finalTransformedSize = new Vector2 (slideOffset.x, slideOffset.y).magnitude * 2.0f;

        while (transform.localScale != transformScale) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, transformScale, transformSpeed);
			if (canSlide) {
				transform.parent.position = Vector3.MoveTowards(transform.parent.position, target, moveLerpSpeed);
            }
			float currentTransformScale = Vector2.Dot (new Vector2(transform.localScale.x, transform.localScale.y), cancelDistanceIndicator) * finalTransformedSize;
			if (isCanceling && currentTransformScale > distanceToCancel) {
				UpdateTargetPosition (false);
				StartCoroutine (LerpToNormalScale ());
				isCanceling = false;
				yield break;
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
        
		TargetPositionUpdate (transforming);
    }

    private void TargetPositionUpdate(bool transforming) {
		bool dominantDirectionHit = false;
		bool secondaryDirectionHit = false;
		Vector2 dominantDirectionCastPosition = Vector2.zero;
		Vector2 secondaryDirectionCastPosition = Vector2.zero;
		Vector2 dominantDirection = new Vector2 (slideOffset.x, slideOffset.y);
		Vector2 secondaryDirection = -dominantDirection;
		if (transforming) {
			Vector2 currentPos = new Vector2 (parentStateMachine.transform.position.x, parentStateMachine.transform.position.y);
			int groundMask = 1 << LayerMask.NameToLayer ("Ground");
			if (slideOffset.normalized == Vector3.right) {
				groundMask = groundMask | (1 << LayerMask.NameToLayer ("Stilt"));
			} else {
				groundMask = groundMask | (1 << LayerMask.NameToLayer ("Bridge"));
			}

			RaycastHit2D dominantDirectionCast = Physics2D.Raycast (currentPos, dominantDirection.normalized, dominantDirection.magnitude * 2.0f, groundMask);
			RaycastHit2D secondaryDirectionCast = Physics2D.Raycast (currentPos, secondaryDirection.normalized, secondaryDirection.magnitude * 2.0f, groundMask);
			if (dominantDirectionCast.collider != null) {
				dominantDirectionHit = true;
				dominantDirectionCastPosition = dominantDirectionCast.point;
			}
			if (secondaryDirectionCast.collider != null) {
				secondaryDirectionHit = true;
				secondaryDirectionCastPosition = secondaryDirectionCast.point;
			}

			if (dominantDirectionHit && secondaryDirectionHit) {
				target = 0.5f * (dominantDirectionCastPosition + secondaryDirectionCastPosition);
				float distanceBetweenContacts = Mathf.Abs(Vector2.Dot (dominantDirectionCastPosition - secondaryDirectionCastPosition, dominantDirection.normalized));
				if (distanceBetweenContacts < dominantDirection.magnitude * 2.0f) {
					isCanceling = true;
					distanceToCancel = distanceBetweenContacts;
					return;
				}
			}
		}

		float axis = 0.0f;
		if (slideOffset.normalized == Vector3.right) {
			axis = id.GetCurrentHorzAxis ();
		} else {
			axis = id.GetCurrentVertAxis ();
		}
		if (isCanceling) {
			target = transformedFrom;
		} 
		else if (axis < 0) {
			if (!transforming && !PlayerCanTransformTo(-slideOffset)) { // Collider check for secondary direction
                target = transformedFrom;
            }
            else {
				if (secondaryDirectionHit) {
					target = secondaryDirectionCastPosition - secondaryDirection;
				} else {
					if (!transforming) {
						target = transform.parent.position - slideOffset * 0.9f;
					} else {
						target = transform.parent.position - slideOffset;
					}
				}
            }
        }
		else if (axis > 0) {
			if (!transforming && !PlayerCanTransformTo(slideOffset)) { // Collider check for dominant direction
                target = transformedFrom;
            }
            else {
				if (dominantDirectionHit) {
					target = dominantDirectionCastPosition - dominantDirection;
				} else {
					if (!transforming) {
						target = transform.parent.position + slideOffset * 0.9f;
					} else {
						target = transform.parent.position + slideOffset;
					}
				}
            }
        }
        else {
			if (!transforming && !PlayerCanTransformTo(Vector3.zero)) { // Collider check for no direction
                target = transformedFrom;
            }
            else {
				if (dominantDirectionHit) {
					target = dominantDirectionCastPosition - dominantDirection;
				} else if (secondaryDirectionHit) {
					target = secondaryDirectionCastPosition - secondaryDirection;
				} else {
					target = transform.parent.position;
				}
            }
        }
    }

	private bool PlayerCanTransformTo(Vector3 offset) {
		int countedIntersections = 0;
		float horzBoxOffset = 0.3f;
		float vertBoxOffset = 0.55f;
		Vector2 boxSize = new Vector2 (0.5f, 0.5f);
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
}
