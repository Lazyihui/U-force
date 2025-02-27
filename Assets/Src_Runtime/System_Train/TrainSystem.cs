using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF.System_Train {
    public class TrainSystem : MonoBehaviour {
        #region 基础
        public SystemType systemType => SystemType.Train;

        TrainSystemContext ctx;

        TrainSystemEvents events;
        public TrainSystemEvents Events => events;

        public void Ctor() {
            ctx = new TrainSystemContext();
            events = new TrainSystemEvents();
        }

        public void Inject(AppUI appUI, TemplateMoudle templateMoudle, AssetsModule assetsModule, InputModule inputModule, RoleRepository roleRepository, BulletRepository bulletRepository, PropRepository propRepository) {
            ctx.Inject(appUI, templateMoudle, assetsModule, inputModule, roleRepository, bulletRepository, propRepository);
        }

        public void Enter(List<int> typeIDs) {
            ctx.isRunning = true;

            // UI
            ctx.appUI.Panel_Training_Button_Show();

            int stageID = 500;
            bool has = ctx.templateMoudle.Stage_TryGet(stageID, out StageTM tm);
            if (!has) {
                Debug.LogError("StageID not found");
                return;
            }

            // Map
            MapEntity map = MapDomain.Spawn(ctx.assetsModule, ctx.idService, tm.mapPrefab);
            var train = ctx.trainEntity;
            train.mapEntity = map;

            // Role
            if (typeIDs.Count < 2) {
                Debug.LogError("没有获取到两个role");
                return;
            }

            Vector2 pos1 = new Vector2(-15, 0);
            Vector2 pos2 = new Vector2(15, 0);
            RoleEntity player1 = RoleDomain.SpawnByPos(ctx, pos1, typeIDs[0]);
            RoleEntity player2 = RoleDomain.SpawnByPos(ctx, pos2, typeIDs[1]);
            player1.t_roleType = T_RoleType.Player1;
            player2.t_roleType = T_RoleType.Player2;

            train.gameTime = train.gameMax;
            train.propSpawnCD = 0;
        }

        public void Exit() {
            ctx.isRunning = false;

            ClearGame();
        }

        public void Tick(float dt) {
            if (!ctx.isRunning) {
                return;
            }

            PreTick(dt);

            ref float restFixTime = ref ctx.restFixTime;

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

            LastTick(dt);
        }

        void PreTick(float dt) {

        }

        void LogicTick(TrainSystemContext ctx, float dt) {
            var train = ctx.trainEntity;
            if (train.state == TrainState.Pause) { return; }

            train.defenseCD1 -= dt;
            train.defenseCD2 -= dt;
            train.attackCD1 -= dt;
            train.attackCD2 -= dt;
            train.propSpawnCD -= dt;
            train.player1Trap -= dt;
            train.player2Trap -= dt;
            train.player1Shields -= dt;
            train.player2Shields -= dt;
            train.spriteCD -= dt;

            train.gameTime -= dt;
            if (train.gameTime <= 0) {
                GameOver();
            }

            HUD_Training_Update(dt);

            // 所有角色逻辑
            ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            //不知道为什么, 用length一直是3, 会有bug
            for (int i = 0; i < 2; i++) {
                RoleEntity role = roles[i];
                RoleDomain.Loco_Execute(ctx, role, dt);

                RoleDomain.Input_Record(ctx, role, dt);

                HUD_Training_CD_Update(role);
            }

            // Button
            int buttonLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);
            for (int i = 0; i < buttonLen; i++) {
                BulletEntity bullet = bullets[i];
                BulletDomain.FSM_All_Execute(bullet);
                BulletDomain.CheckUnSpawn(ctx, bullet);
            }

            // Prop
            int propLen = ctx.propRepository.TakeAll(out PropEntity[] props);
            for (int i = 0; i < propLen; i++) {
                PropEntity prop = props[i];
                PropDomain.FSM_All_Execute(ctx, prop);
                PropDomain.CheckUnSpawn(ctx, prop);
            }

            // 场景道具生成
            if (train.propSpawnCD <= 0) {
                PropDomain.SpawnByRandom(ctx);
                train.propSpawnCD = train.propSpawnMax;
            }
        }

        void LastTick(float dt) {

        }

        public void Train_Binding() {
            var appUI = ctx.appUI;
            UIEventCenter eventCenter = appUI.eventCenter;

            var events = this.events;

            eventCenter.OnTrainPauseClickHandle += () => {
                events.OnTrainPauseClick();
            };

            eventCenter.OnTrainContinueClickHandle += () => {
                events.OnTrainContinueClick();
            };

            eventCenter.OnTrainIllustratedClickHandle += () => {
                events.OnTrainIllustratedClick();
            };

            eventCenter.OnTrainQuitClickHandle += () => {
                events.OnTrainQuitClick();
            };

            eventCenter.OnTrainOverResetClickHandle += () => {
                events.OnTrainOverResetClick();
            };

            eventCenter.OnTrainOverBackClickHandle += () => {
                events.OnTrainOverBackClick();
            };
        }
        #endregion

        #region 其他
        void GameOver() {
            var train = ctx.trainEntity;

            ctx.roleRepository.TakeAll(out var roles);
            var player1 = roles[0];
            var player2 = roles[1];

            int hp1 = player1.AttributeComponent.GetHp();
            int hp2 = player2.AttributeComponent.GetHp();

            if (hp1 > hp2) {
                string text = "Player1胜利!";
                ctx.templateMoudle.Role_TryGet(player1.typeID, out var tm);
                ctx.appUI.Panel_Training_GameOver_Open(text, tm.sprite);
            } else if (hp2 > hp1) {
                string text = "Player2胜利!";
                ctx.templateMoudle.Role_TryGet(player2.typeID, out var tm);
                ctx.appUI.Panel_Training_GameOver_Open(text, tm.sprite);
            } else {
                ctx.templateMoudle.Role_TryGet(player1.typeID, out var tm1);
                ctx.templateMoudle.Role_TryGet(player2.typeID, out var tm2);

                GameOverNoWinner(tm1.sprite, tm2.sprite);
            }
        }

        void GameOverNoWinner(Sprite sprite1, Sprite sprite2) {
            var train = ctx.trainEntity;

            if (train.spriteCD <= 0) {
                switch (train.spriteIndex) {
                    case 1:
                        train.spriteIndex = 2;
                        ctx.appUI.Panel_Training_GameOver_SetWinner(sprite2);
                        break;
                    case 2:
                        train.spriteIndex = 1;
                        ctx.appUI.Panel_Training_GameOver_SetWinner(sprite1);
                        break;
                    default:
                        break;
                }

                train.spriteCD = train.spriteMax;
            }

            string text = "平局!";
            ctx.appUI.Panel_Training_GameOver_Open(text, sprite1);
        }

        public void ClearGame() {
            BulletDomain.Clear(ctx);
            PropDomain.Clear(ctx);
            RoleDomain.Clear(ctx);
            MapDomain.Clear(ctx.trainEntity.mapEntity);

            ctx.appUI.HUD_Training_Close();
            ctx.appUI.HUD_Training_CD_Close();
            ctx.appUI.Panel_Training_Select_Close();
            ctx.appUI.Panel_Training_Tip_Close();
            ctx.appUI.Panel_Training_Button_Close();
            ctx.appUI.Panel_Training_Pause_Close();
            ctx.appUI.Panel_Training_GameOver_Close();
        }

        // 绝对不是这样写, 这是一种最傻瓜的办法
        public void EnterPause() {
            var train = ctx.trainEntity;
            train.state = TrainState.Pause;

            ctx.appUI.Panel_Training_Pause_Show();
        }

        public void ExitPause() {
            var train = ctx.trainEntity;
            train.state = TrainState.None;

            ctx.appUI.Panel_Training_Pause_Close();
        }

        void HUD_Training_Update(float dt) {
            ctx.roleRepository.TakeAll(out RoleEntity[] roles);

            var player1Attr = roles[0].AttributeComponent;
            int player1Hp = player1Attr.GetHp();
            int player1Max = player1Attr.GetHpMax();

            var player2Attr = roles[1].AttributeComponent;
            int player2Hp = player2Attr.GetHp();
            int player2Max = player2Attr.GetHpMax();

            var train = ctx.trainEntity;
            float time = train.gameTime;

            ctx.appUI.HUD_Training_Update(player1Hp, player1Max, player2Hp, player2Max, time);
        }

        void HUD_Training_CD_Update(RoleEntity role) {
            var train = ctx.trainEntity;

            if (role.t_roleType == T_RoleType.Player1) {
                ctx.appUI.HUD_Training_CD_Update(role, train.attackCD1, train.attackMax1, train.defenseCD1, train.defenseMax);
            } else if (role.t_roleType == T_RoleType.Player2) {
                ctx.appUI.HUD_Training_CD_Update(role, train.attackCD2, train.attackMax2, train.defenseCD2, train.defenseMax);
            }
        }
        #endregion
    }
}