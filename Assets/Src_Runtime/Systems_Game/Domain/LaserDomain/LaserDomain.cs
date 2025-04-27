using System;
using UnityEngine;

namespace UF {
    public static class LaserDomain {
        public static LaserEntity Spawn(GameSystemContext ctx, int typeID, Vector2 startPos, Vector2 endPos) {
            LaserEntity entity = GameFactory.Laser_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID, startPos, endPos);
            entity.TF_SetPos(startPos, endPos);
            
            ctx.laserRepository.Add(entity);
            return entity;
        }

        public static void UnSpawn(GameSystemContext ctx, LaserEntity laser) {
            ctx.laserRepository.Remove(laser);
            laser.TearDown();
        }

        public static void Clear(GameSystemContext ctx) {
            int len = ctx.laserRepository.TakeAll(out LaserEntity[] lasers);
            for (int i = 0; i < len; i += 1) {
                LaserEntity laser = lasers[i];
                laser.TearDown();
            }
            ctx.laserRepository.Clear();
        }

        public static void FSM_All_Execute(GameSystemContext ctx, LaserEntity laser, float dt) {
            Laser_Loco(ctx, laser, dt);
        }

        // 先命名成loco, 感觉不合适
        public static void Laser_Loco(GameSystemContext ctx, LaserEntity laser, float dt) {
            if (laser.state == LaserState.LaserWarning) {
                laser.showTimer += dt;
            }

            if (laser.state == LaserState.Laser) {
                laser.HideTimer += dt;
            }

            if (laser.showTimer >= laser.showTime) {
                laser.state = LaserState.Laser;
                laser.ShowLaser();
                laser.showTimer = 0;
            }

            if (laser.HideTimer >= laser.HideTime) {
                UnSpawn(ctx, laser);
            }
        }

        // 检测是否击中玩家
        public static void DetectionEnemyAttackOwner(LaserEntity laser) {
            if (laser.state != LaserState.Laser) {
                return;
            }

            if (laser.isHit) {
                return;
            }

            var dir = laser.endPos - laser.startPos;
            Ray ray = new Ray(laser.startPos, dir);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 50);
        }
    }
}