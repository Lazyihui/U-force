using System;
using System.Collections;
using UnityEngine;

namespace UF {
    // 移动的所有逻辑
    public static partial class BulletDomain {

        public static void Bullet_Loco(BulletEntity bullet, float dt) {

            if (bullet.moveType == BulletMoveType.None) {
                // Bullet_Loco_Stop(bullet, dt);
            } else if (bullet.moveType == BulletMoveType.Shaped) {
                // Bullet_Loco_Shaped(bullet, dt);
            } else if (bullet.moveType == BulletMoveType.ToLeft) {
                // Bullet_Loco_ToLeft(bullet, dt);
            } else if (bullet.moveType == BulletMoveType.Track) {
                // Bullet_Loco_Track(bullet, dt);
            }

        }

        public static void Bullet_Loco_Stop(BulletEntity bullet, float dt) {
            // 1.停止移动
        }

        public static void Bullet_Loco_Shaped(BulletEntity bullet, float dt) {
            // 1.直线运动
            // 2.碰到边界消失
        }

        public static void Bullet_Loco_ToLeft(BulletEntity bullet, float dt) {
            // 1.向左飞
            // 2.碰到边界消失
        }

        public static void Bullet_Loco_Track(BulletEntity bullet, float dt) {
            // 1.跟踪
            // 2.碰到边界消失
        }

    }
}