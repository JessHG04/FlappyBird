using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour{
    #region Public Variables
    public GameObject pipeGO;

    #endregion

    #region Private Variables
    private static GameAssets _instance;

    #endregion
    
    #region Unity Methods
    private void Awake() {
        _instance = this;
    }

    public static GameAssets GetInstance(){
        return _instance;
    }
    
    #endregion
}
