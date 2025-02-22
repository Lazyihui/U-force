using System;
using System.Collections.Generic;
using UnityEngine;



namespace UF {

    public class GameSystem : MonoBehaviour {
        public SystemType SystemType => SystemType.Game;

        public static GameSystemContext ctx;

        public GameSystemEvents events;

        public void Ctor() {
            ctx = new GameSystemContext();
            events = new GameSystemEvents();
        }

        public void Inject(Canvas ScreenCanvas, AssetsModule assetsCore, TemplateMoudle templateCore, AppUI appUI, InputModule inputModule, RoleRepository roleRepository, PlayerEntity playerSaveData) {
            ctx.Inject(ScreenCanvas, assetsCore, templateCore, appUI, inputModule, roleRepository, playerSaveData);
        }

        public PlayerEntity Get_Player() {
            return ctx.Get_Player();
        }

        public void Exit() {
            ctx.isRunning = false;
        }

        public void Continue() {
            ctx.isRunning = true;
        }

        // 清除游戏信息
        public void ClearGame() {
            var appUI = ctx.appUI;

            BulletDomain.Clear(ctx);
            RoleDomain.Clear(ctx);
            CaveDomain.Clear(ctx);
            MapDomain.Clear(ctx.gameEntity.playerSaveData.mapEntity);
            StuffDoamin.Clear(ctx);
            ctx.gameEntity.Clear();

            // 2.关闭所有Ui
            appUI.Panel_GameOver_Close();
            appUI.Panel_GamePause_Close();
            appUI.Panel_GameDefend_Button_Close();
            appUI.HUD_Progressbar_Close();
        }

        public void Enter(int stageID, int selectedRoleTypeID, int[] skillorTypeIDArray, int sciType) {

            ctx.isRunning = true;
            var game = ctx.gameEntity;

            // UI
            ctx.appUI.Panel_GameDefend_Button_Open();
            ctx.appUI.HUD_Progressbar_Open(game.maxWaves);
            ctx.appUI.Panel_WatchTower_ClearCursor();

            bool has = ctx.templateCore.Stage_TryGet(stageID, out StageTM tm);
            if (!has) {
                Debug.LogError("StageID not found");
                return;
            }

            //game  
            game.InitFromTM(tm.gameTM);

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

            var skillorCom = owner.skillorSlotComponent;

            // 大招
            SkillModel skillorSci = GameFactory.Skillor_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, sciType);
            skillorSci.Set_InputCastKey(InputKeyFlag.Tech);
            skillorCom.Add(skillorSci);

            // 道具
            if (skillorTypeIDArray != null) {
                for (int i = 0; i < skillorTypeIDArray.Length; i += 1) {
                    var skilltypeID = skillorTypeIDArray[i];
                    SkillModel skillor = GameFactory.Skillor_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, skilltypeID);
                    skillor.Set_InputCastKeyWeap(i);
                    skillorCom.Add(skillor);
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
            var attrCom = owner.AttributeComponent;
            UIDomain.HUD_Progressbar_Update(ctx.appUI, attrCom, game.wavesProgress);
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

            if (game.state == GameState.GameOver) {
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
            var appUI = ctx.appUI;
            // 游戏结束
            bool isGameWin = GameDomain.isGameWin(ctx);

            if (Input.GetKeyDown(KeyCode.B)) {
                isGameWin = true;
            }

            if (isGameWin) {
                // 没找到这里StopRole_AllMove();要多运行一遍
                owner.Jump_Stop();
                owner.Move_Stop();
                Exit();
                var playerData = ctx.gameEntity.playerSaveData;
                int starCount = playerData.GetStarCount();
                ctx.appUI.Panel_GameOver_Open(true, playerData.GetRoleGetHurt(), playerData.shootAllTimes, playerData.hitTimes, playerData.GetRoleHitRate(), starCount);
                game.state = GameState.GameOver;

                // 保存数据 得到的星级 和 通关的关卡
                int stageID = appUI.playerSaveData.GetStageCurID();
                appUI.playerSaveData.AddStagePassed(stageID);

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

            eventCenter.OnGameOverHandle += (bool isWin) => {
                events.OnGameOver(isWin);
            };
        }
    }
}