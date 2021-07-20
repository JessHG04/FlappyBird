using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingToStartWindow : MonoBehaviour{
    private void Start() {
        BirdMovement.GetInstance().OnStartPlaying += BirdOnStartPlaying;
    }

    private void BirdOnStartPlaying(object sender, EventArgs e) {
        Hide();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
}