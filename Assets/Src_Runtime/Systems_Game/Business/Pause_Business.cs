using System;
using UnityEngine;

namespace UF {
    public static class Pause_Business {
        public static void Enter(GameContext ctx) {
            GameEntity game = ctx.gameEntity;

            game.state = GameState.GamePause;

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];
                RoleDomain.FSM_All_Pause(ctx, role);
            }
        }


        public static void Tick(GameContext ctx, float dt) {
            PreTick(ctx, dt);

            ref float restFixTime = ref ctx.gameEntity.restFixTime;

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

            LastTick(ctx, dt);
        }

        static void PreTick(GameContext ctx, float dt) {

        }

        static void LogicTick(GameContext ctx, float dt) {

        }

        static void LastTick(GameContext ctx, float dt) {

        }
    }
}