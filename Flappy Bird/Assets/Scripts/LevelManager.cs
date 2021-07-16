using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{
    #region Public Variables

    #endregion

    #region Private Variables
    private List<Pipe> _pipeList;
    private const float CameraOrtoSize = 50.0f;
    private const float PipeWidth = 7.8f;
    private const float PipeHeadHeight = 3.5f;
    private const float PipeDestroyPositonX = -100.0f;

    #endregion

    #region Unity Methods
    private void Awake() {
        _pipeList = new List<Pipe>();
    }
    private void Start() {
        //CreatePipe(20.0f, 50.0f, true);
        //CreatePipe(20.0f, 50.0f, false);
        CreateGapPipes(20.0f, 50.0f, 20f);
        CreateGapPipes(40.0f, 25.0f, 20f);
        CreateGapPipes(50.0f, 40.0f, 5f);
    }

    private void Update() {
        UpdatePipeMovement();
    }

    #endregion

    #region Utility Methods

    private void UpdatePipeMovement(){
        for(int x = 0; x < _pipeList.Count; x++){
            _pipeList[x].Move();
            if(_pipeList[x].getPipeTransform().position.x < PipeDestroyPositonX){
                _pipeList[x].DestroySelf();
                _pipeList.Remove(_pipeList[x]);
                x--; //Decrease x bc u remove an item during for execution
            }
        }
    }

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
        _pipeList.Add(pipe);
        
        if(!createBottom) pipe.getPipeTransform().localScale = new Vector3(1, -1, 1);
        
        //Height of pipe
        pipe.getHead().position =  new Vector3(positionX, pipeHeadPositionY);
        pipe.getBodyRender().size =  new Vector2(PipeWidth, height);
        pipe.getBodyCollider().size = new Vector2(PipeWidth, height);
        pipe.getBodyCollider().offset = new Vector2(0f, height * 0.5f);
    }

    #endregion
}