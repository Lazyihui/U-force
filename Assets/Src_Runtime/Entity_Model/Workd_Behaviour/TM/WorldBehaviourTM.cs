using System;
using System.Collections;
using UnityEngine;
using TriInspector;

namespace UF {
    [Serializable]
    public struct WorldBehaviourTM {
        public bool isSpawnStuff;
        [ShowIf(nameof(isSpawnStuff))] public StuffSo stuffSo;

        public bool isSpawnBullet;
        [ShowIf(nameof(isSpawnBullet))] public BulletSo bulletSo;
        
        public bool isSpawnLaser;
        [ShowIf(nameof(isSpawnLaser))] public LaserSo laserSo;

        public bool isHaveCentred;
    }
}