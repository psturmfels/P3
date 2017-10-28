using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLatch : MonoBehaviour {

    public GameObject TriggeredObject;

    Renderer render;
    Color activeColor = Color.cyan;

	// Use this for initialization
	void Start () {
        render = GetComponent<Renderer>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeAlphaOfChildren(0.5f);
        ChangeColliderOfChildren(false);
        render.material.color = activeColor;
    }

    private void ChangeAlphaOfChildren(float alpha)
    {
        foreach (Transform child in TriggeredObject.transform)
        {
            SpriteRenderer sr = child.gameObject.GetComponent<SpriteRenderer>();
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

    private void ChangeColliderOfChildren(bool doorlocked)
    {
        foreach (Transform child in TriggeredObject.transform)
        {
            BoxCollider2D bc = child.gameObject.GetComponent<BoxCollider2D>();
            bc.enabled = doorlocked;
        }
    }
}
