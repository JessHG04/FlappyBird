using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour{

    public void PlayButtonClicked() {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    
    public void QuitButtonClicked() {
        Application.Quit();
    }
}
