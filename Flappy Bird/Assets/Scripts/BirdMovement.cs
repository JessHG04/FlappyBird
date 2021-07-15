using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour{
    #region Private Variables
    private Rigidbody2D _rigidbody2D;
    private const float JumpAmount = 100.0f;

    #endregion

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }
    }

    private void Jump(){
        _rigidbody2D.velocity = Vector2.up * JumpAmount;
    }
}
