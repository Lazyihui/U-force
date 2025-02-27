using System;
using UnityEngine;

namespace UF {
    public static class OffensiveWarController {
        public static void Enter(GameSystemContext ctx, int stageID, int selectedRoleTypeID, int[] skillorTypeIDArray, int selectSciTypeID, StageTM tm) {

        }

        public static void PreTick(GameSystemContext ctx, float dt) {
            var game = ctx.gameEntity;

            CaveDomain.TrySpawnBoss(ctx, dt);
            RoleEntity owner = ctx.Get_Owner();

            // 输入记录
            RoleDomain.Input_Record(ctx, owner, dt);
            // UI
            var attrCom = owner.AttributeComponent;
            UIDomain.HUD_Progressbar_Update(ctx.appUI, attrCom, game.wavesProgress);
        }

        public static void Tick(GameSystemContext ctx, float dt) {

            var game = ctx.gameEntity;
            //  Role
            RoleEntity owner = ctx.Get_Owner();
            if (owner == null) {
                return;
            }

            // Player的逻辑 所有状态都要执行
            var fsm = owner.FSMComponent;
            var state = fsm.Status;
            var attrCom = owner.AttributeComponent;
            RoleDomain.DetectionMinionByPos(ctx, owner, owner.TF_GetPos(), attrCom.GetBaseDamage());

            if (game.state == GameState.GameOver) {
                return;
            }

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];
                // 这里是player和Minion的逻辑
                RoleDomain.FSM_AllOffensive_Execute(ctx, role, dt);
            }

            // 这里是主角的逻辑
            if (state == RoleFSMStatus.Normal) {
                RoleDomain.FSM_Normal_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Casting) {
                RoleDomain.FSM_Casting_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Dead) {

            }

            //   bullet
            int bulletLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);

            for (int i = 0; i < bulletLen; i += 1) {
                BulletEntity bullet = bullets[i];
                BulletDomain.FSM_All_Execute(ctx, bullet, dt);
                BulletDomain.CheckUnSpawn(ctx, bullet);
            }


            //  tower
            int towerLen = ctx.towerRepository.TakeAll(out CaveEntity[] towers);

            for (int i = 0; i < towerLen; i += 1) {
                CaveEntity tower = towers[i];
                CaveDomain.FSM_All_Execute(ctx, tower, dt);
            }

        }

        public static void LastTick(float dt) {

        }
    }
}