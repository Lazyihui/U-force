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

        public void Enter() {
            ctx.isRunning = true;
            // ui
            ctx.appUI.Panel_Login_Open();
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
            eventCenter.OnViewClickHandle += () => {
                events.OnViewClick();
            };

            eventCenter.OnSelectionWatchCardClickHandle += (typeID) => {
                events.OnSelectionWatchCardClick(typeID);
            };

            eventCenter.OnDetailCloseHandle += () => {
                events.OnDetailCloseClick();
            };

            eventCenter.OnPlayer1ReadyClickHandle += () => {
                events.OnPlayer1ReadyClick();
            };

            eventCenter.OnPlayer2ReadyClickHandle += () => {
                events.OnPlayer2ReadyClick();
            };

            eventCenter.OnTrainTipClickHandle += () => {
                events.OnTrainTipClick();
            };

            eventCenter.OnSelectionTrainCardClickHandle += (int typeID) => {
                events.OnSelectionTrainCardClick(typeID);
            };

            eventCenter.OnTrainTipCloseClickHandle += () => {
                events.OnTrainTipCloseClick();
            };
        }
    }
}