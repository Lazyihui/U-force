using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    [Serializable]
    public class RoleInpuComponent {

        // -Jump
        bool isJump;
        public bool IsJump => isJump;
        public void Jump_Set(bool value) => isJump = value;

        // -input
        InputKeyFlag castKeys;
        public InputKeyFlag CastKeys => castKeys;



        public RoleInpuComponent() {

        }

        public void Clear() {
            isJump = false;
            castKeys = InputKeyFlag.None;
        }

        public void Release() {
            Clear();
        }
        public void RecoredCastKey(InputKeyFlag key) {

            if (key == InputKeyFlag.None) {
                return;
            }
            castKeys = key;
        }
    }
}