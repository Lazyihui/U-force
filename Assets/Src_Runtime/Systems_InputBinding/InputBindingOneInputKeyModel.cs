using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    public class InputBindingOneInputKeyModel {
        // 基础每一个按键的信息
        public InputKeyFlag keyFlag;//按键标志
        public string keyName;//按键名称 Action 名称
        public string keyMapName; // ActionMap名称

        // 后来选的按键名称
        public int keyIndex;//按键索引

        public void Init(string keyName, string keyMapName,  int keyIndex) {
            this.keyName = keyName;
            this.keyMapName = keyMapName;
        }
    }
}