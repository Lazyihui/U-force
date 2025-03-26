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

        public void Inject(AppUI appUI) {
            ctx.Inject(appUI);
        }

        public void Enter() {
            ctx.isRunning = true;
        }

        public void Exit() {
            ctx.isRunning = false;
        }
    }
}