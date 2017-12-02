using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditorSpriteLayerChanger : MonoBehaviour {

    public int layerToChange = 5;

    void Update() {
        ChangeChildrenToLayer(transform);
    }

    private void ChangeChildrenToLayer(Transform t) {
        //        Debug.Log(t.gameObject.name);
        SpriteRenderer sr = t.gameObject.GetComponent<SpriteRenderer>();
        if (sr != null) {
            sr.sortingOrder = layerToChange;
        }
        for (int i = 0; i < t.childCount; ++i) {
            ChangeChildrenToLayer(t.GetChild(i));
        }
    }

}