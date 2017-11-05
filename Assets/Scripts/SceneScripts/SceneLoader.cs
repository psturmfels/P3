using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader instance;

    private PlayerActions controllerActions;
    private PlayerActions keyboardActions;

    void Awake() {
        if (instance != null)
            Destroy(gameObject);
        else {
            instance = this;
        }
    }

    void Start() {
        controllerActions = PlayerActions.CreateWithControllerBindings();
        keyboardActions = PlayerActions.CreateWithKeyboardBindings();
    }

    // Update is called once per frame
    void Update() {
        if (controllerActions.BackLevel.WasPressed || keyboardActions.BackLevel.WasPressed) {
            LoadMenu();
        }
    }

    public void LoadLevel(string levelNumber) {
        int level = 0;
        int.TryParse(levelNumber, out level);
        SceneManager.LoadScene(level);
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Level Select Menu");
    }

    public void LoadEnd() {
        SceneManager.LoadScene("gs_endGame");
    }
}
