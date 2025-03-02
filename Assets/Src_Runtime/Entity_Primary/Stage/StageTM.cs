using System;
using UnityEngine;

namespace UF {

    [Serializable]

    public class StageTM {
        public int stageID;

        public RoleSpawnerTM[] roleSpawns;
        public CaveSpawnerTM[] towerSpawns;
        public StuffSpawnerTM[] stuffSpawns; // x值: -1 ~ 19, y值: -10 ~ 10(极限, 实际根据圆而定)

        // map 
        public GameObject mapPrefab;

        // 数据
        public GameTM gameTM;

    }
}