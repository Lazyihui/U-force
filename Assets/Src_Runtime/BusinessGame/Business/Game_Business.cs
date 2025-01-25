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

            TowerDomain.Spawn(ctx, 100, new Vector2(16, 9));
            TowerDomain.Spawn(ctx, 100, new Vector2(16, 6.5f));
            TowerDomain.Spawn(ctx, 100, new Vector2(16, 4));
            TowerDomain.Spawn(ctx, 100, new Vector2(16, 1.5f));
            TowerDomain.Spawn(ctx, 100, new Vector2(16, -1));
            TowerDomain.Spawn(ctx, 100, new Vector2(16, -3.5f));

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

            #region Role
            RoleEntity owner = ctx.Get_Owner();

            RoleDomain.Input_Record(ctx, owner, dt);

            var fsm = owner.FSMComponent;
            var state = fsm.Status;

            if (state == RoleFSMStatus.Normal) {
                RoleDomain.FSM_Normal_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Casting) {
                RoleDomain.FSM_Casting_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Dead) {

            }
            // 所有状态都要执行
            RoleDomain.FSM_All_Execute(ctx, owner, dt);
            #endregion

            #region  bullet
            int bulletLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);

            for (int i = 0; i < bulletLen; i += 1) {
                BulletEntity bullet = bullets[i];
                BulletDomain.FSM_All_Execute(ctx, bullet, dt);
            }

            #endregion

            #region tower
            int towerLen = ctx.towerRepository.TakeAll(out TowerEntity[] towers);

            for (int i = 0; i < towerLen; i += 1) {
                TowerEntity tower = towers[i];
                TowerDomain.Tower_Execute(ctx, tower, dt);
            }

            #endregion


        }

        static void LastTick(GameContext ctx, float dt) {

        }
    }
}