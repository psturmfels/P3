using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    enum sceneState {
        Menu,
        Level
    }

    public void LoadLevel(string levelNumber) {
        SceneManager.LoadScene("Level " + levelNumber + " Dev");
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Level Select Menu");
    }
}
