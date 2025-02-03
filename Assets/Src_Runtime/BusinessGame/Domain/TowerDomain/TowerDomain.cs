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


        public static TowerEntity SpawnBySpawner(GameContext ctx, TowerSpawnerTM spawnerTM) {
            TowerEntity entity = GameFactory.Tower_CreateBySpawner(ctx.assetsCore, ctx.templateCore, ctx.idService, spawnerTM);
            ctx.towerRepository.Add(entity);
            return entity;
        }

        public static void TrySpawnMinion(GameContext ctx, float dt) {


            // TODO:问j 有没有更好的写法

            TowerEntity tower = ctx.towerRepository.GetRandom();
            // tower = ctx.towerRepository.Get(2);

            if (tower == null) {
                Debug.Log("TrySpawnMinion tower == null");
                return;
            }


            tower.cd -= dt;
            if (tower.cd >= 0) {
                return;
            }

            tower.intervalTimer -= dt;
            if (tower.intervalTimer < 0) {
                ctx.towerRepository.tempTower = null;
                Debug.Log("生成怪物");

                // TODO:从Tower里面TM获取怪物的类型
                RoleDomain.SpawnByPos(ctx, tower.GetPos(), 200);
                tower.intervalTimer = tower.interval;
            }


            tower.maintainTimer -= dt;
            if (tower.maintainTimer < 0) {
                tower.maintainTimer = tower.maintain;
                tower.cd = tower.cdMax;
            }

            // 后面改记录Tower可以生成什么种类的怪物
        }
    }
}
