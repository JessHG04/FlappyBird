using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour{
    #region Public Variables

    #endregion

    #region Private Variables

    #endregion

    #region Unity Methods
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }


    #endregion
}
