using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader instance;

    void Awake() {
        if (instance != null)
            Destroy(gameObject);
        else {
            instance = this;
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

    public void LoadEnd()
    {
        SceneManager.LoadScene("gs_endGame");
    }
}
