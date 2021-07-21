using System;
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
    private const float CloudDestroyPositonX = -185.0f;
    private static LevelManager _instance;
    private List<Pipe> _pipeList;
    private List<Cloud> _frontCloudList;
    private List<Cloud> _backCloudList;
    private int _pipesPassedCount;
    private int _pipesSpawned;
    private float _pipeSpawnTimer;
    private float _pipeSpawnTimerMax;
    private float _gapSize;
    private State _gameState;

    private enum State{
        WaitingToStart,
        Playing,
        BirdDead
    }

    #endregion

    #region Unity Methods
    public static LevelManager getInstance(){
        return _instance;
    }

    private void Awake() {
        _instance = this;
        _frontCloudList = new List<Cloud>();
        _backCloudList = new List<Cloud>();
        SpawnInitialsClouds();
        _pipeList = new List<Pipe>();
        _pipeSpawnTimerMax = 1.0f;
        SetDifficulty(Difficulty.Easy);
        _gameState = State.WaitingToStart;
    }
    private void Start() {
        BirdMovement.GetInstance().OnDie += BirdOnDied;
        BirdMovement.GetInstance().OnStartPlaying += BirdOnStartPlaying;
    }
    
    private void Update() {
        if(_gameState == State.Playing){
            UpdatePipeMovement();
            UpdatePipeSpawning();
            UpdateClouds();
        }
    }

    #endregion

    #region Utility Methods
    
    private void SpawnInitialsClouds(){
        Cloud cloud;
        // Front clouds(white clouds)
        cloud = Instantiate(GameAssets.GetInstance().frontCloudGO.GetComponent<Cloud>());
        _frontCloudList.Add(cloud);
        cloud = Instantiate(GameAssets.GetInstance().frontCloudGO.GetComponent<Cloud>(), new Vector3(cloud.getWidth(), cloud.getPositionY(), 0.0f), Quaternion.identity);
        _frontCloudList.Add(cloud);
        cloud = Instantiate(GameAssets.GetInstance().frontCloudGO.GetComponent<Cloud>(), new Vector3(cloud.getWidth() * 2.0f, cloud.getPositionY(), 0.0f), Quaternion.identity);
        _frontCloudList.Add(cloud);

        // Back clouds (grey clouds)
        cloud = Instantiate(GameAssets.GetInstance().backCloudGO.GetComponent<Cloud>());
        _backCloudList.Add(cloud);
        cloud = Instantiate(GameAssets.GetInstance().backCloudGO.GetComponent<Cloud>(), new Vector3(cloud.getWidth(), cloud.getPositionY(), 0.0f), Quaternion.identity);
        _backCloudList.Add(cloud);
        cloud = Instantiate(GameAssets.GetInstance().backCloudGO.GetComponent<Cloud>(), new Vector3(cloud.getWidth() * 2.0f, cloud.getPositionY(), 0.0f), Quaternion.identity);
        _backCloudList.Add(cloud);
    }

    private void UpdatePipeMovement(){
        for(int x = 0; x < _pipeList.Count; x++){
            bool pipeOnRight = _pipeList[x].getPipeTransform().position.x > BirdPositionX;
            _pipeList[x].Move();
            if(pipeOnRight && _pipeList[x].getPipeTransform().position.x <= BirdPositionX){
                //Pipe passed bird
                _pipesPassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.Score);
            }
            if(_pipeList[x].getPipeTransform().position.x < PipeDestroyPositonX){
                _pipeList[x].DestroySelf();
                _pipeList.Remove(_pipeList[x]);
                x--; // Decrease x because u remove an item during for execution
            }
        }
    }

    private void UpdatePipeSpawning(){
        _pipeSpawnTimer -= Time.deltaTime;
        if(_pipeSpawnTimer < 0){
            _pipeSpawnTimer += _pipeSpawnTimerMax;
            float heightEdgeLimit = 12.0f;
            float minHeight = _gapSize * 0.5f + heightEdgeLimit;
            float maxHeight = (CameraOrtoSize * 2.0f) - (_gapSize * 0.5f) - heightEdgeLimit;
            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(PipeSpawnPositonX, height, _gapSize);
        }
    }

    

    private void UpdateClouds(){
        for(int x = 0; x < _frontCloudList.Count; x++){
            _frontCloudList[x].Move();

            if(_frontCloudList[x].getTransform().position.x < CloudDestroyPositonX){                // Cloud is out of the screen
                float rightMostPositionX = CameraOrtoSize * 2.0f;                                   // The right most position of the game screen
                for(int y = 0; y < _frontCloudList.Count; y++){
                    if(_frontCloudList[y].getTransform().position.x > rightMostPositionX){
                        rightMostPositionX = _frontCloudList[y].getTransform().position.x;
                    }
                }
                //Move the ground to the right most position
                _frontCloudList[x].getTransform().position = new Vector3(rightMostPositionX + _frontCloudList[x].getWidth(), _frontCloudList[x].getTransform().position.y, 0.0f);
            }
        }

        for(int x = 0; x < _backCloudList.Count; x++){
            _backCloudList[x].Move();

            if(_backCloudList[x].getTransform().position.x < CloudDestroyPositonX){                // Cloud is out of the screen
                float rightMostPositionX = CameraOrtoSize * 2.0f;                                   // The right most position of the game screen
                for(int y = 0; y < _backCloudList.Count; y++){
                    if(_backCloudList[y].getTransform().position.x > rightMostPositionX){
                        rightMostPositionX = _backCloudList[y].getTransform().position.x;
                    }
                }
                //Move the ground to the right most position
                _backCloudList[x].getTransform().position = new Vector3(rightMostPositionX + _backCloudList[x].getWidth() , _backCloudList[x].getTransform().position.y, 0.0f);
            }
        }
    }
    private void SetDifficulty(Difficulty difficulty){
        switch(difficulty){
            case Difficulty.Easy:
                _gapSize = 50.0f;
                _pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Medium:
                _gapSize = 35.0f;
                _pipeSpawnTimerMax = 1.0f;
                break;
            case Difficulty.Hard:
                _gapSize = 20.0f;
                _pipeSpawnTimerMax = 0.9f;
                break;
            case Difficulty.Impossible:
                _gapSize = 10.0f;
                _pipeSpawnTimerMax = 0.8f;
                break;
        }
    }

    private Difficulty GetDifficulty(){
        if(_pipesSpawned >= 60) return Difficulty.Impossible;
        if(_pipesSpawned >= 40) return Difficulty.Hard;
        if(_pipesSpawned >= 20) return Difficulty.Medium;
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

    private void BirdOnDied(object sender, EventArgs e){
        _gameState = State.BirdDead;
    }

    private void BirdOnStartPlaying(object sender, EventArgs e){
        _gameState = State.Playing;
    }

    #endregion
}