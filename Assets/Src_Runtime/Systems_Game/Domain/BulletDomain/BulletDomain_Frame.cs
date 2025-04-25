using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    // frame 逻辑   
    public static partial class BulletDomain {

        #region  Frame:Tick 
        public static void Frames_Tick(GameSystemContext ctx, BulletEntity bullet, float dt) {

            if (bullet.IsDead()) {
                return;
            }

            ref int frameIndex = ref bullet.frameIndex;
            List<BulletFrameModel> frames = bullet.frames;
            var frame = frames[frameIndex];

            frame.loopModel.Tick(dt, out bool istrigger, out bool isNextFrame);

            if (istrigger) {
                BulletDomain.Frame_Acting(ctx, bullet, frame, dt);
                // Debug.Log("frameIndex" + frameIndex);
            }

            frame.loopModel.CheckDone();//检测是否结束

            if (frame.loopModel.isDone) {
                Debug.Log("frameIndex" + frameIndex + " 结束了");
                frameIndex += 1;
            }

        }

        #endregion

        static void Bullet_HitRole(BulletFrameModel frame) {
            frame.loopModel.SetHitCount(1);
        }

        static void Bullet_HitBullet(BulletFrameModel frame) {
            frame.loopModel.SetHitCount(1);
        }
        #region Frame:Acting

        static void Frame_Acting(GameSystemContext ctx, BulletEntity bullet, BulletFrameModel frame, float dt) {
            ref var loopModel = ref frame.loopModel;
            if (loopModel.isDone) {
                return;
            }

            if (frame.hasBulletBehaviour) {
                BulletBehaviour_Acting(ctx, bullet, frame.bulletBehaviourModel, dt);
            }

            if (frame.hasWorldBehaviour) {
                WorldBehaviour_Acting(ctx, bullet, frame.worldBehaviourModel, dt);
            }

        }

        #endregion

        #region  BulletBehaviour

        static void BulletBehaviour_Acting(GameSystemContext ctx, BulletEntity bullet, BulletBehaviourModel model, float dt) {

            if (model.isMove) {
                // 正常移动
                Bullet_Shaped(ctx, bullet);
            }

            if (model.isStop) {
                // 停顿
                Bullet_Stop(ctx, bullet);
                // Skill_Boom(ctx, bullet);
            }

            if (model.isRoleMoveSpeedDown) {
                // 降低移动速度
                Debug.LogWarning("角色降低移动速度");
                Bullet_DownRoleMoveSpeed(ctx, bullet);
            }

            if (model.isHaveContinueDamege) {
                // 持续伤害
                Debug.LogWarning("持续伤害");
                Bullet_ContinuousDamageStart(ctx, bullet);
            }

            if (model.isHaveContinueBoomDamege) {
                // 爆炸
                Debug.LogWarning("爆炸");
                Bullet_BoomStart(ctx, bullet);
            }

            if (model.isDestorySelf) {
                // 子弹死亡
                UnSpawn(ctx, bullet);
            }

            if (model.isRoleMoveDown) {
                // 角色下移一格
                Debug.LogWarning("角色下移一格");
                Bullet_RoleMoveDown(ctx, bullet);
            }

            if (model.isRoleMoveRight) {
                // 角色击退
                Debug.LogWarning("角色击退");
                Bullet_RoleMoveBack(ctx, bullet);
            }

            if (model.isRoleElectricity) {
                // 带电
                Debug.LogWarning("带电");
                Bullet_Electricity(ctx, bullet);
            }

            if (model.isHaveConverge) {
                // 汇聚
                Debug.LogWarning("汇聚");
                Skill_Converge(ctx, bullet);
            }
        }
        static void WorldBehaviour_Acting(GameSystemContext ctx, BulletEntity bullet, WorldBehaviourModel model, float dt) {
            if (model.isSpawnStuff) {
                Debug.Log("生成物品");
            }

            if (model.isSpawnBullet) {
                Debug.Log("生成子弹");
                BulletDomain.SpawnByPos(ctx, model.bulletSo.tm.typeID, bullet.TF_GetPos(), new Vector3(0, 0, 0));
            }

            if (model.isSpawnLaser) {
                Debug.Log("生成激光");
            }
        }

        #endregion

        #region 子弹想行为
        public static void Bullet_Shaped(GameSystemContext ctx, BulletEntity bullet) {
            // bullet.Loco_Shaped();
            bullet.Set_MoveType(bullet.moveType);
        }
        static void Bullet_BoomStart(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            Debug.Log("爆炸");
            bullet.isStartBoomDamege = true;
        }
        static void Bullet_ContinuousDamageStart(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            Debug.Log("持续伤害");
            bullet.isStartContinueDamege = true;
        }
        static void Bullet_Stop(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            bullet.Set_MoveType(BulletMoveType.None);
        }
        static void Bullet_DownRoleMoveSpeed(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            // 撞击的角色的速度减半
            ctx.roleRepository.TryGet(bullet.touchRoleIDSig, out RoleEntity role);
            var attaCom = role.AttributeComponent;
            float moveSpeed = attaCom.GetMoveSpeed();
            attaCom.SetMoveSpeed(moveSpeed / 2);
        }

        static void Skill_Converge(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("汇聚");
            bullet.isHaveConverge = true;
        }

        static void Bullet_RoleMoveDown(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("角色下移一格 换行");
            ctx.roleRepository.TryGet(bullet.touchRoleIDSig, out RoleEntity role);
            role.isChangeLine = true;
            role.changeLineDir = bullet.changeLineDir;
            role.changeLineLength = bullet.LineNum * bullet.LineLength;
        }

        static void Bullet_RoleMoveBack(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("角色击退");
            ctx.roleRepository.TryGet(bullet.touchRoleIDSig, out RoleEntity role);
            role.isRepulsed = true;
            role.repulsedLength = bullet.repeLength;
        }

        static void Bullet_Electricity(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("带电");
            ctx.roleRepository.TryGet(bullet.touchRoleIDSig, out RoleEntity role);

            role.elecDamage = bullet.elecDamage;
            role.elecTimes = bullet.elecTimes;
            role.elecMax = bullet.elecMax;
            role.isElectricity = true;
        }

        #endregion
    }
}