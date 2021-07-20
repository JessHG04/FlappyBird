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
    private const float GroundDestroyPositonX = -200.0f;
    private const float CloudSpawnPositonX = 160.0f;
    private const float CloudDestroyPositonX = -160.0f;
    private static LevelManager _instance;
    private List<Pipe> _pipeList;
    private List<Ground> _groundList;
    private List<Cloud> _cloudList;
    private int _pipesPassedCount;
    private int _pipesSpawned;
    private float _pipeSpawnTimer;
    private float _pipeSpawnTimerMax;
    private float _cloudSpawnTimer;
    private float _cloudSpawnTimerMax;
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
        _groundList = new List<Ground>();
        SpawnInitialsGrounds();
        _cloudList = new List<Cloud>();
        SpawnInitialClouds();
        _cloudSpawnTimerMax = 6.0f;
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
            UpdateGround();
            UpdateClouds();
        }
    }

    #endregion

    #region Utility Methods
    
    private void SpawnInitialsGrounds(){
        Ground ground;
        ground = Instantiate(GameAssets.GetInstance().groundGO.GetComponent<Ground>());
        _groundList.Add(ground);
        ground = Instantiate(GameAssets.GetInstance().groundGO.GetComponent<Ground>(), new Vector3(ground.getGroundWidth(), ground.getPositionY(), 0.0f), Quaternion.identity);
        _groundList.Add(ground);
        ground = Instantiate(GameAssets.GetInstance().groundGO.GetComponent<Ground>(), new Vector3(ground.getGroundWidth() * 2.0f, ground.getPositionY(), 0.0f), Quaternion.identity);
        _groundList.Add(ground);
    }

    private void SpawnInitialClouds(){
        Cloud cloud = Instantiate(GameAssets.GetInstance().cloudGO.GetComponent<Cloud>());
        _cloudList.Add(cloud);
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
            float heightEdgeLimit = 10.0f;
            float minHeight = _gapSize * 0.5f + heightEdgeLimit;
            float maxHeight = (CameraOrtoSize * 2.0f) - (_gapSize * 0.5f) - heightEdgeLimit;
            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(PipeSpawnPositonX, height, _gapSize);
        }
    }

    

    private void UpdateGround(){
        for(int x = 0; x < _groundList.Count; x++){
            _groundList[x].Move();

            if(_groundList[x].getGroundTransform().position.x < GroundDestroyPositonX){         // Ground is out of the screen
                float rightMostPositionX = CameraOrtoSize * 2.0f;                               // The right most position of the game screen
                for(int y = 0; y < _groundList.Count; y++){
                    if(_groundList[y].getGroundTransform().position.x > rightMostPositionX){
                        rightMostPositionX = _groundList[y].getGroundTransform().position.x;
                    }
                }
                //Move the ground to the right most position
                float groundWithHalf = _groundList[x].getGroundWidth() * 0.5f;
                _groundList[x].getGroundTransform().position = new Vector3(rightMostPositionX + groundWithHalf, _groundList[x].getGroundTransform().position.y, 0.0f);
            }
        }
    }

    private void UpdateClouds(){
        _cloudSpawnTimer -= Time.deltaTime;
        if(_cloudSpawnTimer < 0){
            _cloudSpawnTimer += _cloudSpawnTimerMax;
            Cloud cloud = Instantiate(GameAssets.GetInstance().cloudGO.GetComponent<Cloud>(), new Vector3 (CloudSpawnPositonX, _cloudList[0].getPositionY(), 0.0f), Quaternion.identity);
            _cloudList.Add(cloud);
        }

        for(int x = 0; x < _cloudList.Count; x++){
            _cloudList[x].Move();
            if(_cloudList[x].getCloudTransform().position.x < CloudDestroyPositonX){
                _cloudList[x].DestroySelf();
                _cloudList.Remove(_cloudList[x]);
                x--; // Decrease x because u remove an item during for execution
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