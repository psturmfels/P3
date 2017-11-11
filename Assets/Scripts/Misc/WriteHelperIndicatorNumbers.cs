using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WriteHelperIndicatorNumbers : MonoBehaviour {
	private HelperIndicatorMove him;

	void Start () {
		him = GetComponent<HelperIndicatorMove> ();
	}

	void Update () {
		AssignLabels ();
	}

	void AssignLabels() {
		foreach (GameObject indicator in him.movePositionIndicators) {
			if (indicator == null) {
				continue;
			}
			if (indicator.GetComponentInChildren<TextMesh> () != null) {
				indicator.GetComponentInChildren<TextMesh> ().text = "";
			}
		}
		int index = 0;
		foreach (GameObject indicator in him.movePositionIndicators) {
			if (indicator == null) {
				continue;
			}
			if (indicator.GetComponentInChildren<TextMesh> () != null) {
				indicator.GetComponentInChildren<TextMesh> ().text += index.ToString() + ",";
			}
			index += 1;
		}
	}
}
