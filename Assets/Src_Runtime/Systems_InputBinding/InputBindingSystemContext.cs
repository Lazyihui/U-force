using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace UF {

    public class InputBindingSystemContext {
        public bool isRunning;

        public AppUI appUI;
        public InputModule inputModule;
        public InputBindingSystemProcessEntity processEntity;

        public InputBindingSystemContext() {
            isRunning = false;
            processEntity = new InputBindingSystemProcessEntity();
        }

        public void Inject(AppUI appUI, InputModule inputModule) {
            this.appUI = appUI;
            this.inputModule = inputModule;
        }
    }
}