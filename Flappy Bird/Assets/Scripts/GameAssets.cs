using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour{
    #region Public Variables
    public GameObject pipeGO;
    //public Transform ptPipeHead;
    //public Transform ptPipeBody;

    #endregion

    #region Private Variables
    private static GameAssets instance;

    #endregion
    
    #region Unity Methods
    private void Awake() {
        instance = this;
    }

    public static GameAssets GetInstance(){
        return instance;
    }
    
    #endregion
}