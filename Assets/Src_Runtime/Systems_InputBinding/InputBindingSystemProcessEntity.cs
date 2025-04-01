using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



namespace UF {

    public class InputBindingSystemProcessEntity {

        public List<InputBindingOneInputKeyModel> inputKeyList;// 输入按键列表


        public bool isKB;//是否是键盘   
        public InputActionRebindingExtensions.RebindingOperation rebindOperation;// 绑定操作
        public bool isRebindingSucc;// 绑定成功


        public InputBindingSystemProcessEntity() {
            inputKeyList = new List<InputBindingOneInputKeyModel>();
        }

        public void AddInputKey(InputBindingOneInputKeyModel inputKey) {
            inputKeyList.Add(inputKey);
        }
        public void ClearInputKey() {
            inputKeyList.Clear();
        }

    }
}