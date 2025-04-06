using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace UF {
    public static class InputBindingDomain {

        public static void AddAllInputKey(InputControls_Player playerControls, InputBindingSystemProcessEntity processEntity) {
            // TODO：功能实现了再来优化
            // 添加所有按键
            string keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Attack);
            string kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            string gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap1);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap2);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap3);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Weap4);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Tech);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Esc);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Tab);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveLeft);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveRight);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveUp);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.MoveDown);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Grap);
            kbBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("World", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "World", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player1Jump);
            kbBindName = GetBindingDisplayString("Player1", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("Player1", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player1", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player1Attack);
            kbBindName = GetBindingDisplayString("Player1", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("Player1", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player1", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player1Defense);
            kbBindName = GetBindingDisplayString("Player1", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("Player1", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player1", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player2Jump);
            kbBindName = GetBindingDisplayString("Player2", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("Player2", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player2", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player2Attack);
            kbBindName = GetBindingDisplayString("Player2", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("Player2", keyName, 0, InputDeviceType.Gamepad, playerControls);
            InputBindOneInputKeyAdd(processEntity, keyName, "Player2", 0, kbBindName, gamepadBindName);

            keyName = InputKeyFlagExtension.ToInputKeyString(InputKeyFlag.Player2Defense);
            kbBindName = GetBindingDisplayString("Player2", keyName, 0, InputDeviceType.Keyboard, playerControls);
            gamepadBindName = GetBindingDisplayString("Player2", keyName, 0, InputDeviceType.Gamepad, playerControls);
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
            if(action == null) {
                Debug.LogError($"没找到action {actionName} 没找到Map {actionMapName}");
                return "";
            }

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

        // 绑定
        public static void OnReBindAction(AppUI appUI, InputControls_Player playerControls, string actionMapName, string actionName, int index, InputDeviceType deviceType, InputBindingSystemProcessEntity processEntity) {

            var callback = processEntity.onRebindingSuccHandle;

            if (!CheckHaveOneBindInaction(playerControls, actionName)) {
                Debug.LogWarning($"Action {actionName} has no bindings.");
                return;
            }

            // TODO:下面这一行是为了测试用的，后面要删除
            InputAction temAction = GetAction(playerControls, actionMapName, actionName);
            int temIndex = GetAdjustedBindingIndex(temAction, index);

            // 保存原始绑定路径以便恢复
            string originalBindingPath = temAction.bindings[temIndex].effectivePath;

            temAction.Disable();
            playerControls.asset.Disable();

            var rebindOperation = temAction.PerformInteractiveRebinding(temIndex)
                    .WithCancelingThrough("<Keyboard>/escape") // 按ESC取消
                    .WithControlsExcluding("<Mouse>/")
                    .OnMatchWaitForAnother(0.1f)
                    .WithTimeout(3.5f);
            appUI.Panel_InputNotice_Open();

            // 根据设备类型排除不相关的控制
            if (deviceType == InputDeviceType.Keyboard) {
                rebindOperation.WithControlsExcluding("<Gamepad>/*")
                    .WithControlsExcluding("<Mouse>/position")
                    .WithControlsExcluding("<Mouse>/delta");
            } else if (deviceType == InputDeviceType.Gamepad) {
                rebindOperation.WithControlsExcluding("<Keyboard>/*")
                    .WithControlsExcluding("<Mouse>/*");
            }

            rebindOperation.OnComplete((operation) => {

                string newBindingPath = temAction.bindings[temIndex].effectivePath;

                if (newBindingPath == originalBindingPath) {
                    Debug.Log("Same key binding, no change needed.");
                    operation.Cancel();
                    callback?.Invoke(false, actionName);
                    return;
                }

                // 是否存在相同的按键绑定
                bool hasExit = IsBindingAlreadyUsed(playerControls, actionMapName, actionName, newBindingPath);
                // 检查按键是否已被使用
                if (hasExit) {
                    Debug.LogWarning($"Key {newBindingPath} is already bound to another action." + "这个按钮已经被绑定了！");
                    // 恢复原始绑定
                    temAction.ApplyBindingOverride(temIndex, originalBindingPath);
                    appUI.Panel_InputNoticeImg_Open();
                    operation.Cancel();
                    callback?.Invoke(hasExit, actionName);
                } else {
                    appUI.Panel_InputNotice_Close();
                    SaveBindings(playerControls);
                    callback?.Invoke(!hasExit, actionName);
                }

                // 保存绑定
                operation.Dispose();
            })

            .OnCancel((operation) => {
                temAction.ApplyBindingOverride(temIndex, originalBindingPath);
                callback?.Invoke(false, actionName);
                operation.Dispose();
            })
                .Start();
            temAction.Enable();
            playerControls.asset.Enable();
        }

        // 重置所有绑定到默认设置
        public static void ResetAllToDefaultBindings(InputControls_Player playerControls) {
            // 首先禁用所有控制以防止冲突
            playerControls.asset.Disable();

            try {
                // 遍历所有动作映射
                foreach (InputActionMap map in playerControls.asset.actionMaps) {
                    // 移除该动作映射中的所有绑定覆盖
                    map.RemoveAllBindingOverrides();
                }
                // 从PlayerPrefs中删除保存的绑定设置
                PlayerPrefs.DeleteKey("PlayerControls");
                PlayerPrefs.Save();
                Debug.Log("所有输入绑定已重置为默认设置");
            } finally {
                // 确保重新启用控制
                playerControls.asset.Enable();
            }
        }


        #region 基础函数
        // 是否有相同的按键绑定
        static bool IsBindingAlreadyUsed(InputControls_Player playerControls, string currentActionMap, string currentAction, string newBindingPath) {
            if (string.IsNullOrEmpty(newBindingPath)) {
                // 如果新绑定路径为空，则不进行检查
                return false;
            }

            // 遍历所有actionMap
            foreach (InputActionMap map in playerControls.asset.actionMaps) {
                // 遍历所有action
                foreach (InputAction action in map.actions) {
                    // 跳过当前正在重绑定的action
                    if (map.name == currentActionMap && action.name == currentAction)
                        continue;

                    // 检查当前action的所有绑定
                    for (int i = 0; i < action.bindings.Count; i++) {
                        if (action.bindings[i].effectivePath == newBindingPath) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
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
            Debug.Assert(action != null, "Action is null");
            Debug.Assert(index >= 0, "Index is negative");
            Debug.Assert(index < action.controls.Count, "Index is out of range");
            if (action.controls.Count == 0) {
                return index;
            }

            int baseIndex = action.GetBindingIndexForControl(action.controls[0]);
            return baseIndex + index;
        }
        // 保存绑定
        static void SaveBindings(InputControls_Player playerControls) {
            string json = playerControls.asset.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("PlayerControls", json);
            PlayerPrefs.Save();
        }
        #endregion
    }
}