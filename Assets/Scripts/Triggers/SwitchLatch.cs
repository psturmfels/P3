using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLatch : MonoBehaviour {

    public GameObject TriggeredObject;
    public Sprite pressedButtonSprite;

//    private Renderer render;
    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
//        render = GetComponent<Renderer>();
	    sr = GetComponent<SpriteRenderer>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeAlphaOfChildren(0.5f);
        ChangeColliderOfChildren(false);
        sr.sprite = pressedButtonSprite;
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
