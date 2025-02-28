using System;
using UnityEngine;

namespace UF {

    public struct BulletBehaviourModel {

        public bool isMove;

        public bool isBomb;

        public void FormTM(BulletBehaviourTM tm) {
            isMove = tm.isMoveTo;
            isBomb = tm.isBomb;
        }
    }
}