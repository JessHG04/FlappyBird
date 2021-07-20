using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour{
    #region Public Variables
    public Transform cloudTransform;

    #endregion

    #region Private Variables
    private const float cloudMoveSpeed = 30.0f;

    #endregion

    #region Getters
    public Transform getCloudTransform() => cloudTransform;
    public float getPositionY() => transform.position.y;

    #endregion

    #region Public Methods
    public void Move(){
        cloudTransform.position += new Vector3(-1, 0, 0) * cloudMoveSpeed * Time.deltaTime * 0.75f;
    }

    public void DestroySelf(){
        Destroy(cloudTransform.gameObject);
    }

    #endregion

}
