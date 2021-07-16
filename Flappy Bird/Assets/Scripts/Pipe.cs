using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour{
    
    private const float PipeMoveSpeed = 10.0f;
    public Transform ownTransform;
    public Transform pipeHead;
    public SpriteRenderer pipeBodySpriteRender;
    public BoxCollider2D pipeBodyCollider;

    public Transform getPipeTransform() => ownTransform;
    public Transform getHead() => pipeHead;
    public SpriteRenderer getBodyRender() => pipeBodySpriteRender;
    public BoxCollider2D getBodyCollider() => pipeBodyCollider;

    public void Move(){
        ownTransform.position += new Vector3(-1, 0, 0) * PipeMoveSpeed * Time.deltaTime;
    }

    public void DestroySelf(){
        Destroy(ownTransform.gameObject);
    }
}
