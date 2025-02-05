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

        public bool isUpdateProcess;

        public bool isPressStuff1;

        public InputModule() {
            player1 = new InputControls_Player();
            player1.Enable();

            isUpdateProcess = true;
        }

        public void ReSetJump() {
            isJump = false;
        }

        void UnButtonPress() {
            isPressStuff1 = false;
        }

        public void Process(float dt) {
            UnButtonPress();

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
                }
            }

        }

        public void Set_InputCastKey(InputKeyFlag flag) {
            inputCastKey |= flag;
        }


    }
}