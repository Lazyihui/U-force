using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class InputModule {

        InputControls_Player player1;
        public InputControls_Player Player1 => player1;

        InputKeyFlag inputCastKey;
        public InputKeyFlag InputCastKey => inputCastKey;

        bool isJump;
        public bool IsJump => isJump;

        public InputModule() {
            player1 = new InputControls_Player();
            player1.Enable();
        }

        public void ReSetJump() {
            isJump = false;
        }

        public void Process(float dt) {
            var world = player1.World;

            // -Input
            {
                bool isJumpDown = world.Jump.WasPressedThisFrame();
                isJump = isJumpDown;
            }

            // -Pause
            {
                bool isPauseDown = world.Pause.WasPressedThisFrame();
                //TODO: Pause
            }

            // 技能
            // -Attack
            inputCastKey = InputKeyFlag.None;
            {
                bool isAttackDown = world.Attack.WasPressedThisFrame();

                if (isAttackDown) {
                    inputCastKey |= InputKeyFlag.Attack;
                    Debug.Log("inputkey:" + inputCastKey);
                }
            }

        }


    }
}