using System;
using UnityEngine;

namespace UF.System_Train {
    public static class PropDomain {
        public static PropEntity Spawn(TrainSystemContext ctx, int typeID, Vector3 pos) {
            PropEntity entity = GameFactory.Prop_Create(ctx.assetsModule, ctx.templateMoudle, ctx.idService, typeID, pos);
            ctx.propRepository.Add(entity);
            return entity;
        }

        public static PropEntity SpawnByRole(TrainSystemContext ctx, RoleEntity role) {
            var pos = role.GetMuzzlePos();
            if (role.t_roleType == T_RoleType.Player1) {
                pos.x += 1.5f;
            } else if (role.t_roleType == T_RoleType.Player2) {
                // 不对劲, pos.x为什么会是16
                pos.x -= 2.5f;
            }

            var prop = Spawn(ctx, 100, pos); // typeID暂用防护罩的100, 虽然没什么必要，但是后面要改
            return prop;
        }

        public static void UnSpawn(TrainSystemContext ctx, PropEntity prop) {
            ctx.propRepository.Remove(prop);
            prop.TearDown();
        }

        public static void Clear(TrainSystemContext ctx) {
            int len = ctx.propRepository.TakeAll(out PropEntity[] props);
            for (int i = 0; i < len; i += 1) {
                PropEntity prop = props[i];
                prop.TearDown();
            }
            ctx.propRepository.Clear();
        }

        public static void FSM_All_Execute(PropEntity prop) {
            if (prop.moveType == PropMoveType.Down) {
                Loco_MoveDown(prop);
            }
        }

        static void Loco_MoveDown(PropEntity prop) {
            prop.Move_Down();
        }

        public static void CheckUnSpawn(TrainSystemContext ctx, PropEntity prop) {
            Vector3 pos = prop.TF_GetPos();
            if (pos.y < -7) {
                UnSpawn(ctx, prop);
            } else if (pos.x < -21 || pos.x > 21) {
                UnSpawn(ctx, prop);
            }
            DetectionBulletByPos(ctx, prop);
        }

        static void DetectionBulletByPos(TrainSystemContext ctx, PropEntity prop) {
            int len = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);
            for (int i = 0; i < len; i += 1) {
                BulletEntity bullet = bullets[i];

                bool check1 = prop.type == PropType.Armor1 && bullet.type == BulletMoveType.Shaped;
                bool check2 = prop.type == PropType.Armor2 && bullet.type == BulletMoveType.ToLeft;
                if (check1 || check2) {
                    continue;
                }

                Vector3 pos = prop.TF_GetPos();
                float dis = Vector3.Distance(bullet.TF_GetPos(), pos);

                if (dis < 1.0f) {
                    BulletDomain.UnSpawn(ctx, bullet);
                    prop.times -= 1;

                    if (prop.times <= 0) {
                        UnSpawn(ctx, prop);
                    }
                }
            }
        }
    }
}