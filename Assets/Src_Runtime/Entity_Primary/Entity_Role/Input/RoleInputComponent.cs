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

        // 训练营
        bool isPlayer1Jump;
        public bool IsPlayer1Jump => isPlayer1Jump;
        public void Player1Jump_Set(bool value) => isPlayer1Jump = value;
        public bool Player1Jump_Get() => isPlayer1Jump;

        bool isPlayer1Attack;
        public bool IsPlayer1Attack => isPlayer1Attack;
        public void Player1Attack_Set(bool value) => isPlayer1Attack = value;
        public bool Player1Attack_Get() => isPlayer1Attack;

        bool isPlayer1Defense;
        public bool IsPlayer1Defense => isPlayer1Defense;
        public void Player1Defense_Set(bool value) => isPlayer1Defense = value;
        public bool Player1Defense_Get() => isPlayer1Defense;

        bool isPlayer2Jump;
        public bool IsPlayer2Jump => isPlayer2Jump;
        public void Player2Jump_Set(bool value) => isPlayer2Jump = value;
        public bool Player2Jump_Get() => isPlayer2Jump;

        bool isPlayer2Attack;
        public bool IsPlayer2Attack => isPlayer2Attack;
        public void Player2Attack_Set(bool value) => isPlayer2Attack = value;
        public bool Player2Attack_Get() => isPlayer2Attack;

        bool isPlayer2Defense;
        public bool IsPlayer2Defense => isPlayer2Defense;
        public void Player2Defense_Set(bool value) => isPlayer2Defense = value;
        public bool Player2Defense_Get() => isPlayer2Defense;

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