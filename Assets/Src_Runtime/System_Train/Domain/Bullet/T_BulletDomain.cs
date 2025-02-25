using System;
using UnityEngine;

namespace UF.System_Train {
    public static class BulletDomain {
        public static BulletEntity Spawn(TrainSystemContext ctx, int typeID, Vector3 pos, RoleEntity role) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsModule, ctx.templateMoudle, ctx.idService, typeID, pos, Vector3.zero);

            if (role.t_roleType == T_RoleType.Player1) {
                entity.type = BulletMoveType.Shaped;
            } else if (role.t_roleType == T_RoleType.Player2) {
                entity.type = BulletMoveType.ToLeft;
            }

            ctx.bulletRepository.Add(entity);
            return null;
        }

        public static BulletEntity SpawnByRolePos(TrainSystemContext ctx, RoleEntity role) {
            var pos = role.GetMuzzlePos();

            var bullet = Spawn(ctx, 100, pos, role);
            return bullet;
        }

        public static void UnSpawn(GameSystemContext ctx, BulletEntity bullet) {
            ctx.bulletRepository.Remove(bullet);
            bullet.TearDown();
        }

        public static void Clear(GameSystemContext ctx) {
            int len = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);
            for (int i = 0; i < len; i += 1) {
                BulletEntity bullet = bullets[i];
                bullet.TearDown();
            }
            ctx.bulletRepository.Clear();
        }

        public static void FSM_All_Execute(BulletEntity bullet, float dt) {
            if (bullet.type == BulletMoveType.Shaped) {
                Loco_Shaped(bullet);
            } else if (bullet.type == BulletMoveType.ToLeft) {
                Loco_ToLeft(bullet);
            }
        }

        public static void Loco_Shaped(BulletEntity bullet) {
            bullet.Loco_Shaped();
        }

        public static void Loco_ToLeft(BulletEntity bullet) {
            bullet.Loco_ToLeft();
        }
    }
}