using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace UF {

    public class InputBindingSystemContext {
        public bool isRunning;

        public AppUI appUI;

        public InputBindingSystemContext() {
            isRunning = false;
        }

        public void Inject(AppUI appUI) {
            this.appUI = appUI;
        }
    }
}