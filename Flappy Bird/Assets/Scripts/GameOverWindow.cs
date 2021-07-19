using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour{
    private Text _scoreText;

    private void Start(){
        BirdMovement.GetInstance().OnDie += BirdOnDied;
        _scoreText = transform.Find("ScoreText").GetComponent<Text>();
        Hide();
    }

    private void BirdOnDied(object sender, EventArgs e){
        _scoreText.text = "Score: " + (LevelManager.getInstance().GetPipesPassed() / 2.0f).ToString();
        Show();
    }

    public void RetryButtonClicked() {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
