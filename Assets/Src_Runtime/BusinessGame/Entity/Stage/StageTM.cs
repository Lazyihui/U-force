using System;
using UnityEngine;

namespace UF {

    [Serializable]

    public class StageTM {
        public int stage;

        public RoleSpawnerTM[] roleSpawns;
        public TowerSpawnTM[] towerSpawns;

    }
}