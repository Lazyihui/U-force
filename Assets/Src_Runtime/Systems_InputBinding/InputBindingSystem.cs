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
            // 鼠标不能绑定

        }
    }
}