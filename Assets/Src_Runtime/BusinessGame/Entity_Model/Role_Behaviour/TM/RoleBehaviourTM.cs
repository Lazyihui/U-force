using System;
using System.Collections;
using UnityEngine;
using TriInspector;

namespace UF {

    [Serializable]
    public struct RoleBehaviourTM {
        public IDSignature belongWho;// 属于谁的技能
        public IDSignature belongMap;// 属于哪个地图

        [Title("Move State")]
        // -Move
        public bool isMoveTo;

        // -Jump
        public bool isJump;

        // -Input 还没有写 不知道要不要用

        [Title("Skill State")]
        // -Skill
        public bool isRestoreSkillTimes; // 是否恢复技能次数
        // public SkillSo[] restoreSkills;

        [Title("Bullet State")]
        // -Spawn Bullet
        public bool isSpawnBullet; // 是否生成子弹
        // public BulletSpawnerTM bulletSpawnerTM;

        // -Bullet: TearDown
        public bool hasBulletTearDown; // 是否有子弹拆解
        [ShowIf(nameof(hasBulletTearDown))] public BulletSo[] tearDownBullets;// 拆解的子弹

        [Title("Attr")]
        // Attr
        public bool isRestoreHp; // 是否恢复生命值
        [ShowIf(nameof(isRestoreHp))] public int restoreHp; // 恢复的生命值

        public bool isRestoreMp; // 是否恢复魔法值
        [ShowIf(nameof(isRestoreMp))] public int restoreMp; // 恢复的魔法值

        // public bool isRestoreGp; // 是否恢复怒气值
        // public int restoreGp; // 恢复的怒气值

        [Title("Buff")]
        // buff 还没有写 buff的实现
        public bool isAttachBuff; // 是否附加buff
        // public BuffSo[] attachBuffs; // 附加的buff
    }
}