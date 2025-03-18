using System;
using UnityEngine;
using TriInspector;

namespace UF {

    [Serializable]
    public class BulletTM {
        public int typeID;
        public string typeName;

        [Title("life")]
        public int hitTimes;
        public int attackDamage;

        public bool isHaveContinueDamege;
        [ShowIf(nameof(isHaveContinueDamege))] public int continueDamege;
        [ShowIf(nameof(isHaveContinueDamege))] public float damageInterval;
        [ShowIf(nameof(isHaveContinueDamege))] public float damageTimer;
        [ShowIf(nameof(isHaveContinueDamege))] public float damageRange;
        [ShowIf(nameof(isHaveContinueDamege))] public float destoryOneselfTime;

        public bool isHaveConverge;
        [ShowIf(nameof(isHaveConverge))] public float convergeLine; // 汇聚行数(比如1代表上下各一行共三行)

        [Title("作用于role")]
        public bool isChangeLine;
        [ShowIf(nameof(isChangeLine))] public Vector2 changeLineDir;
        [ShowIf(nameof(isChangeLine))] public int LineNum; // 变换的行数
        [ShowIf(nameof(isChangeLine))] public float LineLength; // 每行长度

        [Title("带电")]
        public bool isElectricity;
        [ShowIf(nameof(isElectricity))] public int elecDamage; // 伤害
        [ShowIf(nameof(isElectricity))] public int elecTimes; // 伤害次数
        [ShowIf(nameof(isElectricity))] public float elecMax; // 伤害间隔


        [Title("子弹特性")]
        public bool isMakeBulletAppear;
        public bool isDestotyBottomALine;
        public bool isFixedBoomPoint;
        public bool isRepelled; // 击退
        [ShowIf(nameof(isRepelled))] public float repeLength; // 击退距离

        [Title("Locomotion")]
        public BulletMoveType type;
        public float moveSpeed;

        public BulletMod modPrefab;
        public Sprite sprite;
        public string content;

        [Title("Frame")]
        public BulletFrameTM[] frames;


    }
}