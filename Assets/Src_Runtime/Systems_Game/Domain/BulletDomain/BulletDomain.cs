using System;
using UnityEngine;

namespace UF {
    public static class BulletDomain {

        #region Lifecycle
        public static BulletEntity Spawn(GameSystemContext ctx, int typeID, Vector3 pos, Vector3 dir) {
            BulletEntity entity = GameFactory.Bullet_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID, pos, dir);

            ctx.bulletRepository.Add(entity);
            return null;
        }

        public static BulletEntity SpawnByRole_Spawner(GameSystemContext ctx, RoleEntity role, BulletSpawnerTM spawnerTM) {
            // TODO:生成位置
            var BulletEntity = BulletDomain.Spawn(ctx, spawnerTM.bulletSo.tm.typeID, spawnerTM.spawnPos.position, spawnerTM.spawnDir.position);
            return BulletEntity;
        }

        public static void UnSpawn(GameSystemContext ctx, BulletEntity bullet) {
            ctx.bulletRepository.Remove(bullet);
            bullet.TearDown();
        }

        public static void UnSpawn(GameSystemContext ctx, BulletEntity bullet, RoleEntity role) {
            UnSpawn_Skill(ctx, bullet, role);
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

        #endregion

        #region FSM
        public static void FSM_All_Execute(GameSystemContext ctx, BulletEntity bullet, float dt) {
            if (bullet.type == BulletMoveType.Shaped) {
                Loco_Shaped(ctx, bullet);
            }
        }
        #endregion

        #region  Loco 行为
        public static void Loco_Shaped(GameSystemContext ctx, BulletEntity bullet) {
            bullet.Loco_Shaped();
        }
        #endregion

        #region detection检测
        public static void CheckUnSpawn(GameSystemContext ctx, BulletEntity bullet) {
            Vector3 pos = bullet.TF_GetPos();
            if (pos.x < -21 || pos.x > 21) {
                UnSpawn(ctx, bullet);
            }

            detectionMinionByPos(ctx, bullet, pos);
        }

        static void detectionMinionByPos(GameSystemContext ctx, BulletEntity bullet, Vector3 pos) {

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] minions);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = minions[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }

                float dis = Vector3.Distance(minion.TF_GetPos(), pos);

                if (dis < 1.0f) {
                    RoleDomain.HpReduce_MinionFall(ctx, minion, bullet.damage);
                    bullet.hitTimes -= 1;

                    if (bullet.hitTimes <= 0) {
                        UnSpawn(ctx, bullet, minion);
                    }
                }
            }
        }

        #endregion

        #region 技能(子弹效果)
        static void UnSpawn_Skill(GameSystemContext ctx, BulletEntity bullet, RoleEntity role) {
            switch (bullet.skillType) {
                case BulletSkillType.Boom:
                    Skill_Boom(ctx, bullet);
                    break;

                case BulletSkillType.Slow:
                    Skill_Slow(ctx, bullet, role);
                    break;

                case BulletSkillType.Stop:
                    Skill_Stop(ctx, bullet);
                    break;

                case BulletSkillType.Down:
                    Skill_Down(ctx, bullet, role);
                    break;

                case BulletSkillType.Appear:
                    Skill_Appear(ctx, bullet);
                    break;

                case BulletSkillType.Back:
                    Skill_Back(ctx, bullet, role);
                    break;

                case BulletSkillType.Elec:
                    Skill_Elec(ctx, bullet);
                    break;

                case BulletSkillType.Converge:
                    Skill_Converge(ctx, bullet);
                    break;
                    
                default:
                    break;
            }
        }

        static void Skill_Boom(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("爆炸");
        }

        static void Skill_Slow(GameSystemContext ctx, BulletEntity bullet, RoleEntity role) {
            var attaCom = role.AttributeComponent;
            float moveSpeed = attaCom.GetMoveSpeed();
            attaCom.SetMoveSpeed(moveSpeed / 2);
        }

        static void Skill_Stop(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("停顿 + 持续伤害");
        }

        static void Skill_Down(GameSystemContext ctx, BulletEntity bullet, RoleEntity role) {
            role.isControlledDown = true;
        }

        static void Skill_Appear(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("现形");
        }

        static void Skill_Back(GameSystemContext ctx, BulletEntity bullet, RoleEntity role) {
            role.isControlledRight = true;
        }

        static void Skill_Elec(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("持续伤害");
        }

        static void Skill_Converge(GameSystemContext ctx, BulletEntity bullet) {
            Debug.Log("汇聚");
        }
        #endregion
    }
}