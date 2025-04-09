using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF.Systems_Setting {

    public class SettingContext {

        public bool isRunning;

        public AppUI appUI;
        public InputModule inputModule;
        public InputBindingSystemProcessEntity processEntity;

        public bool isInputBindMain; // 按键绑定是否为主游戏按键

        public void Inject(AppUI appUI,InputModule inputModule) {
            this.appUI = appUI;
            this.inputModule = inputModule;
        }

        public SettingContext() {
            isRunning = false;
            processEntity = new InputBindingSystemProcessEntity();

            isInputBindMain = true; // flase代表是训练营按键
        }

    }


}