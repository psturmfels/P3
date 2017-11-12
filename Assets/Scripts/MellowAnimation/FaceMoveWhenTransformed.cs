using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMoveWhenTransformed : MonoBehaviour {
	public Vector2 offsetAxis;

	private float offsetMagnitude;
	private InputDirectional id;
	private float offsetHalfMultiplier = 0.4f;
	private float moveSpeed = 0.5f;
	private Vector3 defaultLocalPosition;
	private GameObject transformObject;
	private StateMachineForJack stateMachine;

	private Vector3 normalScale;
	private Vector3 transformScale;
	private float currentAxisAmount = 0.0f;

	void Start () {
		offsetMagnitude = 1.0f;
		defaultLocalPosition = transform.localPosition;
		if (transform.parent != null) {
			if (GetComponentInParent<StateMachineForJack> () != null) {
				stateMachine = GetComponentInParent<StateMachineForJack> ();
			}
				
			if (transform.parent.GetComponentInChildren<InputDirectional> (true) != null) {
				id = transform.parent.GetComponentInChildren<InputDirectional> (true);
			}
			foreach (Transform child in transform.parent) {
				if (child.gameObject.GetComponent<TransformBehavior> () != null) {
					normalScale = child.gameObject.GetComponent<TransformBehavior> ().normalScale;
					transformScale = child.gameObject.GetComponent<TransformBehavior> ().transformScale;
					transformObject = child.gameObject;
				}
			}
		}
	}
	
	void Update () {
	    TransformBehavior tb = transformObject.GetComponent<TransformBehavior>();
		if (id != null && transformObject != null && transformObject.activeSelf) {
            if (tb != null && !tb.canSlide) {
                return;
            }
            if (transformObject.GetComponent<BoxCollider2D> () != null) {
				float normDiff = Vector3.Distance (normalScale, transformScale);
				float currentDiff = Vector3.Distance (normalScale, transformObject.transform.localScale);
				offsetHalfMultiplier = Mathf.Sqrt(currentDiff / normDiff) * 0.4f;

				Vector3 offsetIndicator = new Vector3 (offsetAxis.x, offsetAxis.y, 0.0f);
				offsetMagnitude = Vector3.Dot (transformObject.GetComponent<BoxCollider2D> ().bounds.size, offsetIndicator) * offsetHalfMultiplier;
			}

			if (stateMachine.GetState () == StateMachineForJack.State.Transformed) {
				ReadAxis ();
			}

			if (currentAxisAmount == 0.0f) {
				StartLerpToOffset (Vector2.zero);
			} else {
				StartLerpToOffset (Mathf.Sign (currentAxisAmount) * offsetMagnitude * offsetAxis);
			}
		} else {
			if (stateMachine.GetState () == StateMachineForJack.State.Normal) {
				ReadAxis ();
			}
			if (transform.localPosition != defaultLocalPosition) {
				currentAxisAmount = 0.0f;
				StartLerpToOffset (Vector2.zero);
			}
		}
	}

	void ReadAxis() {
		if (offsetAxis == Vector2.right) {
			currentAxisAmount = id.GetCurrentHorzAxis ();
		} else if (offsetAxis == Vector2.up) {
			currentAxisAmount = id.GetCurrentVertAxis ();
		}
	}

	void StartLerpToOffset(Vector2 newOffset) {
		StopAllCoroutines ();
		StartCoroutine (MoveToOffset (new Vector3 (newOffset.x, newOffset.y, 0.0f)));
	}

	IEnumerator MoveToOffset(Vector3 newOffset) {
		Vector3 targetPosition = defaultLocalPosition + newOffset;
		while (transform.localPosition != newOffset) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, targetPosition, moveSpeed);
			yield return null;
		}

	}
}
