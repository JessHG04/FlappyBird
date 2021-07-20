using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour{
    #region Public Variables

    #endregion

    #region Private Variables

    #endregion

    #region Unity Methods
    
    private void Start() {
        //PlayerPrefs.SetInt("Highscore", 0);
        //PlayerPrefs.Save();
        BirdMovement.GetInstance().OnDie += BirdOnDied;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    private void BirdOnDied(object sender, EventArgs e){
        SetHighscore(LevelManager.getInstance().GetPipesPassed() / 2);
    }

    #endregion

    #region Utility Methods

    public int GetHighscore(){
        return PlayerPrefs.GetInt("Highscore");
    }

    public bool SetHighscore(int score){
        int oldScore = GetHighscore();
        if(score > oldScore){
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    #endregion
}
