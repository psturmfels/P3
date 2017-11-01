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
        SceneManager.LoadScene("gs_Level_" + levelNumber);
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Level Select Menu");
    }

    public void LoadEnd()
    {
        SceneManager.LoadScene("gs_endGame");
    }
}
