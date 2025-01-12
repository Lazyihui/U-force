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

        public void Clear(){
            isJump = false;
        }

        public void Release(){
            Clear();
        }

    }
}