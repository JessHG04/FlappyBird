using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour{
    #region Public Variables
    public Transform groundTransform;
    public SpriteRenderer groundSpriteRenderer;

    #endregion

    #region Private Variables
    private const float GroundMoveSpeed = 30.0f;

    #endregion

    #region Getters
    public Transform getGroundTransform() => groundTransform;
    public float getGroundWidth() => groundSpriteRenderer.size.x;
    public float getPositionY() => transform.position.y;


    #endregion

    #region Public Methods
    public void Move(){
        groundTransform.position += new Vector3(-1, 0, 0) * GroundMoveSpeed * Time.deltaTime;
    }

    #endregion

}
