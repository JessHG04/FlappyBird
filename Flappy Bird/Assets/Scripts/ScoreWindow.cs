using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour{
    private Text _scoreText;
    private Text _highscoreText;
    private void Awake() {
        _scoreText = transform.Find("ScoreText").GetComponent<Text>();
        _highscoreText = transform.Find("HighscoreText").GetComponent<Text>();
    }

    private void Start() {
        BirdMovement.GetInstance().OnDie += BirdOnDie;
        BirdMovement.GetInstance().OnStartPlaying += BirdOnStartPlaying;
        _highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }
    
    private void Update() {
        _scoreText.text = "Score: " + (LevelManager.getInstance().GetScore()).ToString();
    }

    private void BirdOnDie(object sender, EventArgs e) {
        Hide();
    }

    private void BirdOnStartPlaying(object sender, EventArgs e) {
        Show();
    }
    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Show(){
        gameObject.SetActive(true);
    }
}
