using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{
    #region Public Variables

    #endregion

    #region Private Variables
    private const float PipeWidth = 7.8f;
    private const float PipeHeadHeight = 3.5f;
    #endregion

    #region Unity Methods
    private void Start() {
        CreatePipe(10.0f, 10.0f);
        CreatePipe(20.0f, 30.0f);
        CreatePipe(30.0f, 40.0f);
    }

    #endregion

    #region Utility Methods
    private void CreatePipe(float positionX, float height){
        //Complete pipe position X
        var pipe = Instantiate(GameAssets.GetInstance().pipeGO.GetComponent<Pipe>());
        pipe.getPipeTransform().position = new Vector3(positionX, 0f);
        
        //Height of pipe
        pipe.getHead().position =  new Vector3(positionX, height - PipeHeadHeight * 0.5f);
        pipe.getBodyRender().size =  new Vector2(PipeWidth, height);
        pipe.getBodyCollider().size = new Vector2(PipeWidth, height);
        pipe.getBodyCollider().offset = new Vector2(0f, height * 0.5f);
    }

    #endregion
}