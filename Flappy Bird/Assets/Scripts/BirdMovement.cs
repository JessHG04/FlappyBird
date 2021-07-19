using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour{
    #region Private Variables
    private Rigidbody2D _rigidbody2D;
    private const float JumpAmount = 100.0f;

    #endregion

    #region Unity Methods
    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            Jump();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Collision");
    }

    #endregion

    #region Utility Methods
    private void Jump(){
        _rigidbody2D.velocity = Vector2.up * JumpAmount;
    }

    #endregion
}
