using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public GameObject[] triggers;
    private int unlockedTriggers;

	// Use this for initialization
	void Start () {
	    unlockedTriggers = 0;
        foreach (var t in triggers)
        {
            if (t.GetComponent<ButtonActivate>() != null)
            {
                ButtonActivate ba = t.GetComponent<ButtonActivate>();
                ba.OnButtonPress += TriggerPressed;
                ba.OnButtonRelease += TriggerReleased;
            }
            else if (t.GetComponent<SwitchLatch>() != null) {
                SwitchLatch sl = t.GetComponent<SwitchLatch>();
                sl.OnSwitchTrigger += TriggerPressed;
            }
        }
    }

    private void TriggerPressed() {
        unlockedTriggers++;
        if (unlockedTriggers == triggers.Length) {
            ChangeAlphaOfSprite(0.5f);
            ChangeColliders(false);
        }
    }

    private void TriggerReleased() {
        unlockedTriggers--;
        ChangeAlphaOfSprite(1.0f);
        ChangeColliders(true);
    }

    private void ChangeAlphaOfSprite(float alpha)
    {
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.gameObject.GetComponent<SpriteRenderer>();
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

    private void ChangeColliders(bool doorlocked)
    {
        foreach (Transform child in transform)
        {
            BoxCollider2D bc = child.gameObject.GetComponent<BoxCollider2D>();
            bc.enabled = doorlocked;
        }
    }
}
