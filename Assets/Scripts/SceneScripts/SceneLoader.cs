using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader instance;

    private int currentLevel;
    private PlayerActions controllerActions;
    private PlayerActions keyboardActions;
    private List<string> levels = new List<string>();
    

    void Awake() {
        if (instance != null)
            Destroy(gameObject);
        else {
            instance = this;
        }
        Application.targetFrameRate = 30;

        levels.Add("Hub World"); // Start at hub
        levels.Add("Level 1");
        levels.Add("Level 2");
        levels.Add("Level 3");
        levels.Add("Level 4");
        levels.Add("Level 5");
        levels.Add("Level 6");
        levels.Add("Level 7");
        levels.Add("Level 8");
        levels.Add("Hub World"); // Return to hub

        if (SceneManager.GetActiveScene().name == "Hub World") {
            // Placing players to correct position
            RepositionPlayersInHub();
        }
    }

    void Start() {
        controllerActions = PlayerActions.CreateWithControllerBindings();
        keyboardActions = PlayerActions.CreateWithKeyboardBindings();

        currentLevel = levels.IndexOf(SceneManager.GetActiveScene().name);
        

    }

    // Update is called once per frame
    void Update() {
        if (controllerActions.BackLevel.WasPressed || keyboardActions.BackLevel.WasPressed) {
            if (SceneManager.GetActiveScene().buildIndex != 0) {
                LoadMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            PlayerPrefs.SetInt("Last Level", 0);
            Debug.Log("Restarting the scene with last level: 0");
            Destroy(GameObject.Find("PlayerDeviceManager"));
            RestartLevel();
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
        //        var scenes = SceneManager.GetAllScenes();
        SceneManager.LoadScene(levels[level]);
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene(levels[currentLevel + 1]);
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
