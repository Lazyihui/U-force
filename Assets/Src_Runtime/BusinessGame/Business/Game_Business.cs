using System;
using System.Collections.Generic;
using UnityEngine;



namespace UF {

    public static class Game_Business {

        public static void Enter(GameContext ctx) {

            GameEntity game = ctx.gameEntity;

            game.state = GameState.Game;

            RoleEntity role = RoleDomain.Spawn(ctx, 100);
            game.OwnerIDsig = role.idSig;

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

            RoleEntity owner = ctx.Get_Owner();

            RoleDomain.Input_Record(ctx, owner, dt);

            var fsm = owner.FSMComponent;
            var state = fsm.Status;

            RoleInpuComponent input = owner.InputComponent;
            InputKeyFlag inputKey = input.InputKeys;
            if (input.IsJump) {
                Debug.Log("Jump" + inputKey);
                input.Set_InputKey(InputKeyFlag.None);
                Debug.Log("Jumpaaaa" + inputKey);

            }

            if (state == RoleFSMStatus.Normal) {
                RoleDomain.FSM_Normal_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Casting) {

            } else if (state == RoleFSMStatus.Dead) {

            }

        }

        static void LastTick(GameContext ctx, float dt) {

        }
    }
}