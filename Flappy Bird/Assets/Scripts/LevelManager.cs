using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{
    #region Public Variables

    #endregion

    #region Private Variables

    #endregion

    #region Unity Methods
    private void Start() {
        //CreatePipe(0.0f, 0.0f);
    }

    #endregion

    #region Utility Methods
    private void CreatePipe(float height, float positionX){
        Transform pipeTransform = Instantiate(GameAssets.GetInstance().ptPipe);
        pipeTransform.position = new Vector3(positionX, 0f);
    }

    #endregion
}