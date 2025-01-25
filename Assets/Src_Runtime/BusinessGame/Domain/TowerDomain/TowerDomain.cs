using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public static class TowerDomain {

        public static void Tower_Execute(GameContext ctx, TowerEntity tower, float dt) {
            TrySpawnMinion(ctx, dt);
        }

        public static TowerEntity Spawn(GameContext ctx, int typeID, Vector2 pos) {
            TowerEntity entity = GameFactory.Tower_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, typeID, pos);

            ctx.towerRepository.Add(entity);
            return entity;
        }

        public static void TrySpawnMinion(GameContext ctx, float dt) {

            // 得到随机的一个Tower
            int len = ctx.towerRepository.GetRandom(out var tower);

            if (tower == null) {
                return;
            }

            if (!tower.AllowSpawn(dt)) {
                return;
            }
            // 后面改记录Tower可以生成什么种类的怪物
            RoleDomain.SpawnByPos(ctx, tower.transform.position, 200);

        }
    }
}
