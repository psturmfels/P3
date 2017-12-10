using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiatePrefabOverTime : MonoBehaviour {
	public GameObject prefabToInstantiate;
	public float secondsBetweenInstantiation;

	void Start () {
		StartCoroutine (SpawnPrefabsEndlessly ());
	}
	
	IEnumerator SpawnPrefabsEndlessly() {
        yield return new WaitForSeconds(1.0f);
        while (true) {
			if (prefabToInstantiate != null) {
				Instantiate (prefabToInstantiate, transform.position, Quaternion.identity);
				yield return new WaitForSeconds (secondsBetweenInstantiation);
			}
		}
	}
}
