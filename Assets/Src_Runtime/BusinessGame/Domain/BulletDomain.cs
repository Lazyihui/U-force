using System;
using UnityEngine;

namespace UF {
    public static class BulletDomain {
        public static void SpawnBySpawn() {

        }

        public static BulletEntity Spawn(GameContext ctx, int typeID) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, Vector3.zero, typeID);
            return null;
        }
    }
}