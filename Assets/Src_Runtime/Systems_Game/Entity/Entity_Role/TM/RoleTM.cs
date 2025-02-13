using System;
using UnityEngine;

namespace UF {

    [Serializable]
    public class RoleTM {
        public string typeName;
        public RoleType roleType;
        public RoleMoveType moveType;
        public RoleState roleState;
        public int typeID;

        // -Life
        public int hp;


        // -move
        public float moveSpeed;
        public float jumpForce;

        // -skillor//一般这种要拖拽绑定的放下面
        public SkillorSo[] skillorSoArray;
        public RoleMod modPrefab;

        // -sprite
        public Sprite sprite;
        public String content;

    }
}
