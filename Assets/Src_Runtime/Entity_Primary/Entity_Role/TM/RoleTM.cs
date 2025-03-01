using System;
using TriInspector;
using UnityEngine;

namespace UF {

    [Serializable]
    public class RoleTM {
        public string typeName;
        public RoleType roleType;
        public RoleMoveType moveType;
        public RoleState roleState;
        public int typeID;

        [Title("Life")]
        public int hp;

        [Title("Movement")]
        public float moveSpeed;
        public float jumpForce;
        public int baseDamage; // 基础伤害
        public int fallDamage; // 掉落伤害

        // -skillor//一般这种要拖拽绑定的放下面
        public SkillorSo[] skillorSoArray;
        public RoleMod modPrefab;
        [Title("Feature特性")]
        public FeatureType featureType;

        [Title("sprite")]
        public Sprite sprite;
        public String content;

        [Title("detail")]
        public string feature; // 特征 / 介绍
        public string skill; // 技能
        public string restraint; // 相克武器

    }
}
