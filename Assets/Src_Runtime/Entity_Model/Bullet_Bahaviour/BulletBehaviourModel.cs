using System;
using UnityEngine;

namespace UF {

    public struct BulletBehaviourModel {

        public bool isMove;
        public bool isStop;
        public bool isHaveContinueDamege;
        public bool isHaveContinueBoomDamege;

        public bool isDestorySelf;

        // role身上的
        public bool isRoleMoveSpeedDown;


        public void FormTM(BulletBehaviourTM tm) {
            isMove = tm.isMove;
            isStop = tm.isStop;
            isHaveContinueDamege = tm.isHaveContinueDamege;
            isHaveContinueBoomDamege = tm.isHaveContinueBoomDamege;
            isDestorySelf = tm.isDestorySelf;

            // role身上的
            isRoleMoveSpeedDown = tm.isRoleMoveSpeedDown;
        }
    }
}