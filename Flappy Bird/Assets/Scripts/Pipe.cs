using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour{
    
    public Transform ownTransform;
    public Transform pipeHead;
    public SpriteRenderer pipeBodySpriteRender;
    public BoxCollider2D pipeBodyCollider;

    public Transform getPipeTransform() => ownTransform;
    public Transform getHead() => pipeHead;
    public SpriteRenderer getBodyRender() => pipeBodySpriteRender;
    public BoxCollider2D getBodyCollider() => pipeBodyCollider;

}
