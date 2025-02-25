using System;
using UnityEngine;

namespace UF {
    public static class OffensiveWarController {
        public static void Enter(GameSystemContext ctx, int stageID, int selectedRoleTypeID, int[] skillorTypeIDArray, int selectSciTypeID, StageTM tm) {

        }

        public static void PreTick(GameSystemContext ctx, float dt) {
            CaveDomain.TrySpawnBoss(ctx, dt);
        }

        public static void Tick(GameSystemContext ctx, float dt) {
            RoleEntity owner = ctx.Get_Owner();

            RoleDomain.LocoDefend_Execute(ctx, owner, dt);
        }

        public static void LastTick(float dt) {

        }
    }
}