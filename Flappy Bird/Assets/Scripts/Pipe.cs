using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour{
    #region Public Variables
    public Transform ownTransform;
    public Transform pipeHead;
    public SpriteRenderer pipeBodySpriteRender;
    public BoxCollider2D pipeBodyCollider;

    #endregion

    #region Private Variables
    private const float PipeMoveSpeed = 30.0f;

    #endregion

    #region Getters
    public Transform getPipeTransform() => ownTransform;
    public Transform getHead() => pipeHead;
    public SpriteRenderer getBodyRender() => pipeBodySpriteRender;
    public BoxCollider2D getBodyCollider() => pipeBodyCollider;

    #endregion

    #region Public Methods

    public void Move(){
        ownTransform.position += new Vector3(-1, 0, 0) * PipeMoveSpeed * Time.deltaTime;
    }

    public void DestroySelf(){
        Destroy(ownTransform.gameObject);
    }

    #endregion
}
