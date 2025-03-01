using System;
using UnityEngine;

namespace UF {

    public struct BulletBehaviourModel {

        public bool isMove;

        public bool isStop;

        // role身上的
        public bool isRoleMoveSpeedDown;

        public void FormTM(BulletBehaviourTM tm) {
            isMove = tm.isMoveTo;
            isStop = tm.isStop;

            // role身上的
            isRoleMoveSpeedDown = tm.isRoleMoveSpeedDown;
        }
    }
}