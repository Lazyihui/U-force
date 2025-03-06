using System;
using UnityEngine;

namespace UF.System_Train {
    public static class BulletDomain {
        public static BulletEntity Spawn(TrainSystemContext ctx, int typeID, Vector3 pos, RoleEntity role) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsModule, ctx.templateMoudle, ctx.idService, typeID, pos, Vector3.zero);

            if (role.t_roleType == T_RoleType.Player1) {
                entity.moveType = BulletMoveType.Shaped;
            } else if (role.t_roleType == T_RoleType.Player2) {
                entity.moveType = BulletMoveType.ToLeft;
            }

            ctx.bulletRepository.Add(entity);
            return null; // 之后如果需要在其他地方调用子弹就把return改掉
        }

        public static BulletEntity SpawnByRolePos(TrainSystemContext ctx, RoleEntity role) {
            var pos = role.GetMuzzlePos();
            role.skillorSlotComponent.TakeAll(out var skills);
            var skill = skills[0];
            int typeID = skill.typeID;

            var bullet = Spawn(ctx, typeID, pos, role);
            return bullet;
        }

        public static void UnSpawn(TrainSystemContext ctx, BulletEntity bullet) {
            ctx.bulletRepository.Remove(bullet);
            bullet.TearDown();
        }

        public static void Clear(TrainSystemContext ctx) {
            int len = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);
            for (int i = 0; i < len; i += 1) {
                BulletEntity bullet = bullets[i];
                bullet.TearDown();
            }
            ctx.bulletRepository.Clear();
        }

        public static void FSM_All_Execute(BulletEntity bullet) {
            if (bullet.moveType == BulletMoveType.Shaped) {
                Loco_Shaped(bullet);
            } else if (bullet.moveType == BulletMoveType.ToLeft) {
                Loco_ToLeft(bullet);
            }
        }

        public static void Loco_Shaped(BulletEntity bullet) {
            bullet.Loco_Shaped();
        }

        public static void Loco_ToLeft(BulletEntity bullet) {
            bullet.Loco_ToLeft();
        }

        public static void CheckUnSpawn(TrainSystemContext ctx, BulletEntity bullet) {
            Vector3 pos = bullet.TF_GetPos();
            if (pos.x < -21 || pos.x > 21) {
                UnSpawn(ctx, bullet);
            }

            DetectionMinionByPos(ctx, bullet);
        }

        static void DetectionMinionByPos(TrainSystemContext ctx, BulletEntity bullet) {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];

                bool check1 = role.t_roleType == T_RoleType.Player1 && bullet.moveType == BulletMoveType.Shaped;
                bool check2 = role.t_roleType == T_RoleType.Player2 && bullet.moveType == BulletMoveType.ToLeft;
                if (check1 || check2) {
                    continue;
                }

                Vector3 pos = bullet.TF_GetPos();
                float dis = Vector3.Distance(role.TF_GetPos(), pos);

                if (dis < 1.0f) {
                    RoleDomain.HpReduce_MinionFall(ctx, role, bullet.damage);

                    UnSpawn(ctx, bullet);
                }
            }
        }
    }
}