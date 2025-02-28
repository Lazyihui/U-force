using System;
using System.Collections.Generic;
using UnityEngine;



namespace UF {

    public class LoginSystem : MonoBehaviour {

        public SystemType SystemType => SystemType.Login;

        LoginSystemContext ctx;

        LoginSystemEvents events;
        public LoginSystemEvents Events => events;

        public void Ctor() {
            ctx = new LoginSystemContext();
            events = new LoginSystemEvents();
        }

        public void Inject(AppUI appUI, InputModule input) {
            ctx.Inject(appUI, input);
        }

        public void EnterPanel_Login() {
            ctx.isRunning = true;
            // ui
            ctx.appUI.Panel_Login_Open();
        }

        public void Enter() {
            ctx.isRunning = true;
        }

        public void Exit() {
            ctx.isRunning = false;
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
            // panel
            var input = ctx.inputModule;

            var ui = ctx.appUI;
            ui.Tick(dt, input);


        }

        void LogicTick(float dt) {
            UIDomain.isEnterGameButton(ctx.appUI);
        }

        void LastTick(float dt) {

        }




        public void Login_Binding() {
            var appUI = ctx.appUI;
            UIEventCenter eventCenter = appUI.eventCenter;

            var events = this.events;

            // 开始游戏
            eventCenter.OnLoginStartHandle += () => {
                events.OnLoginStart();
            };

            // 训练
            eventCenter.OnTrainingHandle += () => {
                events.OnTraining();
            };

            // 设置
            eventCenter.OnSettingHandle += () => {
                events.OnSetting();
            };

            // 退出
            eventCenter.OnQuitHandle += () => {
                events.OnQuit();
            };


            eventCenter.OnStageClickHandle += (stageID) => {
                events.OnStageClick(stageID);
            };

            eventCenter.OnChapterClickHandle += (ChapterID) => {
                events.OnChapterClick(ChapterID);
            };

            eventCenter.OnSelectBackHandle += () => {
                events.OnSelectBack();
            };
            // CardSelection
            eventCenter.OnSelectHeroClickHandle += () => {
                events.OnSelectHeroClick();
            };

            eventCenter.OnSelectAgrClickHandle += () => {
                events.OnSelectAgrClick();
            };

            eventCenter.OnSelectIndClickHandle += () => {
                events.OnSelectIndClick();
            };

            eventCenter.OnSelectSciClickHandle += () => {
                events.OnSelectSciClick();
            };


            eventCenter.OnSelectEnterClickHandle += () => {
                events.OnSelectEnterClick();
            };//选择进入

            eventCenter.OnSelectBackClickHandle += () => {
                events.OnSelectBackClick();
            };//选择返回

            eventCenter.OnSelectionCardClickHandle += (cardID) => {
                events.OnSelectionCardClick(cardID);
            };

            eventCenter.OnConfirmClickHandle += (typeID) => {
                events.OnConfirmClick(typeID);//确认选择
            };

            eventCenter.OnCardSelectedClickHandle += (typeID) => {
                events.OnCardSelectedClick(typeID);
            };

            // 瞭望塔
            eventCenter.OnViewClickHandle += () => { // 查看敌人详情
                events.OnViewClick();
            };

            eventCenter.OnSelectionWatchCardClickHandle += (typeID) => { // 选择敌人卡片(查看的第二部)
                events.OnSelectionWatchCardClick(typeID);
            };

            eventCenter.OnDetailCloseHandle += () => { // 关闭敌人详情
                events.OnDetailCloseClick();
            };

            // 训练营
            eventCenter.OnPlayer1ReadyClickHandle += () => { // 玩家1准备
                events.OnPlayer1ReadyClick();
            };

            eventCenter.OnPlayer2ReadyClickHandle += () => { // 玩家2准备
                events.OnPlayer2ReadyClick();
            };

            eventCenter.OnTrainTipClickHandle += () => { // 训练营按键提示
                events.OnTrainTipClick();
            };

            eventCenter.OnSelectionTrainCardClickHandle += (int typeID) => { // 选择卡片(玩家)
                events.OnSelectionTrainCardClick(typeID);
            };

            eventCenter.OnTrainTipCloseClickHandle += () => { // 关闭训练营按键提示Panel
                events.OnTrainTipCloseClick();
            };
        }
    }
}