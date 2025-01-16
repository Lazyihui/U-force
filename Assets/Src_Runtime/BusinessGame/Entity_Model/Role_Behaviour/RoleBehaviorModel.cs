using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public struct RoleBehaviorModel {
        public IDSignature belongWho;// 属于谁的技能
        public IDSignature belongMap;// 属于哪个地图

        // -Move
        public bool isMoveTo;

        // -Jump
        public bool isJump;

        // -Input 还没有写 不知道要不要用

        // -Skill
        public bool isRestoreSkillTimes; // 是否恢复技能次数
        // public SkillSo[] restoreSkills;

        // -Spawn Bullet
        public bool isSpawnBullet; // 是否生成子弹
        // public BulletSpawnerTM bulletSpawnerTM;

        // -Bullet: TearDown
        public bool hasBulletTearDown; // 是否有子弹拆解
        public BulletSo[] tearDownBullets;// 拆解的子弹

        // Attr
        public bool isRestoreHp; // 是否恢复生命值
        public int restoreHp; // 恢复的生命值

        public bool isRestoreMp; // 是否恢复魔法值
        public int restoreMp; // 恢复的魔法值

        // public bool isRestoreGp; // 是否恢复怒气值
        // public int restoreGp; // 恢复的怒气值

        // buff 还没有写 buff的实现
        public bool isAttachBuff; // 是否附加buff
        // public BuffSo[] attachBuffs; // 附加的buff

        public void FromTM(RoleBehaviourTM tm) {

            belongWho = tm.belongWho;
            belongMap = tm.belongMap;

            isMoveTo = tm.isMoveTo;
            isJump = tm.isJump;
            isRestoreSkillTimes = tm.isRestoreSkillTimes;
            isSpawnBullet = tm.isSpawnBullet;
            hasBulletTearDown = tm.hasBulletTearDown;
            isRestoreHp = tm.isRestoreHp;
            restoreHp = tm.restoreHp;
            isRestoreMp = tm.isRestoreMp;
            restoreMp = tm.restoreMp;
            isAttachBuff = tm.isAttachBuff;

            tearDownBullets = tm.tearDownBullets;
        }

    }
}
