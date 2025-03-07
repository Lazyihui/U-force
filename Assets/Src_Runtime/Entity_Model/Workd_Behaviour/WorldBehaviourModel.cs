using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public struct WorldBehaviourModel {
        public bool isSpawnStuff;
        public StuffSo stuffSo;

        public void FromTM(WorldBehaviourTM tm) {
            isSpawnStuff = tm.isSpawnStuff;
            stuffSo = tm.stuffSo;
        }

    }
}