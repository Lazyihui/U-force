using System;
using UnityEngine;
using TriInspector;

namespace UF {

    [Serializable]
    public class BulletTM {
        public int typeID;
        public string typeName;

        [Title("life")]
        public int hp;
        public int hitTimes;
        public int attackDamage;

        public bool isBomb;

        public bool isHaveContinueDamege;
        [ShowIf(nameof(isHaveContinueDamege))] public int continueDamege;
        [ShowIf(nameof(isHaveContinueDamege))] public float damageInterval;
        [ShowIf(nameof(isHaveContinueDamege))] public float damageTimer;

        [ShowIf(nameof(isHaveContinueDamege))] public float damageRange;


        [Title("Locomotion")]
        public BulletMoveType type;
        public float moveSpeed;

        public BulletMod modPrefab;
        public Sprite sprite;
        public string content;

        [Title("子弹特性")]
        public bool isMakeBulletAppear;
        public bool isDestoryALine;


        [Title("Frame")]
        public BulletFrameTM[] frames;

        [Title("作用于role")]
        public bool isChangeLine;
        [ShowIf(nameof(isChangeLine))] public Vector2 changeLineDir;
        [ShowIf(nameof(isChangeLine))] public int LineNum; // 变换的行数
        [ShowIf(nameof(isChangeLine))] public float LineLength; // 每行长度

        public bool isRepelled; // 击退
        [ShowIf(nameof(isRepelled))] public float repeLength; // 击退距离
    }
}