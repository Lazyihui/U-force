using System;
using UnityEngine;

namespace UF {
    public class PauseSystem : MonoBehaviour {

        PauseSystemContext ctx;

        public PauseSystemEvents events;

        public void Ctor() {
            ctx = new PauseSystemContext();
            events = new PauseSystemEvents();
        }

        public void Inject(GameEntity gameEntity, AppUI appUI, RoleRepository roleRepository) {
            ctx.Inject(gameEntity, appUI, roleRepository);
        }


        public void Enter() {
            GameEntity game = ctx.gameEntity;

            game.state = GameState.GamePause;

            // TODO: 业务逻辑

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];
                role.Move_Stop();
            }

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


        public void Pause_Binding() {
            var ui = ctx.appUI;
            UIEventCenter eventCenter = ui.eventCenter;

            var events = this.events;

            eventCenter.OnContinueHandle += () => {
                events.OnContinue();
            };

            eventCenter.OnIllustratedHandle += () => {
                events.OnIllustrated();
            };

            eventCenter.OnPauseExitGameHandle += () => {
                events.OnExitGame();
            };

        }
    }
}