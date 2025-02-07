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

            {
                bool isWeap1Down = world.Weap1.WasPressedThisFrame();
                if (isWeap1Down) {
                    inputCastKey |= InputKeyFlag.Weap1;
                }
            }

            {
                bool isWeap2Down = world.Weap2.WasPressedThisFrame();
                if (isWeap2Down) {
                    inputCastKey |= InputKeyFlag.Weap2;
                }
            }

            {
                bool isWeap3Down = world.Weap3.WasPressedThisFrame();
                if (isWeap3Down) {
                    inputCastKey |= InputKeyFlag.Weap3;
                }
            }

            {
                bool isWeap4Down = world.Weap4.WasPressedThisFrame();
                if (isWeap4Down) {
                    inputCastKey |= InputKeyFlag.Weap4;
                }
            }

        }

        public void Set_InputCastKey(InputKeyFlag flag) {
            inputCastKey |= flag;
        }


    }
}