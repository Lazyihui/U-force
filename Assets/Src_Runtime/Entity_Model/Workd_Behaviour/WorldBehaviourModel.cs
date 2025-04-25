using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public struct WorldBehaviourModel {
        public bool isSpawnStuff;
        public StuffSo stuffSo;

        public bool isSpawnBullet;
        public BulletSo bulletSo;

        public bool isSpawnLaser;

        public bool isHaveCentred;

        public void FromTM(WorldBehaviourTM tm) {
            isSpawnStuff = tm.isSpawnStuff;
            stuffSo = tm.stuffSo;

            isSpawnBullet = tm.isSpawnBullet;
            bulletSo = tm.bulletSo;

            isSpawnLaser = tm.isSpawnLaser;

            isHaveCentred = tm.isHaveCentred;
        }

    }
}