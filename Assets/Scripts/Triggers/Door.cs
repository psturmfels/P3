using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	public bool DefaultActive = true;
    public GameObject[] triggers;
    private int unlockedTriggers;


	// Use this for initialization
	void Start () {
	    unlockedTriggers = 0;
		foreach (GameObject t in triggers)
        {
            if (t.GetComponent<ButtonActivate>() != null)
            {
                ButtonActivate ba = t.GetComponent<ButtonActivate>();
				if (ba != null) {
					ba.OnButtonPress += TriggerPressed;
					ba.OnButtonRelease += TriggerReleased;
				}
            }
            else if (t.GetComponent<SwitchLatch>() != null) {
                SwitchLatch sl = t.GetComponent<SwitchLatch>();
				if (sl != null) {
					if (DefaultActive) {
						sl.OnSwitchTrigger += LatchSwitched;
					} else {
						sl.OnSwitchTrigger += TriggerPressed;
					}
				}
            }
        }
		SetDefault ();
    }

	private void LatchSwitched() {
		++unlockedTriggers;
		if (unlockedTriggers == triggers.Length) {
			DestroyBlocks ();
		}
	}

	private void DestroyBlocks() {
		foreach (Transform child in transform)
		{
			FadeOutAndDie foad = child.gameObject.GetComponent<FadeOutAndDie> ();
			if (foad != null && child.gameObject.activeSelf) {
				foad.StartFadeOut ();
			}
		}
	}

    private void TriggerPressed() {
        unlockedTriggers++;
        if (unlockedTriggers == triggers.Length) {
			SetHighlighted ();
        }
    }

    private void TriggerReleased() {
        unlockedTriggers--;
		SetDefault ();
    }

	private void SetHighlighted() {
		if (DefaultActive) {
			ChangeAlphaOfSprite (0.5f);
			ChangeColliders (false);
		} else {
			ChangeAlphaOfSprite (1.0f);
			ChangeColliders (true);
		}
	}

	private void SetDefault() {
		if (DefaultActive) {
			ChangeAlphaOfSprite (1.0f);
			ChangeColliders (true);
		} else {
			ChangeAlphaOfSprite (0.5f);
			ChangeColliders (false);
		}
	}

    private void ChangeAlphaOfSprite(float alpha)
    {
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.gameObject.GetComponent<SpriteRenderer>();
			if (sr != null) {
				Color color = sr.color;
				color.a = alpha;
				sr.color = color;
			}
        }
    }

    private void ChangeColliders(bool doorlocked)
    {
        foreach (Transform child in transform)
        {
            BoxCollider2D bc = child.gameObject.GetComponent<BoxCollider2D>();
			if (bc != null) {
				bc.enabled = doorlocked;
			}
        }
    }
}
