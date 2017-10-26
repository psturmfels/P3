using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivate : MonoBehaviour {

    public GameObject TriggeredObject;

    Renderer render;
    Color oldColor;
    Color activeColor = Color.cyan;

    // Use this for initialization
    void Start()
    {
        render = this.GetComponent<Renderer>();
        oldColor = render.material.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TriggeredObject.SetActive(false);
        render.material.color = activeColor;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        TriggeredObject.SetActive(true);
        render.material.color = oldColor;
    }
}
