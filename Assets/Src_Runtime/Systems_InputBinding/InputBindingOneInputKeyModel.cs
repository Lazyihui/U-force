using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class InputBindingOneInputKeyModel {
        // 基础每一个按键的信息
        public InputKeyFlag keyFlag;//按键标志
        public string keyName;//按键名称 Action 名称
        public string keyMapName; // ActionMap名称

        public string keyName_KB;//按键名称_键盘
        public string keyName_Gamepad;//按键名称_手柄

        // 后来选的按键名称
        public string keyName_Choose;//按键名称_选择

        public int keyIndex;//按键索引

        public void Init(string keyName, string keyMapName, string keyName_KB, string keyName_Gamepad, int keyIndex) {
            this.keyName = keyName;
            this.keyMapName = keyMapName;
            this.keyName_KB = keyName_KB;
            this.keyName_Gamepad = keyName_Gamepad;
            this.keyName_Choose = keyName_KB;
        }
    }
}