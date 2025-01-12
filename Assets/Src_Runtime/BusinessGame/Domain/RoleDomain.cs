using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public static class RoleDomain {
        public static RoleEntity Spawn(GameContext ctx, int typeID) {
            RoleEntity role = GameFactory.Role_CreateBySpawner(ctx.assetsCore, ctx.templateCore, ctx.idService, Vector3.zero, typeID);
            ctx.roleRepository.Add(role);
            return role;
        }

        public static void Tick(GameContext ctx, RoleEntity role) {



            if (role.Fsm.Status == RoleFSMStatus.Normal) {
                if (Input.GetKeyDown(KeyCode.A)) {
                    Debug.Log("RoleDomain: Tick: Space" + role.Fsm.Status);
                    role.Fsm.Casting_Enter();
                }
            }

            if (role.Fsm.Status == RoleFSMStatus.Casting) {
                if (Input.GetKeyDown(KeyCode.S)) {
                    Debug.Log("RoleDomain: Tick: J" + role.Fsm.Status);
                    BulletDomain.Spawn(ctx, 100);
                }
            }

            if (role.Fsm.Status == RoleFSMStatus.Dead) {

            }
        }
    }
}