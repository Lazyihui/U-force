using System;
using UnityEngine;

namespace UF {
    public static class EliminateWarController { // 资源战
        public static void Enter(GameSystemContext ctx, int stageID, int selectedRoleTypeID, int[] skillorTypeIDArray, int sciType, StageTM tm) {

            Debug.Log("EliminateWarController Enter");
            var game = ctx.gameEntity;
            var playerData = game.playerSaveData;

            // UI
            ctx.appUI.HUD_Progressbar_Open(game.maxWaves);
            ctx.appUI.Panel_WatchTower_ClearCursor();//清除塔的光标

            // Map
            MapEntity map = MapDomain.Spawn(ctx.assetsModule, ctx.idService, tm.mapPrefab);
            ctx.gameEntity.playerSaveData.SetStageCurID(stageID);
            ctx.gameEntity.playerSaveData.mapEntity = map;

            // Role
            RoleEntity owner = null;
            if (selectedRoleTypeID == -1) {
                for (int i = 0; i < tm.roleSpawns.Length; i += 1) {
                    RoleSpawnerTM spawnerTM = tm.roleSpawns[i];
                    owner = RoleDomain.SpawnBySpawner(ctx, spawnerTM);
                    if (owner.roleType == RoleType.Player) {
                        game.OwnerIDsig = owner.idSig;
                    }
                }
            }

            // Stuff 测试抓取
            var stuff = StuffDoamin.Spawn(ctx, 100);
            stuff.TF_SetPos(new Vector2(3, -2));

            owner = RoleDomain.SpawnByPos(ctx, selectedRoleTypeID, new Vector3(-15, 0, 0));
            game.OwnerIDsig = owner.idSig;

        }

        public static void PreTick(GameSystemContext ctx, float dt) {
            var owner = ctx.Get_Owner();
            RoleDomain.Input_Record(ctx, owner, dt);

        }

        public static void Tick(GameSystemContext ctx, float dt) {

            var game = ctx.gameEntity;
            var owner = ctx.Get_Owner();

            // 人物移动
            RoleDomain.LocoDefendAndOffensive_Execute(ctx, owner, dt);


            // 绳子Tick
            RopeDomain.FSM_All_Tick(ctx, owner.GetPos(), dt);

            // Stuff
            int stuffLen = ctx.stuffRepository.TakeAll(out StuffEntity[] stuffs);
            for (int i = 0; i < stuffLen; i += 1) {
                StuffEntity stuff = stuffs[i];

                StuffDoamin.FSM_Normal_Execute(ctx, stuff);
                StuffDoamin.CheckUnSpawn(ctx, stuff);
            }
        }

        public static void LastTick(float dt) {

        }
    }
}