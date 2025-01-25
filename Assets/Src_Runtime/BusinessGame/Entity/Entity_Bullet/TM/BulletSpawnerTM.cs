using System;
using UnityEngine;

namespace UF {

    [Serializable]
    public struct BulletSpawnerTM {
        public BulletSo bulletSo;

        public Transform spawnPos;

        public Transform spawnDir;
    }
}