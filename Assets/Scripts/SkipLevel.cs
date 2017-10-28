using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipLevel : MonoBehaviour {

    int currentBuildInd = SceneManager.GetActiveScene().buildIndex;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.N)) {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            LoadPrevLevel();
        }
	}

    public void LoadNextLevel() {
        Scene nextScene = SceneManager.GetSceneByBuildIndex(currentBuildInd + 1);

        if (nextScene.IsValid())
        {
            SceneManager.LoadScene(nextScene.buildIndex);
        }
        else {
            Debug.Log("At final scene!");
        }
    }

    public void LoadPrevLevel() {
        
        if (currentBuildInd > 0)
        {
            SceneManager.LoadScene(currentBuildInd - 1);
        }
        else {
            Debug.Log("At scene 0!");
        }
        
    }
}
