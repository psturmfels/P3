using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivate : MonoBehaviour {

    public GameObject TriggeredObject;
    public bool locked = true;

    Renderer render;
    Color oldColor;
    Color activeColor = Color.cyan;

    // Use this for initialization
    void Start()
    {
        render = this.GetComponent<Renderer>();
        oldColor = render.material.color;
        ChangeAlphaOfChildren(locked ? 1.0f : 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeColliderOfChildren(!locked);
        ChangeAlphaOfChildren(!locked ? 1.0f : 0.5f);
        render.material.color = activeColor;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ChangeColliderOfChildren(locked);
        ChangeAlphaOfChildren(locked ? 1.0f : 0.5f);
        render.material.color = oldColor;
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
