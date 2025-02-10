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

        public void Inject(AppUI appUI) {
            ctx.Inject(appUI);
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


            eventCenter.OnStageSelectionHandle += (id) => {
                events.OnStageClick(id);
            };


        }
    }
}