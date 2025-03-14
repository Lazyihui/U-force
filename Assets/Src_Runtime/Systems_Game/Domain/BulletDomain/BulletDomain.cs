using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {
    public static partial class BulletDomain {

        #region Lifecycle
        public static BulletEntity SpawnByPos(GameSystemContext ctx, int typeID, Vector3 pos, Vector3 dir) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID, pos, dir);
            ctx.bulletRepository.Add(entity);
            return entity;
        }

        public static BulletEntity SpawnByRole_Spawner(GameSystemContext ctx, RoleEntity role, BulletSpawnerTM spawnerTM) {
            // TODO:生成位置
            var BulletEntity = BulletDomain.SpawnByPos(ctx, spawnerTM.bulletSo.tm.typeID, spawnerTM.spawnPos.position, spawnerTM.spawnDir.position);
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

            // 子弹移动
            Bullet_Loco(ctx, bullet, dt);

            // 持续伤害
            if (bullet.isStartContinueDamege) {
                Bullet_ContinuousDamage(ctx, bullet, dt);
            }
            if (bullet.isStartBoomDamege) {
                Bullet_ContinuousBoomDamage(ctx, bullet, dt);
            }

            if (bullet.isHaveContinueDamege) {
                Bullet_ContinuousBoomDamage(ctx, bullet, dt);
                Bullet_DestoryOneself(ctx, bullet, dt);
            }

            // 汇聚
            if (bullet.isHaveConverge) {
                Bullet_Converge(ctx, bullet, dt);
            }
        }
        #endregion

        public static void Bullet_DestoryOneself(GameSystemContext ctx, BulletEntity bullet, float dt) {
            bullet.destoryOneselfTime -= dt;
            if (bullet.destoryOneselfTime < 0) {
                UnSpawn(ctx, bullet);
            }
        }

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
                    RoleDomain.HpReduce_Minion(ctx, minion, bullet.damage);
                    minion.isHit = true;

                    bullet.hitTimes -= 1;
                    if (bullet.hitTimes <= 0) {
                        UnSpawn(ctx, bullet, minion);
                    }

                    if (bullet.isHit) { continue; }
                    Bullet_HitRole(bullet.frames[bullet.frameIndex]);
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
                        RoleDomain.HpReduce_Minion(ctx, minion, bullet.continueDamege);
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

                // 持续伤害
                bullet.damageTimer += dt;
                if (bullet.damageTimer > bullet.damageInterval) {
                    bullet.damageTimer = 0;
                    if (dis < bullet.damageRange) {
                        var feaCom = minion.FeatureComponent;
                        if (feaCom.featureType == FeatureType.DisAppear) {
                            continue;
                        }
                        RoleDomain.HpReduce_Minion(ctx, minion, bullet.continueDamege);
                    }
                }

            }
        }

        static void Bullet_Converge(GameSystemContext ctx, BulletEntity bullet, float dt) {
            if (bullet.convergeTimes <= 0) { return; }

            Debug.Log("Bullet_Converge");

            bullet.convergeTimes -= 1;

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = roles[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }
                var minionPos = minion.TF_GetPos();
                float dis = Vector3.Distance(bullet.TF_GetPos(), minionPos);

                if (dis < bullet.convergeRange) {
                    int y = Bullet_GetYNearTower(bullet);
                    float minionY = minionPos.y;

                    if (minionY == y) { continue; }

                    Vector2 dir = Vector2.zero;
                    dir.y = y - minionY > 0 ? 1 : -1;
                    float length = Math.Abs(y - minionY);

                    minion.changeLineDir = dir;
                    minion.changeLineLength = length;
                    minion.isChangeLine = true;
                }
            }
        }

        static int Bullet_GetYNearTower(BulletEntity bullet) {
            float centerY = bullet.TF_GetPos().y;
            int y = (int)Math.Round(centerY);

            if (y % 2 != 0) {
                if ((int)centerY % 2 == 0) {
                    y = (int)centerY;
                } else {
                    y = (int)centerY + 1;
                }
            }

            if (y > 6) {
                y = 6;
            } else if (y < -4) {
                y = -4;
            }

            return y;
        }
        #endregion


    }
}