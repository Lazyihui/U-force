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

        public void Inject(GameEntity game, AppUI appUI) {
            ctx.Inject(game, appUI);
        }

        public void Enter() {
            // ui
            ctx.appUI.Panel_Login_Open();

            GameEntity game = ctx.gameEntity;
            game.state = GameState.Login;

        }


        public void Tick(float dt) {
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

        }

        void LogicTick(float dt) {

        }

        void LastTick(float dt) {

        }
    }
}