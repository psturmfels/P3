using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineForJack : MonoBehaviour {
	public enum State {
		Normal,
		Transformed,
		InTransition
	};

	public KeyCode transformKey;

	public GameObject normalObject;
	public GameObject transformedObject;

	private State currentState = State.Normal;

	public void SetState(State newState) {
		currentState = newState;
	}

	void TransitionToState(State newState) {
		if (newState == currentState || currentState == State.InTransition) {
			return;
		}

		currentState = State.InTransition;
		if (newState == State.Transformed) {
			if (normalObject.GetComponent<TransformBehavior> () != null) {
				normalObject.GetComponent<TransformBehavior> ().ScaleToTransform ();
			}
		} else if (newState == State.Normal) {
			if (transformedObject.GetComponent <TransformBehavior> () != null) {
				transformedObject.GetComponent <TransformBehavior> ().ScaleToTransform ();
			}
		}
	}
		
	void Update () {
		if (Input.GetKeyDown (transformKey)) {
			if (currentState == State.Normal) {
				TransitionToState (State.Transformed);
			} else if (currentState == State.Transformed) {
				TransitionToState (State.Normal);
			}
		}
	}
}
