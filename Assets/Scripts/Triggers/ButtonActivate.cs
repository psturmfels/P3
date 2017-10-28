using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivate : MonoBehaviour {

    public GameObject TriggeredObject;
    public Sprite buttonUnpressedSprite;
    public Sprite buttonPressedSprite;
    public bool locked = true;

    private SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ChangeAlphaOfChildren(locked ? 1.0f : 0.5f);
        ChangeColliderOfChildren(locked);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeColliderOfChildren(!locked);
        ChangeAlphaOfChildren(!locked ? 1.0f : 0.5f);
        sr.sprite = buttonPressedSprite;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ChangeColliderOfChildren(locked);
        ChangeAlphaOfChildren(locked ? 1.0f : 0.5f);
        sr.sprite = buttonUnpressedSprite;
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
