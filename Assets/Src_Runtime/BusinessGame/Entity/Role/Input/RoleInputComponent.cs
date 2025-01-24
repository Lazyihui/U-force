using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    [Serializable]
    public class RoleInputComponent {

        // -Jump
        bool isJump;
        public bool IsJump => isJump;
        public void Jump_Set(bool value) => isJump = value;
        public bool Jump_Get() => isJump;


        // -input
        InputKeyFlag castKeys;
        public InputKeyFlag CastKeys => castKeys;

        public RoleInputComponent() {

        }
        public void RecoredCastKey(InputKeyFlag key) {
            if (key == InputKeyFlag.None) {
                return;
            }
            castKeys = key;
        }
        public void Clear() {
            isJump = false;
            castKeys = InputKeyFlag.None;
        }

        public void ClearCastKeys() {
            castKeys = InputKeyFlag.None;
        }

        public void Release() {
            Clear();
        }



    }
}