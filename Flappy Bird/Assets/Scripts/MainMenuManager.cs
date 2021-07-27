using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour{
    public void PlayButtonClicked() {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    
    public void QuitButtonClicked() {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Application.Quit();
    }
}
