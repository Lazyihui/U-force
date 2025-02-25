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
                Debug.Log("游戏结束");
                game.wavesProgress = 1;
                return;
            }

            if (game.cdMinion >= 0) {
                Debug.Log("cd中");
                return;
            }

            game.spawnEnemyIntervalTimer -= dt;
            if (game.spawnEnemyIntervalTimer < 0) {
                Debug.Log("生成小怪");
                ctx.towerRepository.tempTower = null;

                game.spawnEnemyTimer -= 1;

                // TODO:从Tower里面TM获取怪物的类型
                var playerData = game.playerSaveData;
                int typeID = playerData.GetRandomEnemyTypeID();
                RoleDomain.SpawnByPos(ctx, typeID, tower.GetPos());

                game.spawnEnemyIntervalTimer = game.spawnEnemyInterval;
            } else {
                // 进度条逻辑
                float pro = game.progress;
                float minionMax = game.spawnEnemyInterval;
                float progress = pro * dt / minionMax;
                game.wavesProgress += progress;
            }

            if (game.spawnEnemyTimer <= 0) {
                Debug.Log("波次结束");
                game.curWaves += 1;

                game.spawnEnemyTimer = game.spawnEnemy;
                game.cdMinion = game.cdMinionMax;
                game.cdMinion = game.cdMinionMax;
            }

        }


    }
}
