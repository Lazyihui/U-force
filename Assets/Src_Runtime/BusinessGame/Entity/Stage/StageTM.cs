using System;
using UnityEngine;

namespace UF {

    [Serializable]

    public class StageTM {
        public int stageID;

        public RoleSpawnerTM[] roleSpawns;
        public CaveSpawnerTM[] towerSpawns;

        // map 
        public GameObject mapPrefab;

    }
}