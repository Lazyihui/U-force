using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class WorldBehaviourModel {

        public bool isSpawnStuff;
        public StuffSo stuffSo;

        public void FormTM(WorldBehaviourTM tm) {
            isSpawnStuff = tm.isSpawnStuff;
            stuffSo = tm.stuffSo;
        }

    }
}