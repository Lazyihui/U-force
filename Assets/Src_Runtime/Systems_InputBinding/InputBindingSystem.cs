using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace UF {
    // 设备先默认是键盘
    public class InputBindingSystem : MonoBehaviour {
        InputBindingSystemContext ctx;
        InputBindingSystemEvents events;
        public InputBindingSystemEvents Events => events;

        InputControls_Player playerControls => ctx.inputModule.Player;


        public void Ctor() {
            ctx = new InputBindingSystemContext();
            events = new InputBindingSystemEvents();
            AddAllInputKey();
        }

        public void Inject(AppUI appUI, InputModule input) {
            ctx.Inject(appUI, input);
        }

        public void Enter() {
            ctx.isRunning = true;
        }

        public void Exit() {
            ctx.isRunning = false;
        }

        public void Tick(float dt) {

        }

        public void AddAllInputKey() {
            // 添加所有按键
            string keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Attack);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap1);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap2);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap3);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap4);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Tech);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Esc);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Tab);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveLeft);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveRight);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveUp);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveDown);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Grap);
            InputBindOneInputKeyAdd(keyName, "World", 0);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player1Jump);
            InputBindOneInputKeyAdd(keyName, "World", 0);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player1Attack);
            InputBindOneInputKeyAdd(keyName, "Player1", 0);
        }

        void InputBindOneInputKeyAdd(string keyName, string keyMapName, int keyIndex) {
            var processEntity = ctx.processEntity;
            // 添加一个按键
            InputBindingOneInputKeyModel inputKey = new InputBindingOneInputKeyModel();
            inputKey.Init(keyName, keyMapName, keyIndex);
            processEntity.AddInputKey(inputKey);
        }

        // 得到当前action的绑定路径（HQWR）就是键盘上的数字
        public string GetBindingDisplayString(string actionMapName, string actionName, int index, InputDeviceType deviceType) {
            if (!CheckHaveOneBindInaction(actionName)) {
                return "";
            }
            InputAction action = GetAction(actionMapName, actionName);

            int bindingIndex = GetAdjustedBindingIndex(action, index);
            if (bindingIndex < 0 || bindingIndex >= action.bindings.Count) {
                Debug.LogError("Invalid binding index");
                return "";
            }

            // 检查绑定是否符合设备类型要求
            if (deviceType != InputDeviceType.Any) {
                string expectedPathPrefix = deviceType == InputDeviceType.Keyboard ?
                    "<Keyboard>/" : "<Gamepad>/";

                if (!action.bindings[bindingIndex].effectivePath.StartsWith(expectedPathPrefix)) {
                    // effectivePath是Input System中表示绑定路径的字符串(如"<Keyboard>/w"或"<Gamepad>/buttonSouth")
                    return ""; // 如果不是要求的设备类型，返回空字符串
                }
            }

            return action.GetBindingDisplayString(bindingIndex);
        }

        #region 基础函数


        //  检查是否有绑定的action
        public bool CheckHaveOneBindInaction(string actionName) {
            //TOOD:
            if (playerControls.FindAction(actionName).bindings.Count > 0) {
                return true;
            } else {
                return false;
            }
        }
        // 根据actionMapname名称和actionname名称获取当前action的绑定路径InputAction

        public InputAction GetAction(string actionMapName, string actionName) {
            if (!CheckHaveOneBindInaction(actionName)) {
                return null;
            }
            InputActionMap actionMap = playerControls.asset.FindActionMap(actionMapName);
            InputAction action = actionMap.FindAction(actionName);
            return action;
        }

        // 获取当前action的绑定索引
        int GetAdjustedBindingIndex(InputAction action, int index) {
            if (action.controls.Count == 0)
                return index;

            int baseIndex = action.GetBindingIndexForControl(action.controls[0]);
            return baseIndex + index;
        }
        #endregion
    }
}