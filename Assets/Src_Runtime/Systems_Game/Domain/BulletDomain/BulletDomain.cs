using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public static class BulletDomain {

        #region Lifecycle
        public static BulletEntity Spawn(GameSystemContext ctx, int typeID, Vector3 pos, Vector3 dir) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID, pos, dir);
            ctx.bulletRepository.Add(entity);
            return null;
        }

        public static BulletEntity SpawnByRole_Spawner(GameSystemContext ctx, RoleEntity role, BulletSpawnerTM spawnerTM) {
            // TODO:生成位置
            var BulletEntity = BulletDomain.Spawn(ctx, spawnerTM.bulletSo.tm.typeID, spawnerTM.spawnPos.position, spawnerTM.spawnDir.position);
            return BulletEntity;
        }

        public static void UnSpawn(GameSystemContext ctx, BulletEntity bullet) {
            ctx.bulletRepository.Remove(bullet);
            bullet.TearDown();
        }

        public static void UnSpawn(GameSystemContext ctx, BulletEntity bullet, RoleEntity role) {
            ctx.bulletRepository.Remove(bullet);
            bullet.TearDown();
        }

        public static void Clear(GameSystemContext ctx) {
            int len = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);
            for (int i = 0; i < len; i += 1) {
                BulletEntity bullet = bullets[i];
                bullet.TearDown();
            }
            ctx.bulletRepository.Clear();
        }

        #endregion

        #region FSM
        public static void FSM_All_Execute(GameSystemContext ctx, BulletEntity bullet, float dt) {
            // if (bullet.type == BulletMoveType.Shaped) {
            //     Loco_Shaped(ctx, bullet);
            // }

            Frames_Tick(ctx, bullet, dt);
        }
        #endregion

        #region  Loco 行为
        public static void Loco_Shaped(GameSystemContext ctx, BulletEntity bullet) {
            bullet.Loco_Shaped();
        }
        #endregion

        #region detection检测
        public static void CheckUnSpawn(GameSystemContext ctx, BulletEntity bullet) {
            Vector3 pos = bullet.TF_GetPos();
            if (pos.x < -21 || pos.x > 21) {
                UnSpawn(ctx, bullet);
            }

            detectionMinionByPos(ctx, bullet, pos);
        }

        static void detectionMinionByPos(GameSystemContext ctx, BulletEntity bullet, Vector3 pos) {

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] minions);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = minions[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }

                float dis = Vector3.Distance(minion.TF_GetPos(), pos);

                if (dis < 1.0f) {
                    bullet.touchRoleIDSig = minion.idSig;

                    RoleDomain.HpReduce_MinionFall(ctx, minion, bullet.damage);
                    bullet.hitTimes -= 1;

                    if (bullet.hitTimes <= 0) {
                        UnSpawn(ctx, bullet, minion);
                    }
                }
            }
        }
        #endregion

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
                Debug.Log("istrigg");
                BulletDomain.Frame_Acting(ctx, bullet, frame, dt);
            }

            if (isNextFrame) {
                frameIndex += 1;
            }

        }

        #endregion

        #region Frame:Acting

        static void Frame_Acting(GameSystemContext ctx, BulletEntity bullet, BulletFrameModel frame, float dt) {
            ref var loopModel = ref frame.loopModel;
            if (loopModel.isDone) {
                return;
            }

            if (frame.hasBulletBehaviour) {
                Debug.LogWarning("有角色行为");
                BulletBehaviour_Acting(ctx, bullet, frame.bulletBehaviourModel);
            }

            loopModel.CheckDone();
        }

        #endregion

        #region  BulletBehaviour

        static void BulletBehaviour_Acting(GameSystemContext ctx, BulletEntity bullet, BulletBehaviourModel model) {

            if (model.isMove) {
                // 正常移动
                Loco_Shaped(ctx, bullet);
            }

            if (model.isStop) {
                // 爆炸
                Debug.LogWarning("爆炸");
                bullet.Move_Stop();
                // Skill_Boom(ctx, bullet);
            }

            if (model.isRoleMoveSpeedDown) {
                // 降低移动速度
                Debug.LogWarning("角色降低移动速度");
                Bullet_DownRoleMoveSpeed(ctx, bullet);
            }

        }

        #endregion

        #region 子弹想行为

        static void Bullet_Boom(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("爆炸");
        }
        static void Bullet_ContinuousDamage(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("持续伤害");
        }
        static void Bullet_Stop(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            Debug.Log("停顿 ");
            bullet.Move_Stop();
        }
        static void Bullet_DownRoleMoveSpeed(GameSystemContext ctx, BulletEntity bullet) {
            // 写完
            // 撞击的角色的速度减半
            ctx.roleRepository.TryGet(bullet.touchRoleIDSig, out RoleEntity role);
            var attaCom = role.AttributeComponent;
            float moveSpeed = attaCom.GetMoveSpeed();
            attaCom.SetMoveSpeed(moveSpeed / 2);
        }
        static void Skill_Down(GameSystemContext ctx, BulletEntity bullet, RoleEntity role) {
            role.isControlledDown = true;
        }

        static void Bullet_Appear(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("现形");
        }

        static void Bullet_Disappear(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("隐身 次状态不可以攻击");
        }

        static void Skill_Back(GameSystemContext ctx, BulletEntity bullet, RoleEntity role) {
            role.isControlledRight = true;
        }



        static void Skill_Converge(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("汇聚");
        }
        #endregion
    }
}