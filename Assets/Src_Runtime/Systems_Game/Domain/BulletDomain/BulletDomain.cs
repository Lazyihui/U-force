using System;
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

        #endregion

        #region FSM
        public static void FSM_All_Execute(GameSystemContext ctx, BulletEntity bullet, float dt) {
            if (bullet.type == BulletMoveType.Shaped) {
                Loco_Shaped(ctx, bullet);
            }
        }
        #endregion

        #region  Loco 行为
        public static void Loco_Shaped(GameSystemContext ctx, BulletEntity bullet) {
            bullet.Loco_Shaped();
        }
        #endregion

        #region detection

        public static void detectionMinionByPos(GameSystemContext ctx, BulletEntity bullet, Vector3 pos) {

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] minions);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = minions[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }

                float dis = Vector3.Distance(minion.TF_GetPos(), pos);

                if (dis < 1.0f) {
                    RoleDomain.HpReduce_MinionFall(ctx, minion, bullet.damage);
                    bullet.hitTimes -= 1;

                    if (bullet.hitTimes <= 0) {
                        UnSpawn(ctx, bullet);
                    }
                }
            }
        }

        #endregion
    }
}