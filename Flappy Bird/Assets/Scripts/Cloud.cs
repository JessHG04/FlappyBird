using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour{
    #region Public Variables
    public Transform cloudTransform;
    public SpriteRenderer cloudSpriteRenderer;
    public bool isBackCloud;

    #endregion

    #region Private Variables
    private float _cloudMoveSpeed = 40.0f;

    #endregion

    private void Start() {
        if(isBackCloud){
            _cloudMoveSpeed = 20.0f;
        }
    }

    #region Public Methods
    public Transform getTransform() => cloudTransform;
    public float getWidth() => cloudSpriteRenderer.size.x;
    public float getPositionY() => transform.position.y;

    public void Move(){
        cloudTransform.position += new Vector3(-1, 0, 0) * _cloudMoveSpeed * Time.deltaTime;
    }

    #endregion

}
