using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeInOut : MonoBehaviour {
	private float fadeInSpeed = 0.02f;

    void Start() {
        // For object itself
        Image selfImg = gameObject.GetComponent<Image>();
        Text selfTxt = gameObject.GetComponent<Text>();
        if (selfImg != null) {
            selfImg.color = new Color(selfImg.color.r, selfImg.color.g, selfImg.color.b, 0.0f);
        }
        else if (selfTxt != null) {
            selfTxt.color = new Color(selfTxt.color.r, selfTxt.color.g, selfTxt.color.b, 0.0f);
        }

        // For each children
        for (int i = 0; i < transform.childCount; ++i) {
            Image img = transform.GetChild(i).gameObject.GetComponent<Image>();
            Text txt = transform.GetChild(i).gameObject.GetComponent<Text>();
            if (img != null) {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.0f);
            }
            else if (txt != null) {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0.0f);
            }
        }
    }

    public void FadeIn() {
        // For object itself
        Image selfImg = gameObject.GetComponent<Image>();
        Text selfTxt = gameObject.GetComponent<Text>();
        if (selfImg != null) {
            StartCoroutine(ImageFadeIn(selfImg));
        }
        else if (selfTxt != null) {
            StartCoroutine(TextFadeIn(selfTxt));
        }

        // For each children
        for (int i = 0; i < transform.childCount; ++i) {
            Image img = transform.GetChild(i).gameObject.GetComponent<Image>();
            Text txt = transform.GetChild(i).gameObject.GetComponent<Text>();
            if (img != null) {
                StartCoroutine(ImageFadeIn(img));
            }
            else if (txt != null) {
                StartCoroutine(TextFadeIn(txt));
            }
        }
    }

    public void FadeOut() {
        // For object itself
        Image selfImg = gameObject.GetComponent<Image>();
        Text selfTxt = gameObject.GetComponent<Text>();
        if (selfImg != null) {
            StartCoroutine(ImageFadeOut(selfImg));
        }
        else if (selfTxt != null) {
            StartCoroutine(TextFadeOut(selfTxt));
        }

        // For each children
        for (int i = 0; i < transform.childCount; ++i) {
            Image img = transform.GetChild(i).gameObject.GetComponent<Image>();
            Text txt = transform.GetChild(i).gameObject.GetComponent<Text>();
            if (img != null) {
                StartCoroutine(ImageFadeOut(img));
            }
            else if (txt != null) {
                StartCoroutine(TextFadeOut(txt));
            }
        }
    }
	
	IEnumerator ImageFadeIn(Image img) {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.0f);
        while (img.color.a < 1.0f) {
            img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.Min (img.color.a + fadeInSpeed, 1.0f));
			yield return null;
		}
	}

    IEnumerator ImageFadeOut(Image img) {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
        while (img.color.a > 0.0f) {
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Max(img.color.a - fadeInSpeed, 0.0f));
            yield return null;
        }
    }

    IEnumerator TextFadeIn(Text txt) {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0.0f);
        while (txt.color.a < 1.0f) {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, Mathf.Min(txt.color.a + fadeInSpeed, 1.0f));
            yield return null;
        }
    }

    IEnumerator TextFadeOut(Text txt) {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1.0f);
        while (txt.color.a > 0.0f) {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, Mathf.Max(txt.color.a - fadeInSpeed, 0.0f));
            yield return null;
        }
    }
}
