using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader instance;

    private int currentLevel;
    private PlayerActions controllerActions;
    private PlayerActions keyboardActions;
    private List<string> levels;
    private List<string> levelNames = new List<string>();
    private GameObject topLargeText;
    private GameObject mediumLargeText;

    void Awake() {
        if (instance != null)
            Destroy(gameObject);
        else {
            instance = this;
        }
        Application.targetFrameRate = 30;
        levels = new List<string> {
            "Hub World", // Start at hub
            "Level 1",
            "Level 2",
            "Level 3",
            "Level 4",
            "Level 5",
            "Level 6",
            "Level 7",
            "Level 7",
            "Hub World" // Return to hub
        };

        levelNames = new List<string> {
            "Hub World",
            "Minty Meadows",
            "Gummy Gardens",
            "Strawberry Shortcake",
            "Cookie Crumbles",
            "Red Velvet Ravine",
            "Frosty Flakes",
            "Winter Wonderland",
            "Winter Wonderland",
            "Hub World"
        };

        if (SceneManager.GetActiveScene().name == "Hub World") {
            // Placing players to correct position
            RepositionPlayersInHub();
        }
    }

    void Start() {
        controllerActions = PlayerActions.CreateWithControllerBindings();
        keyboardActions = PlayerActions.CreateWithKeyboardBindings();
        currentLevel = levels.IndexOf(SceneManager.GetActiveScene().name);

        StartCoroutine(DisplayLevelName());
    }

    // Update is called once per frame
    void Update() {
        if (controllerActions != null && keyboardActions != null) {
            if (controllerActions.BackLevel.WasPressed || keyboardActions.BackLevel.WasPressed) {
                if (SceneManager.GetActiveScene().buildIndex != 0) {
                    LoadMenu();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R)) {
            PlayerPrefs.SetInt("Last Level", 0);
            Debug.Log("Restarting the scene with last level: 0");
            Destroy(GameObject.Find("PlayerDeviceManager"));
            PlayerPrefs.DeleteAll();
            LoadMenu();
        }
    }

    IEnumerator DisplayLevelName() {
        yield return new WaitForSeconds(3.0f);
        GameObject topLargeText = GameObject.Find("Top Large Text");
        GameObject topMediumText = GameObject.Find("Top Medium Text");
        GameObject topLeftText = GameObject.Find("Top Left Text");
        if (topLargeText != null && topMediumText != null && topLeftText != null) {
            Text[] txts = topLargeText.GetComponentsInChildren<Text>();
            if (txts.Length > 0){
                foreach (var txt in txts){
                    txt.text = levelNames[currentLevel];
                }
            }
            txts = topMediumText.GetComponentsInChildren<Text>();
            if (txts.Length > 0) {
                foreach (var txt in txts) {
                    txt.text = levels[currentLevel];
                }
            }
            txts = topLeftText.GetComponentsInChildren<Text>();
            if (txts.Length > 0) {
                foreach (var txt in txts) {
                    txt.text = "Press Start to go back\nto Candy Commons";
                }
            }
            topLargeText.GetComponent<ImageFadeInOut>().FadeIn();
            topMediumText.GetComponent<ImageFadeInOut>().FadeIn();
            topLeftText.GetComponent<ImageFadeInOut>().FadeIn();
            yield return new WaitForSeconds(5.0f);
            topLargeText.GetComponent<ImageFadeInOut>().FadeOut();
            topMediumText.GetComponent<ImageFadeInOut>().FadeOut();
            topLeftText.GetComponent<ImageFadeInOut>().FadeOut();
        }
    }

    private void RepositionPlayersInHub() {
        int lastLevel = PlayerPrefs.GetInt("Last Level");
        Debug.Log("Last completed level: " + lastLevel);
        Vector3 pos;
        Vector3 bridgeOffset = new Vector3(-1f, 0f, 0f);
        Vector3 stiltOffset = new Vector3(1f, 0f, 0f);
        Vector3 cameraPos = new Vector3(0f, 0f, -10f);
        switch (lastLevel) {
            case 1:
                pos = new Vector2(23.5f, 1.5f);
                break;
            case 2:
                pos = new Vector2(19f, 7.5f);
                break;
            case 3:
                pos = new Vector2(27.5f, 13.5f);
                break;
            case 4:
                pos = new Vector2(40f, 7.5f);
                break;
            case 5:
                pos = new Vector2(30.5f, 6.5f);
                break;
            case 6:
                pos = new Vector2(38f, 2f);
                break;
            case 7:
                pos = new Vector2(52f, 12.5f);
                break;
            case 8:
                pos = new Vector2(62.5f, 4.5f);
                break;
            default:
                pos = new Vector3(0f, 1f, 0f);
                bridgeOffset = new Vector3(-5f, 0f, 0f);
                stiltOffset = new Vector3(5f, 0f, 0f);
                break;
        }
        Camera.main.transform.position = cameraPos + pos;
        GameObject.Find("BridgeMellow").transform.position = pos + bridgeOffset;
        GameObject.Find("StiltMellow").transform.position = pos + stiltOffset;
    }

    public void LoadLevel(string levelNumber) {
        int level = 0;
        int.TryParse(levelNumber, out level);
        PlayerPrefs.SetInt("Last Level", level);
        SceneManager.LoadScene(levels[level]);
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Hub World");
    }

    public void LevelCompleted() {
        PlayerPrefs.SetInt("Last Level", currentLevel);
    }

    public void RestartLevel() {
        SceneManager.LoadScene(levels[currentLevel]);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
