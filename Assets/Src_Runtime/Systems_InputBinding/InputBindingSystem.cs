using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace UF {

    public class InputBindingSystem : MonoBehaviour {
        InputBindingSystemContext ctx;
        InputBindingSystemEvents events;
        public InputBindingSystemEvents Events => events;

        public void Ctor() {
            ctx = new InputBindingSystemContext();
            events = new InputBindingSystemEvents();
        }

        public void Inject(AppUI appUI, InputModule input) {
            ctx.Inject(appUI, input);
        }

        public void Enter() {
            ctx.isRunning = true;
            Reload();
        }

        public void Exit() {
            ctx.isRunning = false;
        }
        public void Tick(float dt) {

        }

        public void BindingTip_Start() {
            var appUI = ctx.appUI;
            var inputModule = ctx.inputModule;
            var processEntity = ctx.processEntity;

            Debug.Log("processEntity.actionIndex" + processEntity.actionIndex);
            InputBindingOneInputKeyModel keyModel = processEntity.actions[processEntity.actionIndex];

            InputKeyFlag inputKey = keyModel.inputkey;

            bool has = inputModule.TryGetToAction(inputKey, out var inputAction);
            if (!has) {
                Debug.LogWarning("InputEntity.StartBind: Action not found: " + inputKey);
                return;
            } else {
                Debug.Log("inputflagKey" + inputKey);
            }

            inputAction.Disable();// 禁用原有的绑定
            // 绑定操作
            processEntity.rebindOperation = inputAction.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete((operation) => {
                    // 绑定完成
                    Debug.Log("InputEntity.StartBind: Rebinding complete: " + inputKey);
                    processEntity.rebindOperation.Dispose();// 释放绑定操作
                })
                .OnCancel((operation) => {
                    Debug.Log("InputEntity.StartBind: Rebinding canceled: " + inputKey);
                    // 取消绑定
                    Reload();
                })
            .Start();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.H)) {
                Reload();
                BindingTip_Start();
            }

        }
        void BakeKeys() {
            // 赋值
            var processEntity = ctx.processEntity;
            var keys = processEntity.actions;
            keys.Clear();

            var inputModule = ctx.inputModule;

            Span<InputKeyFlag> allowBindingKeys = stackalloc InputKeyFlag[0];

            if (processEntity.isKB) {
                allowBindingKeys = stackalloc InputKeyFlag[] {
                    InputKeyFlag.Attack,
                    InputKeyFlag.Jump,
                    InputKeyFlag.MoveLeft,
                    InputKeyFlag.MoveRight,
                    InputKeyFlag.MoveUp,
                    InputKeyFlag.MoveDown,
                    InputKeyFlag.Grap,
                    InputKeyFlag.Esc,
                    InputKeyFlag.Tab,
                    InputKeyFlag.Weap1,
                    InputKeyFlag.Weap2,
                    InputKeyFlag.Weap3,
                    InputKeyFlag.Weap4,
                    InputKeyFlag.Tech,
                    InputKeyFlag.Player1Jump,
                    InputKeyFlag.Player1Attack,
                    InputKeyFlag.Player1Defense,
                    InputKeyFlag.Player2Jump,
                    InputKeyFlag.Player2Attack,
                    InputKeyFlag.Player2Defense,
                    InputKeyFlag.Navigate,
                    InputKeyFlag.Submit,
                };
            } else {
                allowBindingKeys = stackalloc InputKeyFlag[] {
                    InputKeyFlag.Attack,
                    InputKeyFlag.Jump,
                    InputKeyFlag.MoveLeft,
                    InputKeyFlag.MoveRight,
                    InputKeyFlag.MoveUp,
                    InputKeyFlag.MoveDown,
                    InputKeyFlag.Grap,
                    InputKeyFlag.Esc,
                    InputKeyFlag.Tab,
                    InputKeyFlag.Weap1,
                    InputKeyFlag.Weap2,
                    InputKeyFlag.Weap3,
                    InputKeyFlag.Weap4,
                    InputKeyFlag.Tech,
                    InputKeyFlag.Player1Jump,
                    InputKeyFlag.Player1Attack,
                    InputKeyFlag.Player1Defense,
                    InputKeyFlag.Player2Jump,
                    InputKeyFlag.Player2Attack,
                    InputKeyFlag.Player2Defense,
                    InputKeyFlag.Navigate,
                    InputKeyFlag.Submit,
                };
            }

            for (int i = 0; i < allowBindingKeys.Length; i += 1) {
                var allowKey = allowBindingKeys[i];

                if (inputModule.TryGetToAction(allowKey, out var inputAction)) {
                    // 绑定了
                    InputBindingOneInputKeyModel keyModel = new InputBindingOneInputKeyModel() {
                        inputkey = allowKey,
                        isForbidBind_0 = false,
                        inputBindingIndex_0 = i,
                        inputAction_0 = inputAction,
                    };
                    keys.Add(keyModel);
                } else {
                    // 没有绑定
                    InputBindingOneInputKeyModel keyModel = new InputBindingOneInputKeyModel() {
                        inputkey = allowKey,
                        isForbidBind_0 = true,
                        inputBindingIndex_0 = i,
                        inputAction_0 = null,
                    };
                    keys.Add(keyModel);
                }
            }

            // // 获取所有的输入键
            // foreach (InputKeyFlag key in Enum.GetValues(typeof(InputKeyFlag))) {
            //     if (key == InputKeyFlag.None) {
            //         continue;
            //     }
            //     // 检查是否有绑定
            //     if (inputModule.TryGetToAction(key, out var inputAction)) {
            //         // 绑定了
            //         InputBindingOneInputKeyModel keyModel = new InputBindingOneInputKeyModel() {
            //             inputkey = key,
            //             isForbidBind_0 = false,
            //         };
            //         keys.Add(keyModel);
            //     } else {
            //         // 没有绑定
            //         InputBindingOneInputKeyModel keyModel = new InputBindingOneInputKeyModel() {
            //             inputkey = key,
            //             isForbidBind_0 = true,
            //         };
            //         keys.Add(keyModel);
            //     }
            // }
        }

        void Reload() {
            // 重新加载绑定
            var inputModule = ctx.inputModule;
            var processEntity = ctx.processEntity;

            BakeKeys();
        }
    }
}