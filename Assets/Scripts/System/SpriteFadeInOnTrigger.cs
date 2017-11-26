using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class SpriteFadeInOnTrigger : MonoBehaviour {

    private SpriteRenderer buttonSr;
    private SpriteRenderer[] childSrs;
    private float fadeIncrement = 0.025f;
    private int numMellows = 0;

    void Start()
    {
        buttonSr = GetComponent<SpriteRenderer>();
        childSrs = GetComponentsInChildren<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            numMellows += 1;
            if (numMellows == 1)
            {
                StopAllCoroutines();
                StartCoroutine("FadeIn");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            numMellows -= 1;
            if (numMellows == 0)
            {
                StopAllCoroutines();
                StartCoroutine("FadeOut");
            }
        }
    }

    IEnumerator FadeOut() {
        bool mainButtonDone = false;
        bool childrenDone = false;
        while (!mainButtonDone && !childrenDone) {
            if (buttonSr != null) {
                if (buttonSr.color.a > 0f) {
                    mainButtonDone = true;
                }
                Color buttonColor = buttonSr.color;
                buttonColor.a = Mathf.Max(buttonColor.a - fadeIncrement, 0.0f);
                buttonSr.color = buttonColor;
            }

            if (childSrs != null) {
                bool allChildrenDone = true;
                foreach (var sr in childSrs) {
                    if (sr.color.a > 0f) {
                        allChildrenDone = false;
                    }
                    Color panelColor = sr.color;
                    panelColor.a = Mathf.Max(panelColor.a - fadeIncrement, 0.0f);
                    sr.color = panelColor;
                }
                childrenDone = allChildrenDone;
            }
            yield return null;
        }
    }

    IEnumerator FadeIn() {
        bool mainButtonDone = false;
        bool childrenDone = false;
        while (!mainButtonDone && !childrenDone) {
            if (buttonSr != null) {
                if (buttonSr.color.a > 0.8f) {
                    mainButtonDone = true;
                }
                Color buttonColor = buttonSr.color;
                buttonColor.a = Mathf.Min(buttonColor.a + fadeIncrement, 0.8f);
                buttonSr.color = buttonColor;
            }

            if (childSrs != null) {
                bool allChildrenDone = true; 
                foreach (var sr in childSrs) {
                    if (sr.color.a < 0.8f) {
                        allChildrenDone = false;
                    }
                    Color panelColor = sr.color;
                    panelColor.a = Mathf.Min(panelColor.a + fadeIncrement, 0.8f);
                    sr.color = panelColor;
                }
                childrenDone = allChildrenDone;
            }
            yield return null;
        }
    }
}
