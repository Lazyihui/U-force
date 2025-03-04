using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public static class BulletDomain {

        #region Lifecycle
        public static BulletEntity Spawn(GameSystemContext ctx, int typeID, Vector3 pos, Vector3 dir) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID, pos, dir);
            ctx.bulletRepository.Add(entity);
            return entity;
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
            Frames_Tick(ctx, bullet, dt);
        }
        #endregion

        #region  Loco 行为

        #endregion

        #region detection检测
        public static void CheckUnSpawn(GameSystemContext ctx, BulletEntity bullet, float dt) {
            Vector3 pos = bullet.TF_GetPos();
            if (pos.x < -21 || pos.x > 21) {
                UnSpawn(ctx, bullet);
            }

            if (bullet.isStartBoomDamege || bullet.isStartContinueDamege) {
                return;
            }

            detectionMinionByPos(ctx, bullet, pos, 1);

            if (bullet.isStartContinueDamege) {
                Bullet_ContinuousDamage(ctx, bullet, dt);
            }
            if (bullet.isStartBoomDamege) {
                Bullet_ContinuousBoomDamage(ctx, bullet, dt);
            }
        }

        static void detectionMinionByPos(GameSystemContext ctx, BulletEntity bullet, Vector3 pos, float range) {

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] minions);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = minions[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }

                float dis = Vector3.Distance(minion.TF_GetPos(), pos);
                if (dis < range) {
                    var feaCom = minion.FeatureComponent;
                    if (feaCom.featureType == FeatureType.DisAppear) {
                        if (bullet.isMakeBulletAppear) {
                            feaCom.SetFeatureType(FeatureType.None);
                            minion.Set_SpriteDisappear();
                        } else {
                            continue;
                        }
                    }

                    if (minion.isHit) { continue; }
                    bullet.touchRoleIDSig = minion.idSig;
                    RoleDomain.HpReduce_MinionFall(ctx, minion, bullet.damage);
                    minion.isHit = true;

                    bullet.hitTimes -= 1;
                    Debug.Log("击中次数" + bullet.hitTimes);

                    if (bullet.hitTimes <= 0) {
                        UnSpawn(ctx, bullet, minion);
                    }

                    if (bullet.isHit) { continue; }
                    var playerData = ctx.gameEntity.playerSaveData;
                    playerData.HitTimes += 1;
                    bullet.isHit = true;
                }
            }
        }

        static void Bullet_ContinuousDamage(GameSystemContext ctx, BulletEntity bullet, float dt) {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = roles[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }
                float dis = Vector3.Distance(minion.TF_GetPos(), bullet.TF_GetPos());

                if (dis < bullet.damageRange) {
                    var feaCom = minion.FeatureComponent;
                    if (feaCom.featureType == FeatureType.DisAppear) {
                        continue;
                    }

                    minion.moveType = RoleMoveType.Prohibit;
                    // 持续伤害
                    bullet.damageTimer += dt;
                    if (bullet.damageTimer > bullet.damageInterval) {
                        bullet.damageTimer = 0;
                        RoleDomain.HpReduce_MinionFall(ctx, minion, bullet.continueDamege);
                    }
                }
            }
        }

        // 同上想差别不会使敌人停止
        static void Bullet_ContinuousBoomDamage(GameSystemContext ctx, BulletEntity bullet, float dt) {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = roles[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }
                float dis = Vector3.Distance(minion.TF_GetPos(), bullet.TF_GetPos());

                if (dis < bullet.damageRange) {
                    var feaCom = minion.FeatureComponent;
                    if (feaCom.featureType == FeatureType.DisAppear) {
                        continue;
                    }
                    // 持续伤害
                    bullet.damageTimer += dt;
                    if (bullet.damageTimer > bullet.damageInterval) {
                        bullet.damageTimer = 0;
                        RoleDomain.HpReduce_MinionFall(ctx, minion, bullet.continueDamege);
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
                BulletBehaviour_Acting(ctx, bullet, frame.bulletBehaviourModel, dt);
            }

            loopModel.CheckDone();
        }

        #endregion

        #region  BulletBehaviour

        static void BulletBehaviour_Acting(GameSystemContext ctx, BulletEntity bullet, BulletBehaviourModel model, float dt) {

            if (model.isMove) {
                // 正常移动
                Bullet_Shaped(ctx, bullet);
            }

            if (model.isStop) {
                // 爆炸
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
                Debug.LogWarning("子弹死亡");
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
            Debug.Log("子弹移动");
            bullet.Loco_Shaped();
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
            role.RepulsedLength = bullet.repeLength;
        }

        #endregion
    }
}