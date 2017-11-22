using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader instance;

    private PlayerActions controllerActions;
    private PlayerActions keyboardActions;
    private List<string> levels = new List<string>();
    private int currentLevel;

    void Awake() {
        if (instance != null)
            Destroy(gameObject);
        else {
            instance = this;
        }
        Application.targetFrameRate = 30;

        levels.Add("Hub World"); //this will eventually become hub
        levels.Add("gs_Level_1");
        levels.Add("gs_Level_2");
        levels.Add("Level 3 Dev");
        levels.Add("Level 4 Dev");
        levels.Add("Level5_dev");
        levels.Add("Level6_dev");
        levels.Add("Level 7 Dev");
        levels.Add("Hub World"); //TODO: fill with level 8
        levels.Add("Hub World"); //what do we do about this? return to hub?
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
    }

    public void LoadLevel(string levelNumber) {
        int level = 0;
        int.TryParse(levelNumber, out level);
        var scenes = SceneManager.GetAllScenes();
        SceneManager.LoadScene(levels[level]);
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene(levels[currentLevel + 1]);
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Hub World");
    }

    public void LoadEnd() {
        SceneManager.LoadScene("gs_endGame");
    }

    public void RestartLevel() {
        Debug.Log(currentLevel);
        SceneManager.LoadScene(levels[currentLevel]);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
