using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour{
    #region Public Variables

    #endregion

    #region Private Variables

    private static GameHandler _instance;

    #endregion

    #region Unity Methods
    private void Awake() {
        _instance = this;
    }
    private void Start() {
        //PlayerPrefs.SetInt("Highscore", 0);
        //PlayerPrefs.Save();
        BirdMovement.GetInstance().OnDie += BirdOnDied;
    }

    public static GameHandler GetInstance(){
        return _instance;
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
