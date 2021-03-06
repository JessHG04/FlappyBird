using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour{
    private Text _scoreText;
    private Text _highscoreText;

    private void Start(){
        BirdMovement.GetInstance().OnDie += BirdOnDied;
        _scoreText = transform.Find("ScoreText").GetComponent<Text>();
        _highscoreText = transform.Find("HighscoreText").GetComponent<Text>();
        Hide();
    }

    private void BirdOnDied(object sender, EventArgs e){
        _scoreText.text = "Score: " + (LevelManager.getInstance().GetScore()).ToString();
        if((LevelManager.getInstance().GetScore()) >= GameHandler.GetInstance().GetHighscore()){
            _highscoreText.text = "NEW HIGHSCORE!";
        }else{
            _highscoreText.text = "Highscore: " + GameHandler.GetInstance().GetHighscore().ToString();
        }

        Show();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    public void RetryButtonClicked() {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void ShareButtonClicked() {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Debug.Log("Share button clicked");
    }
    
    public void MainMenuButtonClicked() {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
