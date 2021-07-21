using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour{
    #region Public Variables
    public GameObject pipeGO; // The pipe prefab
    public GameObject frontCloudGO; // The front cloud (white cloud) prefab
    public GameObject backCloudGO; // The back cloud (black cloud) prefab
    //public GameObject cloudGO; // The cloud prefab
    public SoundAudioClip[] soundAudioClips;

    #endregion

    #region Private Variables
    private static GameAssets _instance;

    #endregion
    
    private void Awake() {
        _instance = this;
    }

    public static GameAssets GetInstance(){
        return _instance;
    }
    
    [Serializable]
    public class SoundAudioClip{
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
