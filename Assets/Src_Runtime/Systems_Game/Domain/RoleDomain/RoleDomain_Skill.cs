using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public static partial class RoleDomain {


        #region Skill Cast

        public static void Skillor_Input_FlushKeys(GameSystemContext ctx, RoleEntity role, float dt) {
            SkillModel skill = role.Skill_GetByInput();
            if (skill != null) {
                bool succ = Skillor_Cast(ctx, role, skill, dt);
            }
        }

        public static bool Skillor_Cast(GameSystemContext ctx, RoleEntity role, SkillModel skill, float dt) {
            if (skill == null) {
                Debug.Log("Skillor_Cast skill == null");
                return false;
            }

            SkillFrameModel frame = role.Skillor_CurrentFrame();
            if (frame == null) {
                Debug.Log("Skillor_Cast frame == null");
                return false;
            }

            // 最后决定的技能是否可施放
            if (!Skillor_AllowCast(ctx, role, skill, dt)) {
                return false;
            }

            // Cost
            skill.restUseTimes -= 1;
            skill.cd = skill.cdMax;

            //TODO:如果有销毁蓝写在这里

            // TODO:Buff写在这里

            // Remove Maintain Buff
            FSM_Casting_Enter(ctx, role, skill, dt);
            return true;

        }

        static bool Skillor_AllowCast(GameSystemContext ctx, RoleEntity role, SkillModel skillor, float dt) {
            // 完成
            if (skillor == null) {
                Debug.Log("Skillor_AllowCast skillor == null");
                FSM_Casting_Exit(ctx, role, dt);
                return false;
            }

            if (skillor.restUseTimes <= 0) {
                Debug.Log("Skillor_AllowCast skillor.restUseTimes <= 0");
                FSM_Casting_Exit(ctx, role, dt);
                return false;
            }

            if (skillor.cd > 0) {
                FSM_Casting_Exit(ctx, role, dt);
                return false;
            }
            return true;
        }

        #endregion

        #region Act 行为
        public static void Skillor_Act_Acting(GameSystemContext ctx, RoleEntity role, RoleCastingStateModel stateModel, SkillFrameModel frame, float dt) {
            var skillor = stateModel.skillor;

            if (frame.loopModel.isDone) {
                Debug.Log("Skillor_Act_Acting frame.loopModel.isDone");
                return;
            }

            frame.loopModel.CheckDone();

            if (skillor == null) {
                Debug.Log("Skillor_Frame_Acting skillor == null");
                return;
            }

            // -Role Behavior
            if (frame.hasRoleBehaviour) {
                RoleBehavior_Act(ctx, role, frame.roleBehaviour, dt);
            }

            // -world Behavior
            if (frame.hasWorldBehaviour) {
                WorldBehavior_Act(ctx, role, frame.worldBehaviourModel, dt);
            }

        }
        public static void RoleBehavior_Act(GameSystemContext ctx, RoleEntity role, RoleBehaviorModel model, float dt) {

            if (model.isMoveTo) {
                // MoveTo
                Debug.LogWarning("RoleDomain.MoveTo");
            }

            if (model.isJump) {
                // RoleDomain.Jump(ctx, role);
                Debug.LogWarning("RoleDomin.Jump");
            }

            if (model.isRestoreSkillTimes) {
                // RestoreSkillTimes
                Debug.LogWarning("RoleDomain.RestoreSkillTimes");
            }

            if (model.isSpawnBullet) {
                // SpawnBullet
                var spawnTM = model.bulletSpawnerTM;
                spawnTM.spawnPos.position = role.GetMuzzlePos();
                if (spawnTM.bulletSo.tm.isDestotyBottomALine) {
                    // TODO:
                    Debug.LogWarning("RoleDomain.MakeBulletAppear");
                    spawnTM.spawnPos.position = GetNearestEnemyPosition(ctx, role);
                }
                BulletEntity bullet = BulletDomain.SpawnByRole_Spawner(ctx, role, spawnTM);
                // 记录一共发射了多少次
                ctx.gameEntity.playerSaveData.shootTimes += 1;
                ctx.gameEntity.playerSaveData.eliShootTimes += 1;
            }

            if (model.hasBulletTearDown) {
                // TearDown
                Debug.LogWarning("RoleDomain.TearDown");
            }

        }

        public static void WorldBehavior_Act(GameSystemContext ctx, RoleEntity role, WorldBehaviourModel model, float dt) {

            if (model.isSpawnStuff) {
                Debug.LogWarning("RoleDomain.SpawnStuff生成4物品");
                // SpawnStuff
                for (int i = 0; i < 4; i++) {
                    // 得到随机的坐标
                    Vector3 pos = new Vector3(UnityEngine.Random.Range(-7, 6), UnityEngine.Random.Range(12, -4), 0);
                    StuffEntity stuff = StuffDomain.SpawnByPos(ctx, model.stuffSo.tm.typeID, pos);
                }
            }

            if (model.isSpawnBullet) {
                Debug.LogWarning("RoleDomain.SpawnBullet生成子弹");
                // SpawnBullet
                if (model.bulletSo.tm.isFixedBoomPoint) {
                    for (int i = 0; i < 4; i++) {
                        // 得到随机的坐标
                        Vector3 pos = new Vector3(UnityEngine.Random.Range(-7, 6), UnityEngine.Random.Range(12, -4), 0);
                        BulletEntity bullet = BulletDomain.SpawnByPos(ctx, model.bulletSo.tm.typeID, pos, new Vector3(0, 0, 0));
                    }
                }
            }

            if (model.isHaveCentred) {
                Debug.LogWarning("RoleDomain.Bullet_Centred集中位置");
                Bullet_Centred(ctx);
            }
        }
        #endregion
        #region  查找坐标Y轴最近的敌人坐标
        public static Vector3 GetNearestEnemyPosition(GameSystemContext ctx, RoleEntity player) {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            RoleEntity nearestEnemy = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < len; i++) {
                RoleEntity role = roles[i];
                if (role.roleType == RoleType.Player) {
                    continue;
                }
                float distance = -10 + role.TF_GetPos().y;
                if (distance < minDistance) {
                    minDistance = distance;
                    nearestEnemy = role;
                }
            }

            if (nearestEnemy == null) {
                return player.TF_GetPos();
            }
            var pos = new Vector3(player.TF_GetPos().x, nearestEnemy.TF_GetPos().y, 0);
            return pos;
        }
        #endregion

        #region 击中位置(大招版汇聚)
        static void Bullet_Centred(GameSystemContext ctx) {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity minion = roles[i];
                if (minion.roleType == RoleType.Player) {
                    continue;
                }
                var minionPos = minion.TF_GetPos();

                int random = UnityEngine.Random.Range(1, 4);
                int y = 0;
                switch (random) {
                    case 1:
                        y = 4;
                        break;
                    case 2:
                        y = 2;
                        break;
                    case 3:
                        y = 0;
                        break;
                }
                float minionY = minionPos.y;

                if (minionY == y) { continue; }

                Vector2 dir = Vector2.zero;
                dir.y = y - minionY > 0 ? 1 : -1;
                float length = Math.Abs(y - minionY);

                minion.changeLineDir = dir;
                minion.changeLineLength = length;
                minion.isChangeLine = true;
            }
        }
        #endregion
    }
}