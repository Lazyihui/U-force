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

        // -Life
        public int hp;

        // -move
        public float moveSpeed;
        public float jumpForce;
        public int baseDamage; // 基础伤害
        public int fallDamage; // 掉落伤害


        // -skillor//一般这种要拖拽绑定的放下面
        public SkillorSo[] skillorSoArray;
        public RoleMod modPrefab;

        // -sprite
        public Sprite sprite;
        public String content;

        // -detail 详情
        public string feature; // 特征
        public string skill; // 技能
        public string restraint; // 相克武器

        // -restraint 被克制
        public bool isMinion;
        [ShowIf(nameof(isMinion))] public BulletSo[] restBullet; // 被克制的子弹
        [ShowIf(nameof(isMinion))] public BulletSo[] restKill; // 被必杀的子弹
        [ShowIf(nameof(isMinion))] public RoleSo[] restPlayer; // 被克制的英雄
    }
}
