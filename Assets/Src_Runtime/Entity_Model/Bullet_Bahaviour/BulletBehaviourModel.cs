using System;
using UnityEngine;

namespace UF {

    public struct BulletBehaviourModel {

        public bool isMove;

        public void FormTM(BulletBehaviourTM tm) {
            isMove = tm.isMoveTo;
        }
    }
}