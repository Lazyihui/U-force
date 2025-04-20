using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {
    public static class CaveDomain {

        public static void FSM_All_Execute(GameSystemContext ctx, CaveEntity tower, float dt) {

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

        // 从塔里生成小怪
        public static void TrySpawnMinionbyCave(GameSystemContext ctx, float dt) {

            var game = ctx.gameEntity;
            CaveEntity tower = ctx.towerRepository.GetRandom();

            if (tower == null) {
                return;
            }

            game.cdEnemy -= dt;
            if (game.curWaves >= game.maxWaves) {
                game.wavesProgress = 1;
                return;
            }
            if (game.cdEnemy >= 0) {
                return;
            }

            game.spawnEnemyIntervalTimer -= dt;
            if (game.spawnEnemyIntervalTimer < 0) {
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
                game.curWaves += 1;
                game.spawnEnemyTimer = game.spawnEnemyCount;
                game.cdEnemy = game.cdEnemyMax;
                game.cdEnemy = game.cdEnemyMax;
            }

        }

        // 随机位置生成小怪
        public static void TrySpawnEnemyRandomPos(GameSystemContext ctx, float dt) {

            var game = ctx.gameEntity;

            game.cdEnemy -= dt;
            if (game.curWaves >= game.maxWaves) {
                game.wavesProgress = 1;
                return;
            }

            if (game.cdEnemy >= 0) {
                return;
            }

            game.spawnEnemyIntervalTimer -= dt;
            if (game.spawnEnemyIntervalTimer < 0) {
                ctx.towerRepository.tempTower = null;

                game.spawnEnemyTimer -= 1;

                // TODO:怪物类型还没有
                var playerData = game.playerSaveData;
                int typeID = playerData.GetRandomEnemyTypeID();

                // 这里要得到坐标
                Vector2 pos = Extension.GetSquareAnnulusRandomPos(new Vector2(0, 0), 60, 33.75f);
                RoleDomain.SpawnByPos(ctx, typeID, pos);

                game.spawnEnemyIntervalTimer = game.spawnEnemyInterval;
            } else {
                // 进度条逻辑
                float pro = game.progress;
                float minionMax = game.spawnEnemyInterval;
                float progress = pro * dt / minionMax;
                game.wavesProgress += progress;
            }

            if (game.spawnEnemyTimer <= 0) {
                game.curWaves += 1;
                game.spawnEnemyTimer = game.spawnEnemyCount;
                game.cdEnemy = game.cdEnemyMax;
                game.cdEnemy = game.cdEnemyMax;
            }

        }
        public static void TrySpawnBoss(GameSystemContext ctx, float dt) {
            var game = ctx.gameEntity;
            var playerDate = game.playerSaveData;

            CaveEntity tower = null;
            ctx.towerRepository.TakeAll(out CaveEntity[] towers);
            if (towers.Length > 0) {
                tower = towers[0];
            }

            int typeID = playerDate.GetRandomEnemyTypeID();

            if (game.curWaves >= game.maxWaves) {
                return;
            }

            game.cdEnemy -= dt;

            if (game.cdEnemy >= 0) {
                return;
            }

            game.spawnEnemyIntervalTimer -= dt;
            if (game.spawnEnemyIntervalTimer < 0) {
                game.spawnEnemyTimer -= 1;
                Debug.Assert(tower != null, "tower!=null");
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
                game.spawnEnemyTimer = game.spawnEnemyCount;
                game.cdEnemy = game.cdEnemyMax;
                game.curWaves += 1;
            }

        }


    }
}
