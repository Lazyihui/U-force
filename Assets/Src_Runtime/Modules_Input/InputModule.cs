using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

namespace UF {
    public class InputModule : MonoBehaviour {

        InputControls_Player player;
        public InputControls_Player Player => player;

        InputKeyFlag inputCastKey;
        public InputKeyFlag InputCastKey => inputCastKey;

        bool isGrap;
        public bool IsGrap => isGrap;

        public bool isPressStuff1;

        bool isTabDown;
        public bool IsTabDown => isTabDown;

        bool isEscDown;
        public bool IsEscDown => isEscDown;

        // Train
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

        // 进攻战
        Vector2 moveDir;
        public Vector2 MoveDir => moveDir;

        // UI
        public Vector2 uiAxis;
        public bool isConfirm;

        // 存ActionMap
        // Dictionary<InputKey, InputAction> keyToActionMap;



        public void Ctor() {
            player = new InputControls_Player();
            player.Enable();

        }


        void UnButtonPress() {
            isPressStuff1 = false;
        }

        public void Process(float dt) {
            UnButtonPress();

            #region World
            var world = player.World;

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
            // K
            {
                bool isGrapDown = world.Grap.WasPressedThisFrame();
                isGrap = isGrapDown;
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
            // k
            {
                bool isTechDown = world.Tech.WasPressedThisFrame();
                if (isTechDown) {
                    inputCastKey |= InputKeyFlag.Tech;
                }
            }

            // 进攻战Move
            {
                moveDir.x = world.MoveRight.ReadValue<float>() - world.MoveLeft.ReadValue<float>();
                moveDir.y = world.MoveUp.ReadValue<float>() - world.MoveDown.ReadValue<float>();
            }
            #endregion

            #region Player1
            var player1 = player.Player1;
            // 训练营Player1
            {
                bool isPlayer1JumpDown = player1.Player1Jump.WasPressedThisFrame();
                isPlayer1Jump = isPlayer1JumpDown;

                bool isPlayer1AttackDown = player1.Player1Attack.WasPressedThisFrame();
                isPlayer1Attack = isPlayer1AttackDown;

                bool isPlayer1DefenseDown = player1.Player1Defense.WasPressedThisFrame();
                isPlayer1Defense = isPlayer1DefenseDown;
            }
            #endregion

            #region Player2
            var player2 = player.Player2;
            // 训练营Player2
            {
                bool isPlayer2JumpDown = player2.Player2Jump.WasPressedThisFrame();
                isPlayer2Jump = isPlayer2JumpDown;

                bool isPlayer2AttackDown = player2.Player2Attack.WasPressedThisFrame();
                isPlayer2Attack = isPlayer2AttackDown;

                bool isPlayer2DefenseDown = player2.Player2Defense.WasPressedThisFrame();
                isPlayer2Defense = isPlayer2DefenseDown;
            }
            #endregion

            #region UI
            var ui = player.UI;
            // UI: Move
            {
                if (ui.Navigate.WasPressedThisFrame()) {
                    uiAxis = ui.Navigate.ReadValue<Vector2>();
                } else {
                    uiAxis = Vector2.zero;
                }
            }

            // UI: Confirm
            {
                isConfirm = ui.Submit.WasPressedThisFrame();
            }
            #endregion
        }

        public void Set_InputCastKey(InputKeyFlag flag) {
            inputCastKey |= flag;
        }
        // ==== Save ====
        public string ToJson() {
            string json = player.SaveBindingOverridesAsJson();
            return json;
        }

        public void FromJson(string json) {
            if (!string.IsNullOrEmpty(json)) {
                player.LoadBindingOverridesFromJson(json, true);
            }
        }

    }
}