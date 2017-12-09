using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour {
	private EnableChocolateOnTrigger firstSmore;
	private EnableChocolateOnTrigger secondSmore;
	private bool hasFinishedlevel = false;
	private int numUniqueMellowsFinished = 0;

    public int backToMenuDelay = 10;
//    public GameObject finishPanel;
    public AudioSource finishSound;
    public AudioSource mainTheme;
    public GameObject candyWave;
    public int level = 1;
    private GameObject firstPlayer;

    // UI Elements regarding level finish
    private GameObject topLargeText;
    private GameObject topMediumText;
    private GameObject leftPanel;
    private GameObject rightPanel;


    private void Awake() {
        if (mainTheme == null || finishSound == null || candyWave == null) {
            var cam = GameObject.Find("GameCamera");
            mainTheme = cam.GetComponent<AudioSource>();
            finishSound = cam.transform.Find("SFX").Find("Finish").GetComponent<AudioSource>();
            candyWave = cam.transform.Find("CandyWave").gameObject;
        }
    }

    void Start() {
		EnableChocolateOnTrigger[] smores = GetComponentsInChildren<EnableChocolateOnTrigger> ();
		if (smores.Length == 2) {
			firstSmore = smores [0];
			firstSmore.registeredMellow += AddMellow;
			firstSmore.lostMellow += SubtractMellow;

			secondSmore = smores [1];
			secondSmore.registeredMellow += AddMellow;
			secondSmore.lostMellow += SubtractMellow;
		}
        topLargeText = GameObject.Find("Top Large Text");
        topMediumText = GameObject.Find("Top Medium Text");
        leftPanel = GameObject.Find("Left Panel");
        rightPanel = GameObject.Find("Right Panel");
    }

	void AddMellow() {
		numUniqueMellowsFinished += 1;
		if (numUniqueMellowsFinished == 2) {
			CheckFinishCondition ();
		}
    }

	void SubtractMellow() {
		numUniqueMellowsFinished -= 1;
    }

	void CheckFinishCondition() {
		if (!hasFinishedlevel && firstSmore.mellowName != secondSmore.mellowName) {
            Invoke ("BackToMenu", backToMenuDelay);
			hasFinishedlevel = true;
			firstSmore.EnableChocolate ();
			secondSmore.EnableChocolate ();

            // Audio changes
            mainTheme.Pause();
            finishSound.Play();

            // UI changes
		    if (topLargeText != null) {
                Text[] txts = topLargeText.GetComponentsInChildren<Text>();
                if (txts.Length > 0) {
                    foreach (var txt in txts) {
                        txt.text = "Level Completed";
                    }
                }
                topLargeText.GetComponent<ImageFadeInOut>().FadeIn();
            }
		    if (topMediumText != null) {
                StartCoroutine(ChangeGoingBackText(backToMenuDelay - 1));
                topMediumText.GetComponent<ImageFadeInOut>().FadeIn();
            }
		    if (leftPanel != null && rightPanel != null) {
                // Reposition panels based on which smore is on which side
                if (firstSmore.mellowName == "BridgeMellowMove") {
		            Vector3 rightPos = rightPanel.GetComponent<RectTransform>().position;
		            Vector3 leftPos = leftPanel.GetComponent<RectTransform>().position;
		            leftPanel.GetComponent<RectTransform>().position = rightPos;
		            rightPanel.GetComponent<RectTransform>().position = leftPos;
		            Vector3 xAxisFlippedScale = new Vector3(-1f, 1f, 1f);
		            leftPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale = xAxisFlippedScale;
		            rightPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale = xAxisFlippedScale;
		        }
                leftPanel.GetComponent<ImageFadeInOut>().FadeIn();
                rightPanel.GetComponent<ImageFadeInOut>().FadeIn();
            }
		    StartCoroutine("StartDecay");
        }
	}

    IEnumerator ChangeGoingBackText(int seconds) {
        Text[] txts = topMediumText.GetComponentsInChildren<Text>();
        if (txts.Length > 0) {
            foreach (var txt in txts) {
                txt.text = "Going back in " + seconds;
            }
        }
        yield return new WaitForSeconds(1.0f);
        if (seconds > 1) {
            StartCoroutine(ChangeGoingBackText(seconds - 1));
        }
    }

    private IEnumerator StartDecay() {
        yield return new WaitForSeconds(3.0f);
        topLargeText.GetComponent<ImageFadeInOut>().FadeOut();
        topMediumText.GetComponent<ImageFadeInOut>().FadeOut();
        leftPanel.GetComponent<ImageFadeInOut>().FadeOut();
        rightPanel.GetComponent<ImageFadeInOut>().FadeOut();
        candyWave.GetComponent<ReverseDecay>().ReverseWaveDecay();
    }

    private void BackToMenu() {
        SetLevelComplete();
        SceneLoader.instance.LevelCompleted();
        SceneLoader.instance.LoadMenu();
    }

    private void SetLevelComplete()
    {
        string target = "Level" + level.ToString() + "Finish";
        PlayerPrefs.SetInt(target, 1);
    }

}
