using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class InputModule {

        InputControls_Player player1;
        public InputControls_Player Player1 => player1;

        InputKeyFlag inputKey;
        public InputKeyFlag InputKey => inputKey;

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
            inputKey = InputKeyFlag.None;
            {
                bool isJumpDown = world.Jump.WasPressedThisFrame();
                isJump = isJumpDown;
            }

            // -Attack
            inputKey = InputKeyFlag.None;
            {
                bool isAttackDown = world.Attack.WasPressedThisFrame();

                if (isAttackDown) {
                    inputKey |= InputKeyFlag.Attack;
                }
            }

            // -Pause
            inputKey = InputKeyFlag.None;
            {
                bool isPauseDown = world.Pause.WasPressedThisFrame();
                //TODO: Pause
            }
        }


    }
}