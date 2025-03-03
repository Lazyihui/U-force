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
        public bool isRoleMoveDown; // 往下掉
        public bool isRoleMoveRight; // 击退


        public void FormTM(BulletBehaviourTM tm) {
            isMove = tm.isMove;
            isStop = tm.isStop;
            isHaveContinueDamege = tm.isHaveContinueDamege;
            isHaveContinueBoomDamege = tm.isHaveContinueBoomDamege;
            isDestorySelf = tm.isDestorySelf;

            // role身上的
            isRoleMoveSpeedDown = tm.isRoleMoveSpeedDown;
            isRoleMoveDown = tm.isRoleMoveDown;
            isRoleMoveRight = tm.isRoleMoveRight;
        }
    }
}