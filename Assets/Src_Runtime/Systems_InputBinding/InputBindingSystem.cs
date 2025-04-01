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


        public void Ctor() {
            ctx = new InputBindingSystemContext();
            events = new InputBindingSystemEvents();
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

        public void InputBindOneInputKeyAdd(string keyName, string keyMapName, string keyName_KB, string keyName_Gamepad, int keyIndex) {
            var processEntity = ctx.processEntity;
            // 添加一个按键
            InputBindingOneInputKeyModel inputKey = new InputBindingOneInputKeyModel();
            inputKey.Init(keyName, keyMapName, keyName_KB, keyName_Gamepad, keyIndex);
            processEntity.AddInputKey(inputKey);
        }

    }
}