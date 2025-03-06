using System;
using System.Collections;
using UnityEngine;

namespace UF {
    // 移动的所有逻辑
    public static partial class BulletDomain {

        public static void Bullet_Loco(GameSystemContext ctx, BulletEntity bullet, float dt) {

            if (bullet.moveType == BulletMoveType.None) {
                Bullet_Loco_Stop(bullet, dt);
            } else if (bullet.moveType == BulletMoveType.Shaped) {
                Bullet_Loco_Shaped(bullet, dt);
            } else if (bullet.moveType == BulletMoveType.ToLeft) {
                Bullet_Loco_ToLeft(bullet, dt);
            } else if (bullet.moveType == BulletMoveType.Track) {
                Bullet_Loco_Track(ctx, bullet, dt);
            }

        }

        public static void Bullet_Loco_Stop(BulletEntity bullet, float dt) {
            bullet.Move_Stop();
        }

        public static void Bullet_Loco_Shaped(BulletEntity bullet, float dt) {
            bullet.Loco_Shaped();
        }

        public static void Bullet_Loco_ToLeft(BulletEntity bullet, float dt) {
            bullet.Loco_ToLeft();
        }

        public static void Bullet_Loco_Track(GameSystemContext ctx, BulletEntity bullet, float dt) {
            // 找到最近的敌人
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            RoleEntity target = null;
            float minDis = float.MaxValue;
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];
                if (role.roleType == RoleType.Player) continue;
                float dis = Vector3.Distance(bullet.TF_GetPos(), role.TF_GetPos());
                if (dis < minDis) {
                    minDis = dis;
                    target = role;
                }
            }
            var dir = target.TF_GetPos() - bullet.TF_GetPos();
            bullet.Loco_MoveDir(dir);
            
        }

    }
}