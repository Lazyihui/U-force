using System;
using UnityEngine;

namespace UF {
    public static class BulletDomain {

        #region Lifecycle
        public static BulletEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 dir) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, typeID, pos, dir);

            ctx.bulletRepository.Add(entity);
            return null;
        }

        public static BulletEntity SpawnByRole_Spawner(GameContext ctx, RoleEntity role, BulletSpawnerTM spawnerTM) {
            // TODO:生成位置

            var BulletEntity = BulletDomain.Spawn(ctx, spawnerTM.bulletSo.tm.typeID, spawnerTM.spawnPos.position, spawnerTM.spawnDir.position);
            return BulletEntity;
        }
        #endregion

        #region FSM
        public static void FSM_All_Execute(GameContext ctx, BulletEntity bullet, float dt) {
            if (bullet.type == BulletType.Shaped) {
                Loco_Shaped(ctx, bullet);
            }
        }
        #endregion

        #region  Loco 行为
        public static void Loco_Shaped(GameContext ctx, BulletEntity bullet) {
            bullet.Loco_Shaped();
        }
        #endregion

        #region detection

        public static void detectionMinionByPos(GameContext ctx, BulletEntity bullet, Vector3 pos) {

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] minions);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = minions[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }

                float dis = Vector3.Distance(minion.TF_GetPos(), pos);

                if (dis < 1.0f) {
                    Debug.Log("打到敌人");


                }


            }
        }

        #endregion
    }
}