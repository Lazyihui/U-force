using System;
using System.Collections.Generic;
using UnityEngine;



namespace UF {

    public class GameSystem : MonoBehaviour {
        public SystemType SystemType => SystemType.Game;

        static GameSystemContext ctx;

        public GameSystemEvents events;

        public void Ctor() {
            ctx = new GameSystemContext();
            events = new GameSystemEvents();
        }

        public void Inject(Canvas ScreenCanvas, AssetsModule assetsCore, TemplateMoudle templateCore, AppUI appUI, InputModule inputModule, RoleRepository roleRepository) {
            ctx.Inject(ScreenCanvas, assetsCore, templateCore, appUI, inputModule, roleRepository);
        }

        public void Exit() {
            ctx.isRunning = false;
        }

        public void Continue() {
            ctx.isRunning = true;
        }

        public void Enter(int stageID) {

            ctx.isRunning = true;
            var game = ctx.gameEntity;

            // UI
            ctx.appUI.Panel_GameDefend_Button_Open();
            ctx.appUI.HUD_Progressbar_Open(game.maxWaves);

            // Game
            // int stageID = game.stageCurID;
            
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

            // Tower
            for (int i = 0; i < tm.towerSpawns.Length; i += 1) {
                CaveSpawnerTM spawnerTM = tm.towerSpawns[i];
                CaveDomain.SpawnBySpawner(ctx, spawnerTM);
            }

        }


        public void Tick(float dt) {

            if (!ctx.isRunning) {
                return;
            }

            var game = ctx.gameEntity;

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

            if (game.state == GameState.GameOver) {
                StopRole_AllMove();
                return;
            }


            RoleEntity owner = ctx.Get_Owner();

            Game_Win(owner);


            // 生成怪物 不需要循环
            CaveDomain.TrySpawnMinion(ctx, dt);

            // UI
            int hp = owner.Attr_Hp_Get();
            int maxHp = owner.Attr_HpMax_Get();
            ui.HUD_Progressbar_Update(game.wavesProgress, hp, maxHp);//更新进度条
        }

        void LogicTick(float dt) {
            var game = ctx.gameEntity;
            
            //  Role
            RoleEntity owner = ctx.Get_Owner();
            if (owner == null) {
                return;
            }

            // Player的逻辑
            RoleDomain.Input_Record(ctx, owner, dt);
            RoleDomain.DetectionMinionByPos(ctx, owner, owner.TF_GetPos());

            var fsm = owner.FSMComponent;
            var state = fsm.Status;

            if (game.state == GameState.GameOver)
            {
                return;
            }

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


            //   bullet
            int bulletLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);

            for (int i = 0; i < bulletLen; i += 1) {
                BulletEntity bullet = bullets[i];
                BulletDomain.FSM_All_Execute(ctx, bullet, dt);
                BulletDomain.detectionMinionByPos(ctx, bullet, bullet.TF_GetPos());
            }


            //  tower
            int towerLen = ctx.towerRepository.TakeAll(out CaveEntity[] towers);

            for (int i = 0; i < towerLen; i += 1) {
                CaveEntity tower = towers[i];
                CaveDomain.Tower_Execute(ctx, tower, dt);
            }

        }

        void LastTick(float dt) {

        }

        #region Role
        void StopRole_AllMove() {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] minions);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = minions[i];
                minion.Move_Stop();
            }
        }

        #endregion

        // 游戏结算
        void Game_Win(RoleEntity owner) {
            var game = ctx.gameEntity;
            // 游戏结束
            bool isGameWin = GameDomain.isGameWin(ctx);
            if (isGameWin) {
                // 没找到这里StopRole_AllMove();要多运行一遍
                owner.Jump_Stop();
                owner.Move_Stop();
                Exit();
                ctx.appUI.Panel_GameOver_Open(true, game.hurt, game.shoot, game.hitNumber, game.hitRate);
                Debug.Log("GameWin");
                game.state = GameState.GameOver;

            }
        }

        public void Game_Binding() {
            var ui = ctx.appUI;
            UIEventCenter eventCenter = ui.eventCenter;

            var events = this.events;

            // 暂停
            eventCenter.OnGamePauseHandle += () => {
                events.OnGamePause();
            };

            // 攻击
            eventCenter.OnAttackHandle += () => {
                events.OnAttack();
            };

            eventCenter.OnTechHandle += () => {
                events.OnTech();
            };

            eventCenter.OnWeap1Handle += () => {
                events.OnWeap1();
            };

            eventCenter.OnWeap2Handle += () => {
                events.OnWeap2();
            };

            eventCenter.OnWeap3Handle += () => {
                events.OnWeap3();
            };

            eventCenter.OnWeap4Handle += () => {
                events.OnWeap4();
            };

            eventCenter.OnGameOverHandle += () => {
                events.OnGameOver();
            };
        }
    }
}