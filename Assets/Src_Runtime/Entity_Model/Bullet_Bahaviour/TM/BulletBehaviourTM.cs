using System;
using UnityEngine;

namespace UF {
    [Serializable]
    public struct BulletBehaviourTM {
        // -Move
        public bool isMoveTo;

        public bool isStop;

        [Header("role身上的")]
        public bool isRoleMoveSpeedDown;
    }
}