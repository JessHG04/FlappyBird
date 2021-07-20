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
        _highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }

    private void Update() {
        _scoreText.text = "Score: " + (LevelManager.getInstance().GetPipesPassed() / 2).ToString();
    }
}
