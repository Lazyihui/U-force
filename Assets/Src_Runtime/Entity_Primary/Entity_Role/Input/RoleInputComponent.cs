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

        // 进攻战移动
        Vector2 moveDir;
        public Vector2 MoveDir => moveDir;
        public void MoveDir_Set(Vector2 value) => moveDir = value;

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