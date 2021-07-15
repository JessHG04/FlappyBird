using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour{
    #region Public Variables
    public Sprite pipeHeadSprite;

    #endregion

    #region Private Variables
    private static GameAssets instance;

    #endregion
    
    public static GameAssets GetInstance(){
        return instance;
    }

    private void Awake() {
        instance = this;
    }
}
