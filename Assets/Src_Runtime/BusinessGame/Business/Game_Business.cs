using System;
using System.Collections.Generic;
using UnityEngine;



namespace UF {

    public static class Game_Business {

        public static void Enter(GameContext ctx) {

            var game = ctx.gameEntity;
            game.state = GameState.Game;

            int stageID = game.stageCurID;
            bool has = ctx.templateCore.Stage_TryGet(stageID, out StageTM tm);
            if (!has) {
                Debug.LogError("StageID not found");
                return;
            }

            // Role
            for (int i = 0; i < tm.roleSpawns.Length; i += 1) {
                RoleSpawnerTM spawnerTM = tm.roleSpawns[i];
                RoleEntity role = RoleDomain.SpawnBySpawner(ctx, spawnerTM);
                if (role.roleType == RoleType.Player) {
                    game.OwnerIDsig = role.idSig;
                }
            }

            // RoleEntity role = RoleDomain.Spawn(ctx, 100);
            // game.OwnerIDsig = role.idSig;
            // Tower
            for (int i = 0; i < tm.towerSpawns.Length; i += 1) {
                TowerSpawnerTM spawnerTM = tm.towerSpawns[i];
                TowerDomain.SpawnBySpawner(ctx, spawnerTM);
            }

        }


        public static void Tick(GameContext ctx, float dt) {
            PreTick(ctx, dt);

            ref float restFixTime = ref ctx.gameEntity.restFixTime;

            restFixTime += dt;

            const float FIX_INTERVAL = 0.020f;

            if (restFixTime <= FIX_INTERVAL) {

                LogicTick(ctx, restFixTime);

                restFixTime = 0;
            } else {
                while (restFixTime >= FIX_INTERVAL) {
                    LogicTick(ctx, FIX_INTERVAL);
                    restFixTime -= FIX_INTERVAL;
                }
            }

            LastTick(ctx, dt);
        }

        static void PreTick(GameContext ctx, float dt) {
            // 生成怪物 不需要循环
            TowerDomain.TrySpawnMinion(ctx, dt);

        }

        static void LogicTick(GameContext ctx, float dt) {

            #region Role
            RoleEntity owner = ctx.Get_Owner();
            Debug.Assert(owner != null, "owner != null");
            RoleDomain.Input_Record(ctx, owner, dt);

            var fsm = owner.FSMComponent;
            var state = fsm.Status;

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];
                // 这里是player和Minion的逻辑
                RoleDomain.FSM_All_Execute(ctx, role, dt);
            }

            // 这里是主角的逻辑
            if (state == RoleFSMStatus.Normal) {
                RoleDomain.FSM_Normal_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Casting) {
                RoleDomain.FSM_Casting_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Dead) {

            }
            // 所有状态都要执行



            #endregion

            #region  bullet
            int bulletLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);

            for (int i = 0; i < bulletLen; i += 1) {
                BulletEntity bullet = bullets[i];
                BulletDomain.FSM_All_Execute(ctx, bullet, dt);
            }

            #endregion

            #region tower
            int towerLen = ctx.towerRepository.TakeAll(out TowerEntity[] towers);

            for (int i = 0; i < towerLen; i += 1) {
                TowerEntity tower = towers[i];
                TowerDomain.Tower_Execute(ctx, tower, dt);
            }
            #endregion


        }

        static void LastTick(GameContext ctx, float dt) {

        }
    }
}