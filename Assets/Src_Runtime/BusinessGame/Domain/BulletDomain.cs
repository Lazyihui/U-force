using System;
using UnityEngine;

namespace UF {
    public static class BulletDomain {
        public static void SpawnBySpawn() {

        }

        public static BulletEntity Spawn(GameContext ctx, int typeID, Vector3 pos, Vector3 dir) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, typeID, pos, dir);
            return null;
        }

        public static BulletEntity SpawnByRole_Spawner(GameContext ctx, RoleEntity role, BulletSpawnerTM spawnerTM) {
            // TODO:生成位置

            var BulletEntity = BulletDomain.Spawn(ctx, spawnerTM.bulletSo.tm.typeID, spawnerTM.spawnPos.position, spawnerTM.spawnDir.position);
            return BulletEntity;
        }

    }
}