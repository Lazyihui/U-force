using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



namespace UF {

    public class InputBindingSystemProcessEntity {

        public InputBindingSystemMode mode;
        public bool isKB;//是否是键盘

        public int actionIndex;// 选择哪一个动作
        public List<InputBindingOneInputKeyModel> actions;// 选择的动作列表

        public InputActionRebindingExtensions.RebindingOperation rebindOperation;// 绑定操作
        public bool isRebindingSucc;// 绑定成功

        public int hack_delay_frame;// 绑定操作延迟帧数

        public InputBindingSystemProcessEntity() {
            actions = new List<InputBindingOneInputKeyModel>();
        }




    }
}