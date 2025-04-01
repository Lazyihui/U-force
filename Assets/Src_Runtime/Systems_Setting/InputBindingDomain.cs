using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace UF {
    public static class InputBindingDomain {
        public static void AddAllInputKey(InputControls_Player playerControls, InputBindingSystemProcessEntity processEntity) {
            // 添加所有按键
            string keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Attack);
            string kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            string gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap1);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap2);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap3);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap4);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Tech);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Esc);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Tab);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveLeft);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveRight);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveUp);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveDown);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Grap);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player1Jump);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player1Attack);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player1", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player1Defense);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player1", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player2Jump);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player2", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player2Attack);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player2", 0, kbBindName, gamepadBindName);
            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player2Defense);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player2", 0, kbBindName, gamepadBindName);

        }

        static void InputBindOneInputKeyAdd(InputBindingSystemProcessEntity processEntity, string keyName, string keyMapName, int keyIndex, string kbBindName, string gamepadBindName) {
            // 添加一个按键
            InputBindingOneInputKeyModel inputKey = new InputBindingOneInputKeyModel();
            inputKey.Init(keyName, keyMapName, keyIndex, kbBindName, gamepadBindName);
            processEntity.AddInputKey(inputKey);
        }

        // 得到当前action的绑定路径（HQWR）就是键盘上的数字
        public static string GetBindingDisplayString(string actionMapName, string actionName, int index, InputDeviceType deviceType, InputControls_Player playerControls) {
            if (!CheckHaveOneBindInaction(playerControls, actionName)) {
                return "";
            }
            InputAction action = GetAction(playerControls, actionMapName, actionName);

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
        public static bool CheckHaveOneBindInaction(InputControls_Player playerControls, string actionName) {
            //TOOD:
            if (playerControls.FindAction(actionName).bindings.Count > 0) {
                return true;
            } else {
                return false;
            }
        }
        // 根据actionMapname名称和actionname名称获取当前action的绑定路径InputAction

        public static InputAction GetAction(InputControls_Player playerControls, string actionMapName, string actionName) {
            if (!CheckHaveOneBindInaction(playerControls, actionName)) {
                return null;
            }
            InputActionMap actionMap = playerControls.asset.FindActionMap(actionMapName);
            InputAction action = actionMap.FindAction(actionName);
            return action;
        }

        // 获取当前action的绑定索引
        static int GetAdjustedBindingIndex(InputAction action, int index) {
            if (action.controls.Count == 0)
                return index;

            int baseIndex = action.GetBindingIndexForControl(action.controls[0]);
            return baseIndex + index;
        }
        #endregion
    }
}