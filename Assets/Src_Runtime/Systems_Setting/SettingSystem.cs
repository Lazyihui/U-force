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
        }

        public void Enter() {
            ctx.isRunning = true;
            var appUI = ctx.appUI;
            appUI.Panel_Setting_Open();

            var processEntity = ctx.processEntity;

            foreach (var inputKey in processEntity.inputKeyList) {
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
                Debug.Log("SettingSystem: OnSettingMusicSliderChangeHandle" + value);
            };

            eventCenter.OnSettingSFXSliderChangeHandle += (value) => { // 音效音量
                events.OnSettingSFXSliderChange(value);
                Debug.Log("SettingSystem: OnSettingSFXSliderChangeHandle" + value);
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

            ctx.processEntity.onRebindingSuccHandle += (isSucc, bindName) => {
                var processEntity = ctx.processEntity;
                var oneInputModel = processEntity.GetInputKeyByIndex(bindName);
                if (isSucc) {
                    Debug.Log("SettingSystem: OnRebindingSuccHandle" + isSucc);
                    // 绑定成功
                    // 更新UI
                    appUI.Panel_SettingSetBindElement(oneInputModel.keyName, oneInputModel.kbBindName, oneInputModel.gamepadBindName);
                } else {
                    Debug.Log("SettingSystem: OnRebindingSuccHandle" + isSucc);
                    // 绑定失败
                }
            };

            #endregion
        }

    }
}