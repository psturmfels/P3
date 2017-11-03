using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFadeInOnTrigger : MonoBehaviour {

    private SpriteRenderer buttonSr;
    private SpriteRenderer sr;
    private float fadeIncrement = 0.025f;
    private int numMellows = 0;

    void Start()
    {
        buttonSr = GetComponent<SpriteRenderer>();
        sr = GetComponentsInChildren<SpriteRenderer>()[1];
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

    IEnumerator FadeOut()
    {
        while (buttonSr.color.a > 0.0f)
        {
            Color buttonColor = buttonSr.color;
            buttonColor.a = Mathf.Max(buttonColor.a - fadeIncrement, 0.0f);
            buttonSr.color = buttonColor;

            Color panelColor = sr.color;
            panelColor.a = Mathf.Max(panelColor.a - fadeIncrement, 0.0f);
            sr.color = panelColor;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        while (buttonSr.color.a < 0.8f)
        {
            Color buttonColor = buttonSr.color;
            buttonColor.a = Mathf.Min(buttonColor.a + fadeIncrement, 0.8f);
            buttonSr.color = buttonColor;

            Color panelColor = sr.color;
            panelColor.a = Mathf.Min(panelColor.a + fadeIncrement, 0.8f);
            sr.color = panelColor;
            yield return null;
        }
    }
}
