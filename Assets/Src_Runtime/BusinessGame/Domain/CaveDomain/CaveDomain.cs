using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF
{
    public static class CaveDomain
    {

        public static void Tower_Execute(GameContext ctx, CaveEntity tower, float dt)
        {
        }

        public static CaveEntity Spawn(GameContext ctx, int typeID, Vector2 pos)
        {
            CaveEntity entity = GameFactory.Tower_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, typeID, pos);

            ctx.towerRepository.Add(entity);
            return entity;
        }


        public static CaveEntity SpawnBySpawner(GameContext ctx, CaveSpawnerTM spawnerTM)
        {
            CaveEntity entity = GameFactory.Tower_CreateBySpawner(ctx.assetsCore, ctx.templateCore, ctx.idService, spawnerTM);
            ctx.towerRepository.Add(entity);
            return entity;
        }

        public static void TrySpawnMinion(GameContext ctx, float dt)
        {

            // TODO:问j 有没有更好的写法

            var game = ctx.gameEntity;
            CaveEntity tower = ctx.towerRepository.GetRandom();

            if (tower == null)
            {
                Debug.Log("TrySpawnMinion tower == null");
                return;
            }

            game.cdMinion -= dt;

            if (game.curWaves >= game.maxWaves)
            {
                Debug.Log("停止生怪 游戏结束");
                game.wavesProgress = 1;
                return;
            }

            if (game.cdMinion >= 0)
            {
                return;
            }

            game.spawnMinionIntervalTimer -= dt;
            if (game.spawnMinionIntervalTimer < 0)
            {
                ctx.towerRepository.tempTower = null;

                game.spawnMinionTimer -= 1;

                // TODO:从Tower里面TM获取怪物的类型
                RoleDomain.SpawnByPos(ctx, tower.GetPos(), 200);
                game.spawnMinionIntervalTimer = game.spawnMinionInterval;
            }
            else
            {
                // 进度条逻辑
                float pro = game.progress;
                float minionMax = game.spawnMinionInterval;
                float progress = pro * dt / minionMax;
                game.wavesProgress += progress;
            }

            if (game.spawnMinionTimer <= 0)
            {
                game.curWaves += 1;

                game.spawnMinionTimer = game.spawnMinion;
                game.cdMinion = game.cdMinionMax;
                game.cdMinion = game.cdMinionMax;
            }

        }
    }
}
