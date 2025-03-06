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
                frameIndex += 1;
            }

        }

        #endregion

        static void Bullet_HitRole(BulletFrameModel frame) {
            frame.loopModel.hitCount += 1;
            Debug.Log("击中次数" + frame.loopModel.hitCount);
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
                Debug.LogWarning("停顿");
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

        }

        #endregion

        #region 子弹想行为
        public static void Bullet_Shaped(GameSystemContext ctx, BulletEntity bullet) {
            // bullet.Loco_Shaped();
            Debug.LogWarning("正常移动");
            bullet.Set_MoveType(BulletMoveType.Shaped);
            Debug.Log("bullet.MoveType" + bullet.moveType);
        }
        static void Bullet_BoomStart(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            Debug.Log("爆炸");
            bullet.isStartBoomDamege = true;
        }
        static void Bullet_ContinuousDamageStart(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            Debug.Log("停止持续伤害");
            bullet.isStartContinueDamege = true;
        }
        static void Bullet_Stop(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            Debug.Log("停顿 ");
            // bullet.Move_Stop();
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

        static void Bullet_Appear(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("现形");
        }

        static void Bullet_Disappear(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("隐身 次状态不可以攻击");
        }

        static void Skill_Converge(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("汇聚");
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

        #endregion
    }
}