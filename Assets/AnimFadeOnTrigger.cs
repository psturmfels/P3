using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFadeOnTrigger : MonoBehaviour {

    private SpriteRenderer sr;
    private float fadeIncrement = 0.025f;
    private int numMellows = 0;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
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
        while (sr.color.a > 0.0f)
        {
            Color panelColor = sr.color;
            panelColor.a = Mathf.Max(panelColor.a - fadeIncrement, 0.0f);
            sr.color = panelColor;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        while (sr.color.a < 0.8f)
        {
            Color panelColor = sr.color;
            panelColor.a = Mathf.Min(panelColor.a + fadeIncrement, 0.8f);
            sr.color = panelColor;
            yield return null;
        }
    }
}
