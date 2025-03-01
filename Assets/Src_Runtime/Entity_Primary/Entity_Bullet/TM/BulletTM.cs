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

        public bool isHaveDamageRange;
        [ShowIf(nameof(isHaveDamageRange))] public float damageRange;


        [Title("Locomotion")]
        public BulletMoveType type;
        public float moveSpeed;

        public BulletMod modPrefab;
        public Sprite sprite;
        public string content;

        [Title("子弹特性")]
        public bool isMakeBulletAppear;


        [Title("Frame")]
        public BulletFrameTM[] frames;


    }
}