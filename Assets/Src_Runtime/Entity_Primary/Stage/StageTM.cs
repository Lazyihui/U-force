using System;
using UnityEngine;

namespace UF {

    [Serializable]

    public class StageTM {
        public int stageID;

        public RoleSpawnerTM[] roleSpawns;
        public CaveSpawnerTM[] towerSpawns;
        public StuffSpawnerTM[] stuffSpawns;

        // map 
        public GameObject mapPrefab;

        // 数据
        public GameTM gameTM;

    }
}