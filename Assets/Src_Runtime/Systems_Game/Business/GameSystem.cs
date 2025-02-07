using System;
using System.Collections.Generic;
using UnityEngine;



namespace UF {

    public class GameSystem : MonoBehaviour {
        public SystemType SystemType => SystemType.Game;

        static GameContext ctx;

        public void Ctor() {
            ctx = new GameContext();
        }

        public void Inject(GameEntity gameEntity, Canvas ScreenCanvas, AssetsModule assetsCore, TemplateMoudle templateCore, AppUI appUI, InputModule inputModule) {
            ctx.Inject(gameEntity, ScreenCanvas, assetsCore, templateCore, appUI, inputModule);
        }

        public void Enter() {

            var game = ctx.gameEntity;
            game.state = GameState.Game;

            // UI
            ctx.appUI.Panel_GameDefend_Button_Open();
            ctx.appUI.HUD_Progressbar_Open(game.maxWaves);

            // Game
            int stageID = game.stageCurID;
            bool has = ctx.templateCore.Stage_TryGet(stageID, out StageTM tm);
            if (!has) {
                Debug.LogError("StageID not found");
                return;
            }

            // Map
            MapDomain.Spawn(ctx, tm.mapPrefab);

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
                CaveSpawnerTM spawnerTM = tm.towerSpawns[i];
                CaveDomain.SpawnBySpawner(ctx, spawnerTM);
            }

        }


        public void Tick(float dt) {
            var game = ctx.gameEntity;

            if (game.state != GameState.Game) {
                return;
            }


            PreTick(dt);

            ref float restFixTime = ref ctx.gameEntity.restFixTime;

            restFixTime += dt;

            const float FIX_INTERVAL = 0.020f;

            if (restFixTime <= FIX_INTERVAL) {

                LogicTick(restFixTime);

                restFixTime = 0;
            } else {
                while (restFixTime >= FIX_INTERVAL) {
                    LogicTick(FIX_INTERVAL);
                    restFixTime -= FIX_INTERVAL;
                }
            }

            LastTick(dt);
        }

        void PreTick(float dt) {
            var game = ctx.gameEntity;
            var ui = ctx.appUI;

            RoleEntity owner = ctx.Get_Owner();

            // 游戏结束
            // TODO: 数值部分
            bool isGameFail = RoleDomain.isPlyerDead(ctx, owner);
            bool isGameWin = CaveDomain.isGameWin(ctx);

            bool isWin;
            int hurt = 0;
            int shoot = 0;
            int hitNumber = 0;
            float hitRate = 0;

            if (isGameFail) {
                game.state = GameState.GameOver;
                isWin = false;

                ctx.appUI.Panel_GameOver_Open(isWin, hurt, shoot, hitNumber, hitRate);
                Debug.Log("GameOver");
            } else if (isGameWin) {
                game.state = GameState.GameOver;
                isWin = true;

                ctx.appUI.Panel_GameOver_Open(isWin, hurt, shoot, hitNumber, hitRate);
            }

            // 生成怪物 不需要循环
            CaveDomain.TrySpawnMinion(ctx, dt);

            ui.HUD_Progressbar_Update(game.wavesProgress);


        }

        void LogicTick(float dt) {

            #region Role
            RoleEntity owner = ctx.Get_Owner();

            // Player的逻辑
            RoleDomain.Input_Record(ctx, owner, dt);
            RoleDomain.DetectionMinionByPos(ctx, owner, owner.TF_GetPos());

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
                BulletDomain.detectionMinionByPos(ctx, bullet, bullet.TF_GetPos());
            }

            #endregion

            #region tower
            int towerLen = ctx.towerRepository.TakeAll(out CaveEntity[] towers);

            for (int i = 0; i < towerLen; i += 1) {
                CaveEntity tower = towers[i];
                CaveDomain.Tower_Execute(ctx, tower, dt);
            }
            #endregion

        }

        void LastTick(float dt) {

        }
    }
}