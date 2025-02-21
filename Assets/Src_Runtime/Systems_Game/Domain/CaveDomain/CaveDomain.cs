using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public static class CaveDomain {

        public static void Tower_Execute(GameSystemContext ctx, CaveEntity tower, float dt) {
        }

        public static CaveEntity Spawn(GameSystemContext ctx, int typeID, Vector2 pos) {
            CaveEntity entity = GameFactory.Tower_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID, pos);

            ctx.towerRepository.Add(entity);
            return entity;
        }


        public static CaveEntity SpawnBySpawner(GameSystemContext ctx, CaveSpawnerTM spawnerTM) {
            CaveEntity entity = GameFactory.Tower_CreateBySpawner(ctx.assetsModule, ctx.templateCore, ctx.idService, spawnerTM);
            ctx.towerRepository.Add(entity);
            return entity;
        }

        public static void Clear(GameSystemContext ctx) {
            int len = ctx.towerRepository.TakeAll(out CaveEntity[] towers);
            for (int i = 0; i < len; i += 1) {
                CaveEntity tower = towers[i];
                tower.TearDown();
            }
            ctx.towerRepository.Clear();
        }

        public static void TrySpawnMinion(GameSystemContext ctx, float dt) {


            var game = ctx.gameEntity;
            CaveEntity tower = ctx.towerRepository.GetRandom();

            if (tower == null) {
                return;
            }

            game.cdMinion -= dt;

            if (game.curWaves >= game.maxWaves) {
                game.wavesProgress = 1;
                return;
            }

            if (game.cdMinion >= 0) {
                return;
            }

            game.spawnMinionIntervalTimer -= dt;
            if (game.spawnMinionIntervalTimer < 0) {
                ctx.towerRepository.tempTower = null;

                game.spawnMinionTimer -= 1;

                // TODO:从Tower里面TM获取怪物的类型
                RoleDomain.SpawnByPos(ctx, tower.GetPos(), 200);

                game.spawnMinionIntervalTimer = game.spawnMinionInterval;
            } else {
                // 进度条逻辑
                float pro = game.progress;
                float minionMax = game.spawnMinionInterval;
                float progress = pro * dt / minionMax;
                game.wavesProgress += progress;
            }

            if (game.spawnMinionTimer <= 0) {
                game.curWaves += 1;

                game.spawnMinionTimer = game.spawnMinion;
                game.cdMinion = game.cdMinionMax;
                game.cdMinion = game.cdMinionMax;
            }

        }


    }
}
