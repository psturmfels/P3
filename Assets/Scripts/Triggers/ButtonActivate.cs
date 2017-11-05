using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActivate : MonoBehaviour {

    public UnityAction OnButtonPress;
    public UnityAction OnButtonRelease;

//    public GameObject TriggeredObject;
    public Sprite buttonUnpressedSprite;
    public Sprite buttonPressedSprite;
    public bool locked = true;

    private SpriteRenderer sr;
    private int numPlayersOnButton = 0;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        OnButtonPress += ButtonPressed;
        OnButtonRelease += ButtonReleased;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        numPlayersOnButton++;
        if (numPlayersOnButton == 1) {
            OnButtonPress();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        numPlayersOnButton--;
        if (numPlayersOnButton == 0) {
            OnButtonRelease();
        }
    }

    private void ButtonPressed() {
        sr.sprite = buttonPressedSprite;
    }

    private void ButtonReleased() {
        sr.sprite = buttonUnpressedSprite;
    }


}
