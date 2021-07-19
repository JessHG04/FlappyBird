using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour{
    #region Public Variables
    public enum Difficulty{
        Easy,
        Medium,
        Hard,
        Impossible
    }

    #endregion

    #region Private Variables
    private const float CameraOrtoSize = 50.0f;
    private const float BirdPositionX = 0.0f;
    private const float PipeWidth = 7.8f;
    private const float PipeHeadHeight = 3.5f;
    private const float PipeDestroyPositonX = -100.0f;
    private const float PipeSpawnPositonX = 100.0f;
    private static LevelManager _instance;
    private List<Pipe> _pipeList;
    private int _pipesPassedCount;
    private int _pipesSpawned;
    private float _pipeSpawnTimer;
    private float _pipeSpawnTimerMax;
    private float gapSize;

    #endregion

    #region Unity Methods
    public static LevelManager getInstance(){
        return _instance;
    }

    private void Awake() {
        _instance = this;
        _pipeList = new List<Pipe>();
        _pipeSpawnTimerMax = 1.0f;
        SetDifficulty(Difficulty.Easy);
    }
    
    private void Update() {
        UpdatePipeMovement();
        UpdatePipeSpawning();
    }

    #endregion

    #region Utility Methods

    private void UpdatePipeMovement(){
        for(int x = 0; x < _pipeList.Count; x++){
            bool pipeOnRight = _pipeList[x].getPipeTransform().position.x > BirdPositionX;
            _pipeList[x].Move();
            if(pipeOnRight && _pipeList[x].getPipeTransform().position.x <= BirdPositionX){
                //Pipe passed bird
                _pipesPassedCount++;
            }
            if(_pipeList[x].getPipeTransform().position.x < PipeDestroyPositonX){
                _pipeList[x].DestroySelf();
                _pipeList.Remove(_pipeList[x]);
                x--; // Decrease x bc u remove an item during for execution
            }
        }
    }

    private void UpdatePipeSpawning(){
        _pipeSpawnTimer -= Time.deltaTime;
        if(_pipeSpawnTimer < 0){
            _pipeSpawnTimer += _pipeSpawnTimerMax;
            float heightEdgeLimit = 10.0f;
            float minHeight = gapSize * 0.5f + heightEdgeLimit;
            float maxHeight = (CameraOrtoSize * 2.0f) - (gapSize * 0.5f) - heightEdgeLimit;
            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(PipeSpawnPositonX, height, gapSize);
        }
    }

    private void SetDifficulty(Difficulty difficulty){
        switch(difficulty){
            case Difficulty.Easy:
                gapSize = 50.0f;
                _pipeSpawnTimerMax = 1.2f;
                break;
            case Difficulty.Medium:
                gapSize = 40.0f;
                _pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Hard:
                gapSize = 30.0f;
                _pipeSpawnTimerMax = 1.0f;
                break;
            case Difficulty.Impossible:
                gapSize = 20.0f;
                _pipeSpawnTimerMax = 0.9f;
                break;
        }
    }

    private Difficulty GetDifficulty(){
        if(_pipesSpawned >= 30) return Difficulty.Impossible;
        if(_pipesSpawned >= 20) return Difficulty.Hard;
        if(_pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void CreateGapPipes(float positionX, float gapY, float gapSize){
        CreatePipe(positionX, gapY - gapSize * 0.5f, true);
        CreatePipe(positionX, CameraOrtoSize * 2.0f - gapY - gapSize * 0.5f, false);
        _pipesSpawned++;
        SetDifficulty(GetDifficulty());
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
        
        // Complete pipe position X
        var pipe = Instantiate(GameAssets.GetInstance().pipeGO.GetComponent<Pipe>());
        pipe.getPipeTransform().position = new Vector3(positionX, pipePositionY);
        _pipeList.Add(pipe);
        
        if(!createBottom) pipe.getPipeTransform().localScale = new Vector3(1, -1, 1);
        
        // Height of pipe
        pipe.getHead().position = new Vector3(positionX, pipeHeadPositionY);
        pipe.getBodyRender().size = new Vector2(PipeWidth, height);
        pipe.getBodyCollider().size = new Vector2(PipeWidth, height);
        pipe.getBodyCollider().offset = new Vector2(0f, height * 0.5f);
    }

    public int GetPipesSpawned(){
        return _pipesSpawned;
    }

    public int GetPipesPassed(){
        return _pipesPassedCount;
    }

    #endregion
}