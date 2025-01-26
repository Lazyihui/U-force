using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public static class TowerDomain {

        public static void Tower_Execute(GameContext ctx, TowerEntity tower, float dt) {
        }

        public static TowerEntity Spawn(GameContext ctx, int typeID, Vector2 pos) {
            TowerEntity entity = GameFactory.Tower_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, typeID, pos);

            ctx.towerRepository.Add(entity);
            return entity;
        }

        public static void TrySpawnMinion(GameContext ctx, float dt) {
            // TODO:问j 有没有更好的写法
            TowerEntity tower = ctx.towerRepository.tempTower;

            if (tower == null) {
                ctx.towerRepository.GetRandom(out tower);
                ctx.towerRepository.tempTower = tower;
            } else {
                tower.intervalTimer -= dt;
                if (tower.intervalTimer <= 0) {
                    RoleDomain.SpawnByPos(ctx, tower.GetPos(), 200);
                    tower.intervalTimer = tower.interval;
                    ctx.towerRepository.tempTower = null;
                }
            }

            // // 后面改记录Tower可以生成什么种类的怪物

            // var game = ctx.gameEntity;
            // game.spawnMinionIntervalTimer -= dt;
            // Debug.Log("game.spawnMinionIntervalTimer: " + game.spawnMinionIntervalTimer);
            // if (game.spawnMinionIntervalTimer <= 0) {
            //     ctx.towerRepository.GetRandom(out TowerEntity tower);
            //     RoleDomain.SpawnByPos(ctx, tower.GetPos(), 200);
            //     game.spawnMinionIntervalTimer = game.spawnMinionInterval;
            // }


        }
    }
}
