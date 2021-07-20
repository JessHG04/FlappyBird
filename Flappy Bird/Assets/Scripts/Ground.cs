using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour{
    #region Public Variables
    public Transform groundTransform;
    public SpriteRenderer groundSpriteRenderer;


    //public BoxCollider2D groundCollider;

    #endregion

    #region Private Variables
    private const float GroundMoveSpeed = 30.0f;

    #endregion

    #region Getters
    public Transform getGroundTransform() => groundTransform;
    public SpriteRenderer getGroundRender() => groundSpriteRenderer;
    public float getGroundWidth() => groundSpriteRenderer.size.x;

    public float getGroundHeight() => groundSpriteRenderer.size.y;

    public float getPositionY() => transform.position.y;

    public float getPositionX() => transform.position.x;
    //public BoxCollider2D getGroundCollider() => groundColider;

    #endregion

    #region Public Methods
    public void Move(){
        groundTransform.position += new Vector3(-1, 0, 0) * GroundMoveSpeed * Time.deltaTime;
    }

    #endregion

}
