using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGrahamCrackerDoorsOnDeath : MonoBehaviour {
	public GameObject[] doors;
	public GameObject[] switches;

	private List<GameObject> savedDoors = new List<GameObject> ();
	private List<GameObject> savedSwitches = new List<GameObject> ();

	void Start () {
		if (doors.Length != switches.Length) {
			gameObject.SetActive (false);
			return;
		}

		for (int i = 0; i < doors.Length; ++i) {
			GameObject doorObj = doors [i];
			GameObject doorCopy = Instantiate (doorObj, doorObj.transform.position, doorObj.transform.rotation);
			if (doorObj.transform.parent != null) {
				doorCopy.transform.SetParent (doorObj.transform.parent);
			}

			savedDoors.Add (doorCopy);

			GameObject switchObj = switches [i];
			GameObject switchCopy = Instantiate (switchObj, switchObj.transform.position, switchObj.transform.rotation);
			if (switchObj.transform.parent != null) {
				switchCopy.transform.SetParent (switchObj.transform.parent);
			}
				
			savedSwitches.Add (switchCopy);

			if (doorCopy.GetComponent<Door> () != null) {
				doorCopy.GetComponent<Door> ().triggers = new GameObject[] { switchCopy };
			}

			doorCopy.SetActive (false);
			switchCopy.SetActive (false);
		}


		GameObject StiltMellow = GameObject.Find ("StiltMellow");
		if (StiltMellow != null && StiltMellow.GetComponent<MellowCrushed> () != null) {
			StiltMellow.GetComponent<MellowCrushed> ().Respawn += ResetObjects;
		}

		GameObject BridgeMellow = GameObject.Find ("BridgeMellow");
		if (BridgeMellow != null && BridgeMellow.GetComponent<MellowCrushed> () != null) {
			BridgeMellow.GetComponent<MellowCrushed> ().Respawn += ResetObjects;
		}
	}



	void ResetObjects() {
		for (int i = 0; i < doors.Length; ++i) {
			if (doors [i] == null && switches [i] == null) {
				doors [i] = savedDoors [i];
				switches [i] = savedSwitches [i];

				GameObject currentDoor = doors [i];
				GameObject currentSwitch = switches [i];
				currentDoor.SetActive (true);
				currentSwitch.SetActive (true);

				GameObject doorCopy = Instantiate (currentDoor, currentDoor.transform.position, currentDoor.transform.rotation);
				if (currentDoor.transform.parent != null) {
					doorCopy.transform.SetParent (currentDoor.transform.parent);
				}

				GameObject switchCopy = Instantiate (currentSwitch, currentSwitch.transform.position, currentSwitch.transform.rotation);
				if (currentSwitch.transform.parent != null) {
					switchCopy.transform.SetParent (currentSwitch.transform.parent);
				}

				if (doorCopy.GetComponent<Door> () != null) {
					doorCopy.GetComponent<Door> ().triggers = new GameObject[] { switchCopy };
				}

				doorCopy.SetActive (false);
				switchCopy.SetActive (false);
				savedDoors [i] = doorCopy;
				savedSwitches [i] = switchCopy;
			}
		}
	}
}
