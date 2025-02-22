using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public class InputModule : MonoBehaviour {

        InputControls_Player player1;
        public InputControls_Player Player1 => player1;

        InputKeyFlag inputCastKey;
        public InputKeyFlag InputCastKey => inputCastKey;

        bool isJump;
        public bool IsJump => isJump;

        public bool isUpdateProcess;

        public bool isPressStuff1;

        bool isTabDown;
        public bool IsTabDown => isTabDown;

        bool isEscDown;
        public bool IsEscDown => isEscDown;

        bool isPlayer1Jump;
        public bool IsPlayer1Jump => isPlayer1Jump;

        bool isPlayer1Attack;
        public bool IsPlayer1Attack => isPlayer1Attack;

        bool isPlayer1Defense;
        public bool IsPlayer1Defense => isPlayer1Defense;

        bool isPlayer2Jump;
        public bool IsPlayer2Jump => isPlayer2Jump;

        bool isPlayer2Attack;
        public bool IsPlayer2Attack => isPlayer2Attack;

        bool isPlayer2Defense;
        public bool IsPlayer2Defense => isPlayer2Defense;

        public void Ctor() {
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

            #region World
            var world = player1.World;

            // -Input
            {
                bool isJumpDown = world.Jump.WasPressedThisFrame();
                isJump = isJumpDown;
            }

            // -Pause Exc
            {
                bool isEscDown = world.Esc.WasPressedThisFrame();
                this.isEscDown = isEscDown;
            }

            // -Tab
            {
                bool isTabDown = world.Tab.WasPressedThisFrame();
                this.isTabDown = isTabDown;
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

            // 4
            {
                bool isWeap4Down = world.Weap4.WasPressedThisFrame();
                if (isWeap4Down) {
                    inputCastKey |= InputKeyFlag.Weap4;
                }
            }
            #endregion

            #region Train
            var train = player1.Train;

            // Player1
            {
                bool isPlayer1JumpDown = train.Player1Jump.WasPressedThisFrame();
                isPlayer1Jump = isPlayer1JumpDown;

                bool isPlayer1AttackDown = train.Player1Attack.WasPressedThisFrame();
                isPlayer1Attack = isPlayer1AttackDown;

                bool isPlayer1DefenseDown = train.Player1Defense.WasPressedThisFrame();
                isPlayer1Defense = isPlayer1DefenseDown;
            }

            // Player2
            {
                bool isPlayer2JumpDown = train.Player2Jump.WasPressedThisFrame();
                isPlayer2Jump = isPlayer2JumpDown;

                bool isPlayer2AttackDown = train.Player2Attack.WasPressedThisFrame();
                isPlayer2Attack = isPlayer2AttackDown;

                bool isPlayer2DefenseDown = train.Player2Defense.WasPressedThisFrame();
                isPlayer2Defense = isPlayer2DefenseDown;
            }
            #endregion
        }

        public void Set_InputCastKey(InputKeyFlag flag) {
            inputCastKey |= flag;
        }


    }
}