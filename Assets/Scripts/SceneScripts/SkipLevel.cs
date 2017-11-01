using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class SkipLevel : MonoBehaviour {

    public int currentLevel = 1;
    int numberOfLevels = 2;
    PlayerActions controllerActions;
    PlayerActions keyboardActions;

    private void Start()
    {
        controllerActions = PlayerActions.CreateWithControllerBindings();
        keyboardActions = PlayerActions.CreateWithKeyboardBindings();
    }

    // Update is called once per frame
    void Update () {
        if (controllerActions.AdvanceLevel.WasPressed || keyboardActions.AdvanceLevel.WasPressed) {
            Debug.Log("Advance Level");
            LoadNextLevel();
        }

        if (controllerActions.BackLevel.WasPressed || keyboardActions.BackLevel.WasPressed) {
            LoadPrevLevel();
        }
	}

    public void LoadNextLevel() {
        if (currentLevel < numberOfLevels)
        {
            ++currentLevel;
            GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadLevel(currentLevel.ToString());
        }
        else {
            Debug.Log("At final scene!");
        }
    }

    public void LoadPrevLevel() {
        
        if (currentLevel > 1)
        {
            --currentLevel;
            GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadLevel(currentLevel.ToString());
        }
        else {
            Debug.Log("At scene 0!");
        }
        
    }
}
