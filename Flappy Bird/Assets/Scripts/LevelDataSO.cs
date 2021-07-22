using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Flappy Bird/LevelDataSO", order = 0)]
public class LevelDataSO : ScriptableObject {
    public List<LevelData> dataList = new List<LevelData>();
}

[Serializable]
public class LevelData{
    public float gapSize;
    public float pipeSpawnTimerMax;
}