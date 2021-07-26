using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour{
    #region Public Variables
    public LevelDataSO levelDataSO;

    #endregion

    #region Private Variables
    private const float CameraOrtoSize = 50.0f;
    private const float BirdPositionX = 0.0f;
    private const float PipeWidth = 7.8f;
    private const float PipeHeadHeight = -2.9f;
    private const float PipeDestroyPositonX = -100.0f;
    private const float PipeSpawnPositonX = 100.0f;
    private const float CloudDestroyPositonX = -185.0f;
    private static LevelManager _instance;
    private List<Pipe> _pipeList;
    private List<Cloud> _frontCloudList;
    private List<Cloud> _backCloudList;
    private float _score;
    private int _pipesSpawned;
    private int _currentLevel;
    private int _maxLevels;
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
        _currentLevel = 0;
        _maxLevels = levelDataSO.dataList.Count;
        LoadLevelData();
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
    /*
    *  _pipeList contains all the pipes in the game
    *  _pipeList = [pipeBottom, pipeTop, pipeBottom, pipeTop, ...]
    *               |   PAR 1     |         |    PAR 2   |   
    *  When X%2 = 0, the pipe is at the bottom of the screen, and his pair is x+1 pipes away
    */


    private void UpdatePipeMovement(){
        for(int x = 0; x < _pipeList.Count; x++){
            bool pipeOnRight = _pipeList[x].getPipeTransform().position.x > BirdPositionX;
            _pipeList[x].Move();
            if(pipeOnRight && _pipeList[x].getPipeTransform().position.x <= BirdPositionX){
                //Pipe passed bird
                if(x%2 == 0){
                    UpdateScore(x);
                }
                SoundManager.PlaySound(SoundManager.Sound.Score);
            }
            if(_pipeList[x].getPipeTransform().position.x < PipeDestroyPositonX){
                _pipeList[x].DestroySelf();
                _pipeList.Remove(_pipeList[x]);
                x--; // Decrease x because u remove an item during execution
            }
        }
    }


    private void UpdateScore(int listPosition){    
        bool isTop = false;     //listPosition is the position of the pipe in the list
        float maxScore = 100;
        float scoreMultiplier = 0.0f;
        float positionUp   = _pipeList[listPosition + 1].getHead().position.y - (_pipeList[listPosition + 1].getHeadHeight() /2.0f);
        float positionDown = _pipeList[listPosition].getHead().position.y  + (_pipeList[listPosition].getHeadHeight() /2.0f);
        float center = positionDown + (_gapSize/2.0f);
        float birdPositionY = BirdMovement.GetInstance().getPositionY();
        float birdPosYPercentage = birdPositionY;
        int percentage = 0;

        if(birdPositionY > center){
            isTop = true;
            birdPosYPercentage = birdPositionY - (_gapSize/2.0f);
        }

        percentage = (int) ((birdPosYPercentage - center) / (positionDown - center) * 100.0f);

        if(isTop){
            scoreMultiplier = percentage / 100.0f;
        }else{
            scoreMultiplier = (100.0f - percentage) / 100.0f;
        }
       
        var go = Instantiate(GameAssets.GetInstance().checkGO, new Vector3(BirdPositionX, birdPositionY, 0.0f), Quaternion.identity, _pipeList[listPosition].getPipeTransform());
        var text = go.GetComponentInChildren<TextMesh>();
        text.text = (maxScore * scoreMultiplier).ToString();
        _score += maxScore * scoreMultiplier;
    }

    private void UpdatePipeSpawning(){
        _pipeSpawnTimer -= Time.deltaTime;
        if(_pipeSpawnTimer < 0){
            _pipeSpawnTimer += _pipeSpawnTimerMax;
            float heightEdgeLimit = 12.0f;
            float minHeight = _gapSize * 0.5f + heightEdgeLimit;
            float maxHeight = (CameraOrtoSize * 2.0f) - (_gapSize * 0.5f) - heightEdgeLimit;
            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(PipeSpawnPositonX, height, _gapSize - PipeHeadHeight);
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

            if(_backCloudList[x].getTransform().position.x < CloudDestroyPositonX){                 // Cloud is out of the screen
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

    private void LoadLevelData(){
        if(_currentLevel < _maxLevels){
            _gapSize = levelDataSO.dataList[_currentLevel].gapSize;
            _pipeSpawnTimerMax = levelDataSO.dataList[_currentLevel].pipeSpawnTimerMax;
        }else{
            _gapSize = levelDataSO.dataList[_maxLevels - 1].gapSize;
            _pipeSpawnTimerMax = levelDataSO.dataList[_maxLevels - 1].pipeSpawnTimerMax;
        }
    }

    private void CreateGapPipes(float positionX, float gapY, float gapSize){
        CreatePipe(positionX, gapY - gapSize * 0.5f, true);
        CreatePipe(positionX, CameraOrtoSize * 2.0f - gapY - gapSize * 0.5f, false);
        _pipesSpawned++;

        if(_pipesSpawned >= levelDataSO.pipesPerLevel){
            //Load next level
            _currentLevel++;
            LoadLevelData();
            _pipesSpawned = 0;
        }
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

    public float GetScore(){
        return _score;
    }

    private void BirdOnDied(object sender, EventArgs e){
        _gameState = State.BirdDead;
    }

    private void BirdOnStartPlaying(object sender, EventArgs e){
        _gameState = State.Playing;
    }

    #endregion
}