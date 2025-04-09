using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UF.Systems_Setting;

namespace UF {

    public class SettingSystem : MonoBehaviour {

        SettingContext ctx;

        SettingSystemEvents events;
        public SettingSystemEvents Events => events;

        InputControls_Player playerControls => ctx.inputModule.Player;


        public void Ctor() {
            ctx = new SettingContext();
            events = new SettingSystemEvents();
        }

        public void Inject(AppUI appUI, InputModule inputModule) {
            ctx.Inject(appUI, inputModule);

            InputBindingDomain.AddAllInputKey(playerControls, ctx.processEntity);
            InputBindingDomain.ShowInputKeyByMap(ctx.processEntity, ctx.isInputBindMain);
        }

        public void Enter() {
            ctx.isRunning = true;
            var appUI = ctx.appUI;
            appUI.Panel_Setting_Open();

            var processEntity = ctx.processEntity;

            foreach (var inputKey in processEntity.inputKeyListByMap) {
                appUI.Panel_SettingHideInputBindElement();
                appUI.Panel_SettingAddInputbindElement(inputKey);
            }
        }

        public void Exit() {
            ctx.isRunning = false;
        }

        public void Tick(float dt) {

        }
        public void Setting_Binding() {
            var appUI = ctx.appUI;
            Debug.Assert(appUI != null, "appUI is null");
            Debug.Assert(appUI.eventCenter != null, "appUI.eventCenter is null");
            var eventCenter = appUI.eventCenter;
            // 设置
            eventCenter.OnSettingBackClickHandle += () => { // 关闭设置
                events.OnSettingBackClick();
                Debug.Log("SettingSystem: OnSettingBackClickHandle");
            };

            eventCenter.OnSettingReselustion1920ClickHandle += () => { // 1920*1080
                events.OnSettingReselustion1920Click();
                Debug.Log("SettingSystem: OnSettingReselustion1920ClickHandle");
            };

            eventCenter.OnSettingReselustion1600ClickHandle += () => { // 1600*900
                events.OnSettingReselustion1600Click();
                Debug.Log("SettingSystem: OnSettingReselustion1600ClickHandle");
            };

            eventCenter.OnSettingReselustion640ClickHandle += () => { // 640*360
                events.OnSettingReselustion640Click();
                Debug.Log("SettingSystem: OnSettingReselustion640ClickHandle");
            };
            eventCenter.OnSettingFullScreenToggleHandle += (isOn) => { // 全屏
                events.OnSettingFullScreenToggle(isOn);
                Debug.Log("SettingSystem: OnSettingFullScreenToggleHandle" + isOn);
            };

            eventCenter.OnSettingMusicSliderChangeHandle += (value) => { // 音乐音量
                events.OnSettingMusicSliderChange(value);
            };

            eventCenter.OnSettingSFXSliderChangeHandle += (value) => { // 音效音量
                events.OnSettingSFXSliderChange(value);
            };

            eventCenter.OnResetInputBindClickHandle += () => { // 重置按键绑定
                Debug.Log("重置按钮被点击了");
                events.OnSettingResetInputBindClick();
                InputBindingDomain.ResetAllToDefaultBindings(playerControls);
                // 重置所有的按键绑定
                var processEntity = ctx.processEntity;
                // var oneInputModel = processEntity.GetInputKeyByIndex(actionName);
                // 遍历所有的按键绑定
                foreach (var oneInputModel in processEntity.inputKeyListByMap) {
                    // Debug.Log("原来" + kbBindName);
                    string kbBindName = InputBindingDomain.GetBindingDisplayString(oneInputModel.keyMapName, oneInputModel.keyName, 0, InputDeviceType.Keyboard, playerControls);
                    string gamepadBindName = InputBindingDomain.GetBindingDisplayString(oneInputModel.keyMapName, oneInputModel.keyName, 0, InputDeviceType.Gamepad, playerControls);
                    appUI.Panel_SettingSetBindElement(oneInputModel.keyName, kbBindName, gamepadBindName);
                }

            };

            eventCenter.OnInputBindMainClickHandle += () => { // 按键绑定切换主游戏按键
                ctx.isInputBindMain = true;
            };

            eventCenter.OnInputBindTrainClickHandle += () => { // 按键绑定切换训练营按键
                ctx.isInputBindMain = false;
            };

            #region InputBind
            eventCenter.OnSettingInputBindClickKbHandle += (keyName) => { // 键盘绑定
                var processEntity = ctx.processEntity;
                var oneInputModel = processEntity.GetInputKeyByIndex(keyName);
                if (oneInputModel == null) {
                    Debug.LogError("SettingSystem: OnSettingInputBindClickKbHandle" + keyName + " is null");
                    return;
                }

                InputBindingDomain.OnReBindAction(ctx.appUI, ctx.inputModule.Player, oneInputModel.keyMapName, oneInputModel.keyName, oneInputModel.keyIndex, InputDeviceType.Keyboard, processEntity);
                // Debug.Log("SettingSystem: OnSettingInputBindKbClickHandle" + keyName);
            };
            eventCenter.OnSettingInputBindClickGamepadHandle += (keyName) => { // 手柄绑定
                // InputBindingDomain.OnReBindAction(keyName, InputDeviceType.Gamepad);
                Debug.Log("SettingSystem: OnSettingInputBindGamepadClickHandle" + keyName);
            };

            ctx.processEntity.onRebindingSuccHandle += (iskeyboard, actionName) => {
                var processEntity = ctx.processEntity;
                var oneInputModel = processEntity.GetInputKeyByIndex(actionName);
                // Debug.Log("原来" + kbBindName);
                string kbBindName = InputBindingDomain.GetBindingDisplayString(oneInputModel.keyMapName, oneInputModel.keyName, 0, InputDeviceType.Keyboard, playerControls);
                string gamepadBindName = InputBindingDomain.GetBindingDisplayString(oneInputModel.keyMapName, oneInputModel.keyName, 0, InputDeviceType.Gamepad, playerControls);
                appUI.Panel_SettingSetBindElement(oneInputModel.keyName, kbBindName, gamepadBindName);
            };

            eventCenter.OnInputNoticeClickHandle += () => { // 输入提示
                appUI.Panel_InputNotice_Close();
            };


            #endregion
        }

    }
}