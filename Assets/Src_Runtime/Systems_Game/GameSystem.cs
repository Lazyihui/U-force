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

        public void Inject(Canvas ScreenCanvas, AssetsModule assetsCore, TemplateMoudle templateCore, AppUI appUI, InputModule inputModule, RoleRepository roleRepository, BulletRepository bulletRepository, GameSaveModel playerSaveData, Camera camera, SaveCore saveCore, SaveSlotFullDTO saveSlotFullDTO) {
            ctx.Inject(ScreenCanvas, assetsCore, templateCore, appUI, inputModule, roleRepository, bulletRepository, playerSaveData, camera, saveCore, saveSlotFullDTO);
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
            StuffDomain.Clear(ctx);
            ctx.gameEntity.Clear();

            // 2.关闭所有Ui
            appUI.Reset();
            appUI.Panel_GameOver_Close();
            appUI.Panel_GamePause_Close();
            appUI.Panel_GameDefend_Button_Close();
            appUI.HUD_Progressbar_Close();
            appUI.HUD_Eliminate_Close();
            appUI.Panel_Eliminate_Pause_Close();
            appUI.Panel_Eliminate_Button_Close();
        }

        public void Enter(int stageID, int selectedRoleTypeID, int[] skillorTypeIDArray, int sciType) {

            ctx.isRunning = true;
            var game = ctx.gameEntity;
            //stageID ;
            bool has = ctx.templateCore.Stage_TryGet(stageID, out StageTM tm);
            if (!has) {
                Debug.LogError("StageID not found");
                return;
            }

            game.Clear();
            game.InitFromTM(tm.gameTM);

            if (game.isDefendWar) {
                Debug.Log("进入防守战");
                DefendWarController.Enter(ctx, stageID, selectedRoleTypeID, skillorTypeIDArray, sciType, tm);
            } else if (game.isOffensiveWar) {
                // 进攻战和防守战的Enter一样
                DefendWarController.Enter(ctx, stageID, selectedRoleTypeID, skillorTypeIDArray, sciType, tm);
                RoleEntity owner = ctx.Get_Owner();
                owner.moveType = RoleMoveType.OffensiveUDRL;
            } else if (game.isEliminateWar) {
                EliminateWarController.Enter(ctx, stageID, selectedRoleTypeID, skillorTypeIDArray, sciType, tm);
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

            if (!game.isEliminateWar) {
                RoleEntity owner = ctx.Get_Owner();
                Game_Win(owner);
            }


            if (game.state == GameState.GameOver) {
                StopRole_AllMove();
                return;
            }



            if (game.isDefendWar) {
                DefendWarController.PreTick(ctx, dt);
            } else if (game.isOffensiveWar) {
                OffensiveWarController.PreTick(ctx, dt);
            } else if (game.isEliminateWar) {
                EliminateWarController.PreTick(ctx, dt);
            }

        }

        void LogicTick(float dt) {
            var game = ctx.gameEntity;

            if (game.isDefendWar) {
                DefendWarController.Tick(ctx, dt);
            } else if (game.isOffensiveWar) {
                OffensiveWarController.Tick(ctx, dt);
            } else if (game.isEliminateWar) {
                EliminateWarController.Tick(ctx, dt);
            }
        }

        void LastTick(float dt) {
            var game = ctx.gameEntity;

            if (game.isDefendWar) {
                DefendWarController.LastTick(ctx, dt);
            } else if (game.isOffensiveWar) {
                OffensiveWarController.LastTick(ctx, dt);
            } else if (game.isEliminateWar) {
                EliminateWarController.LastTick(ctx, dt);
            }
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
                var playerData = ctx.gameEntity.playerSaveData;

                Debug.Assert(playerData != null, "playerDate==null");
                Debug.Assert(owner != null, "owner==null");
                if (owner.state == RoleState.Dead) {
                    ctx.gameEntity.state = GameState.GameOver;
                    ctx.appUI.Panel_GameOver_Open(false, 0);
                }

                // 保存数据

                owner.Jump_Stop();
                owner.Move_Stop();
                Exit();
                int starCount = playerData.GetStarCount();
                // 技术到达的关卡
                ctx.appUI.Panel_GameOver_Open(true, starCount);
                game.state = GameState.GameOver;

                // 保存数据 得到的星级 和 通关的关卡
                int stageID = appUI.playerSaveData.GetStageCurID();
                appUI.playerSaveData.AddStagePassed(stageID,starCount);

                // 添加关卡
                GameDomain.Add_ReachStage(ctx);
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

            eventCenter.OnGameOverHandle += (bool isWin) => {
                events.OnGameOver(isWin);
            };

            eventCenter.OnGameIllusCloseClickHandle += () => {
                events.OnGameIllusCloseClick();
            };

            eventCenter.OnGameIllusCardClickHandle += (int typeID) => {
                events.OnGameIllusCardClick(typeID);
            };

            eventCenter.OnGameIllusStuffCardClickHandle += (int typeID) => {
                events.OnGameIllusStuffCardClick(typeID);
            };
        }
    }
}