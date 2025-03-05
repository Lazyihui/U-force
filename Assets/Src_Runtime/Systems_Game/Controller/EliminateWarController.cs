using System;
using UnityEngine;

namespace UF {
    public static class EliminateWarController { // 资源战
        public static void Enter(GameSystemContext ctx, int stageID, int selectedRoleTypeID, int[] skillorTypeIDArray, int sciType, StageTM tm) {

            Debug.Log("EliminateWarController Enter");
            var game = ctx.gameEntity;
            var playerData = game.playerSaveData;

            // UI
            ctx.appUI.HUD_Eliminate_Open(game.gameMax);

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

            owner = RoleDomain.SpawnByPos(ctx, selectedRoleTypeID, new Vector3(-15, 0, 0));
            game.OwnerIDsig = owner.idSig;

            // stuff 生成道具
            for (int i = 0; i < tm.stuffSpawns.Length; i += 1) {
                StuffSpawnerTM spawnerTM = tm.stuffSpawns[i];
                StuffDoamin.SpawnBySpawner(ctx, spawnerTM);
            }

            // Role 生成障碍物和核心
            for (int i = 0; i < tm.roleSpawns.Length; i += 1) {
                RoleSpawnerTM spawnerTM = tm.roleSpawns[i];
                var role = RoleDomain.SpawnBySpawner(ctx, spawnerTM);
                role.SetRbType(RigidbodyType2D.Static);
            }
        }

        public static void PreTick(GameSystemContext ctx, float dt) {
            var owner = ctx.Get_Owner();
            RoleDomain.Input_Record(ctx, owner, dt);

        }

        public static void Tick(GameSystemContext ctx, float dt) {

            var game = ctx.gameEntity;
            var owner = ctx.Get_Owner();

            game.gameTime += dt;
            game.stuffCD -= dt;

            // UI
            var progress = game.gameTime / game.gameMax;
            ctx.appUI.HUD_Eliminate_Update(progress, game.energyNumber);
            ctx.appUI.Panel_Eliminate_Button_Open();

            // Player
            RoleDomain.LocoDefendAndOffensive_Execute(ctx, owner, dt);
            RoleDomain.FSM_All_Execute(ctx, owner, dt);
            var fsm = owner.FSMComponent;
            var state = fsm.Status;
            var attrCom = owner.AttributeComponent;
            RoleDomain.DetectionMinionByPos(ctx, owner, owner.TF_GetPos(), attrCom.GetBaseDamage());

            if (game.state == GameState.GameOver) {
                return;
            }

            if (state == RoleFSMStatus.Normal) {
                RoleDomain.FSM_Normal_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Casting) {
                RoleDomain.FSM_Casting_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Dead) {

            }


            // 绳子Tick
            RopeDomain.FSM_All_Tick(ctx, owner.GetPos(), dt);

            // Stuff
            int stuffLen = ctx.stuffRepository.TakeAll(out StuffEntity[] stuffs);
            for (int i = 0; i < stuffLen; i += 1) {
                StuffEntity stuff = stuffs[i];

                StuffDoamin.FSM_Normal_Execute(ctx, stuff);
                StuffDoamin.CheckUnSpawn(ctx, stuff);
            }

            // 时间道具生成
            if (game.stuffCD <= 0) {
                game.stuffCD = game.stuffMax;
                var stuff = StuffDoamin.Spawn(ctx, 200);
                var pos = new Vector2(UnityEngine.Random.Range(-9, -7), UnityEngine.Random.Range(-5, 8));
                stuff.TF_SetPos(pos);
            }

            // Role 障碍物 核心
            int roleLen = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < roleLen; i += 1) {
                RoleEntity role = roles[i];
                if (!role.isEliminateRole) { continue; }

                RoleDomain.FSM_AllEliminate_Execute(ctx, role, dt);
                RoleDomain.Loco_MoveEliminate(ctx, role, dt);
            }

            // Bullet
            int bulletLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);

            for (int i = 0; i < bulletLen; i += 1) {
                BulletEntity bullet = bullets[i];
                BulletDomain.CheckUnSpawn(ctx, bullet, dt);
                BulletDomain.FSM_All_Execute(ctx, bullet, dt);
            }

            if (game.energyNumber >= 4 || game.gameTime >= game.gameMax) {
                Debug.Log("游戏结束");
            }
        }

        public static void LastTick(float dt) {

        }
    }
}