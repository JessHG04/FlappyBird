using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{
    #region Public Variables

    #endregion

    #region Private Variables
    private const float CameraOrtoSize = 50.0f;
    private const float PipeWidth = 7.8f;
    private const float PipeHeadHeight = 3.5f;
    #endregion

    #region Unity Methods
    private void Start() {
        //CreatePipe(20.0f, 50.0f, true);
        //CreatePipe(20.0f, 50.0f, false);
        CreateGapPipes(20.0f, 50.0f, 20f);
        CreateGapPipes(30.0f, 25.0f, 20f);
    }

    #endregion

    #region Utility Methods

    private void CreateGapPipes(float positionX, float gapY, float gapSize){
        CreatePipe(positionX, gapY - gapSize * 0.5f, true);
        CreatePipe(positionX, CameraOrtoSize * 2.0f - gapY - gapSize * 0.5f, false);
    }

    private void CreatePipe(float positionX, float height, bool createBottom){
        float pipePositionY, pipeHeadPositionY;

        if(createBottom){
            pipePositionY = -CameraOrtoSize;
            pipeHeadPositionY = (-CameraOrtoSize) + height - (PipeHeadHeight * 0.5f);
        }else{
            pipePositionY = +CameraOrtoSize;
            pipeHeadPositionY = (+CameraOrtoSize) - height + (PipeHeadHeight * 0.5f);
        }
        
        //Complete pipe position X
        var pipe = Instantiate(GameAssets.GetInstance().pipeGO.GetComponent<Pipe>());
        pipe.getPipeTransform().position = new Vector3(positionX, pipePositionY);
        
        if(!createBottom) pipe.getPipeTransform().localScale = new Vector3(1, -1, 1);
        
        //Height of pipe
        pipe.getHead().position =  new Vector3(positionX, pipeHeadPositionY);
        pipe.getBodyRender().size =  new Vector2(PipeWidth, height);
        pipe.getBodyCollider().size = new Vector2(PipeWidth, height);
        pipe.getBodyCollider().offset = new Vector2(0f, height * 0.5f);
    }

    #endregion
}