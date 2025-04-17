using System;
using UnityEngine;

namespace UF {
    public static class EliminateWarController { // 资源战
        public static void Enter(GameSystemContext ctx, int selectedRoleTypeID, int[] skillorTypeIDArray, int sciType, StageTM tm) {

            Debug.Log("EliminateWarController Enter");
            var game = ctx.gameEntity;
            var playerData = game.playerSaveData;

            // UI
            ctx.appUI.HUD_Eliminate_Open(game.gameMax);

            // Map
            MapEntity map = MapDomain.Spawn(ctx.assetsModule, ctx.idService, tm.mapPrefab);
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
                StuffDomain.SpawnBySpawner(ctx, spawnerTM);
            }

            game.stuffCD = 0;

            // Role 生成障碍物和核心
            for (int i = 0; i < tm.roleSpawns.Length; i += 1) {
                RoleSpawnerTM spawnerTM = tm.roleSpawns[i];
                var role = RoleDomain.SpawnBySpawner(ctx, spawnerTM);
                role.SetRbGravityScale(0);
            }
        }

        public static void PreTick(GameSystemContext ctx, float dt) {
            var owner = ctx.Get_Owner();
            RoleDomain.Input_Record(ctx, owner, dt);

        }

        public static void Tick(GameSystemContext ctx, float dt) {

            var game = ctx.gameEntity;
            var playerSaveData = game.playerSaveData;
            var owner = ctx.Get_Owner();

            // Bullet
            int bulletLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);

            for (int i = 0; i < bulletLen; i += 1) {
                BulletEntity bullet = bullets[i];
                BulletDomain.CheckUnSpawn(ctx, bullet, dt);
                BulletDomain.FSM_All_Execute(ctx, bullet, dt);
            }

            if (game.state == GameState.GameOver) {
                return;
            }

            game.gameTime += dt;
            game.stuffCD -= dt;
            playerSaveData.eliTakeTimes += dt;

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

                StuffDomain.FSM_Normal_Execute(ctx, stuff, dt);
                StuffDomain.CheckUnSpawn(ctx, stuff);
            }

            // 时间道具生成
            if (game.stuffCD <= 0) {
                game.stuffCD = game.stuffMax;
                var stuff = StuffDomain.Spawn(ctx, 200);
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

            var energy = game.energyNumber;
            if (energy >= 4) {
                game.state = GameState.GameOver;

                ctx.appUI.HUD_Eliminate_Update(progress, game.energyNumber);
                ctx.appUI.Panel_EliminateGameOver_Open(true, 3); // 四个能量棒必定三星  

                // 保存数据 得到的星级 和 通关的关卡
                int stageID = ctx.appUI.playerSaveData.GetStageCurID();
                ctx.appUI.playerSaveData.Add_Stage_PassedStar(stageID, 3, StagePassStatus.Passed);

                // 添加关卡
                GameDomain.Add_ReachStage(ctx);
            }

            if (game.gameTime >= game.gameMax) {
                game.state = GameState.GameOver;

                if (energy <= 1) {
                    ctx.appUI.HUD_Eliminate_Update(progress, game.energyNumber);
                    ctx.appUI.Panel_EliminateGameOver_Open(false, 0);

                } else {
                    int star = 0;
                    switch (star) {
                        case 2:
                            star = 1;
                            break;
                        case 3:
                            star = 2;
                            break;
                        case 4:
                            star = 3;
                            break;
                        default:
                            star = 0;
                            break;
                    }
                    ctx.appUI.HUD_Eliminate_Update(progress, game.energyNumber);
                    ctx.appUI.Panel_EliminateGameOver_Open(true, star);

                    // 保存数据 得到的星级 和 通关的关卡
                    int stageID = ctx.appUI.playerSaveData.GetStageCurID();
                    ctx.appUI.playerSaveData.Add_Stage_PassedStar(stageID, star, StagePassStatus.Passed);

                    // 添加关卡
                    GameDomain.Add_ReachStage(ctx);
                }
            }
        }

        public static void LastTick(GameSystemContext ctx, float dt) {

        }
    }
}