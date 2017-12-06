using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FadeInOnTrigger : MonoBehaviour {
    private SpriteRenderer[] srs;
    private TextMesh[] tms;
    private float fadeIncrement = 0.025f;
	private int numMellows = 0;
//    private int numWillFade = 0;
    private Dictionary<TextMesh, bool> tmDict;
    private Dictionary<SpriteRenderer, bool> srDict;

	void Start () {
	    tms = GetComponentsInChildren<TextMesh>();
        srs = GetComponentsInChildren<SpriteRenderer>();
        srs = srs.Skip(1).ToArray(); // To skip the sign sprite

        ResetFadedDicts();
//        numWillFade = tms.Length + srs.Length;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			numMellows += 1;
			if (numMellows == 1) {
				StopAllCoroutines ();
				StartCoroutine ("FadeIn");
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			numMellows -= 1;
			if (numMellows == 0) {
				StopAllCoroutines ();
				StartCoroutine ("FadeOut");
			}
		}
	}


    IEnumerator FadeIn() {
        ResetFadedDicts();
        while (tmDict.Any(c => c.Value == false) || srDict.Any(c => c.Value == false)) {
            foreach (var tm in tms) {
                if (tmDict[tm] == false) {
                    Color textColor = tm.color;
                    textColor.a = Mathf.Min(textColor.a + fadeIncrement, 0.8f);
                    tm.color = textColor;
                    if (textColor.a > 0.8f) {
                        tmDict[tm] = true;
                    }
                }
            }
            foreach (var sr in srs) {
                if (srDict[sr] == false) {
                    Color srColor = sr.color;
                    srColor.a = Mathf.Max(srColor.a + fadeIncrement, 0.8f);
                    sr.color = srColor;
                    if (srColor.a > 0.8f) {
                        srDict[sr] = true;
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator FadeOut() {
        ResetFadedDicts();
        while (tmDict.Any(c => c.Value == false) || srDict.Any(c => c.Value == false)) {
            foreach (var tm in tms) {
                if (tmDict[tm] == false) {
                    Color textColor = tm.color;
                    textColor.a = Mathf.Min(textColor.a - fadeIncrement, 0.0f);
                    tm.color = textColor;
                    if (textColor.a <= 0.0f) {
                        tmDict[tm] = true;
                    }
                }
            }
            foreach (var sr in srs) {
                if (srDict[sr] == false) {
                    Color srColor = sr.color;
                    srColor.a = Mathf.Max(srColor.a - fadeIncrement, 0.0f);
                    sr.color = srColor;
                    if (srColor.a <= 0.0f) {
                        srDict[sr] = true;
                    }
                }
            }
            yield return null;
        }
	}

    private void ResetFadedDicts() {
        tmDict = new Dictionary<TextMesh, bool>();
        srDict = new Dictionary<SpriteRenderer, bool>();
        foreach (var tm in tms) {
            tmDict.Add(tm, false);
        }
        foreach (var sr in srs) {
            srDict.Add(sr, false);
        }
    }
}
