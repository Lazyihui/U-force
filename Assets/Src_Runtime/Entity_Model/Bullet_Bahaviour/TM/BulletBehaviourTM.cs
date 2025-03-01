using System;
using UnityEngine;

namespace UF {
    [Serializable]
    public struct BulletBehaviourTM {
        // -Move
        public bool isMove;
        public bool isStop;
        public bool isHaveContinueDamege;
        public bool isHaveContinueBoomDamege;
        public bool isDestorySelf;

        public bool isRoleMoveSpeedDown;
    }
}