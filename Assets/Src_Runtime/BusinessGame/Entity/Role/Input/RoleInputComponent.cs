using System;
using System.Collections;
using UnityEngine;

namespace UF {

    [Serializable]
    public class RoleInpuComponent{

        // -Jump
        bool isJump;
        public bool IsJump => isJump;
        public void Jump_Set(bool value)=>isJump = value;

        // -input
        InputKeyFlag inputKeys;
        public InputKeyFlag InputKeys => inputKeys;

        public void Clear(){
            isJump = false;
            inputKeys = InputKeyFlag.None;
        }

        public void Release(){
            Clear();
            
        }

    }
}