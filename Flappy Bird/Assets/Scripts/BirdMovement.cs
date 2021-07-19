using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour{
    #region Public Variables
    public event EventHandler OnDie;
    public event EventHandler OnStartPlaying;

    #endregion

    #region Private Variables
    private static BirdMovement _instance;
    private Rigidbody2D _rigidbody2D;
    private const float JumpAmount = 100.0f;

    private State _birdState;
    private enum State{
        WaitingToStart,
        Playing,
        Dead
    }

    #endregion

    #region Unity Methods

    public static BirdMovement GetInstance(){
        return _instance;
    }
    private void Awake() {
        _instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    private void Update() {
        switch (_birdState) {
            case State.WaitingToStart:
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
                    _birdState = State.Playing;
                    _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if(OnStartPlaying != null) OnStartPlaying(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }

        
    }
    private void OnTriggerEnter2D(Collider2D collider) {
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        if(OnDie != null) OnDie(this, EventArgs.Empty);
    }

    #endregion

    #region Utility Methods
    private void Jump(){
        if(_rigidbody2D.bodyType == RigidbodyType2D.Dynamic){
            _rigidbody2D.velocity = Vector2.up * JumpAmount;
            SoundManager.PlaySound(SoundManager.Sound.BirdJump);
        }
    }

    #endregion
}
