using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public static partial class RoleDomain {

        #region LifeCycle

        public static RoleEntity SpawnByTypeID(GameSystemContext ctx, int typeID) {
            RoleEntity role = GameFactory.Role_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID);
            ctx.roleRepository.Add(role);
            return role;
        }

        public static RoleEntity SpawnByPos(GameSystemContext ctx, int typeID, Vector2 pos) {
            RoleEntity entity = SpawnByTypeID(ctx, typeID);
            entity.TF_SetPos(pos);
            return entity;
        }

        public static RoleEntity SpawnBySkillorType(GameSystemContext ctx, Vector2 pos, int typeID, int[] skillorTypeIDArray) {
            RoleEntity role = GameFactory.Role_CreateByTypeSkillType(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID, skillorTypeIDArray);
            role.TF_SetPos(pos);
            ctx.roleRepository.Add(role);
            return role;
        }

        public static RoleEntity SpawnBySpawner(GameSystemContext ctx, RoleSpawnerTM spawnerTM) {
            RoleEntity role = GameFactory.Role_CreateBySpawner(ctx.assetsModule, ctx.templateCore, ctx.idService, spawnerTM);
            ctx.roleRepository.Add(role);
            return role;
        }

        public static void UnSpawn(GameSystemContext ctx, RoleEntity role) {
            ctx.roleRepository.Remove(role);
            role.TearDown();
        }

        public static void Clear(GameSystemContext ctx) {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];
                role.TearDown();
            }
            ctx.roleRepository.Clear();
        }

        #endregion

        #region Act 行为
        public static void Skillor_Frame_Acting(GameSystemContext ctx, RoleEntity role, RoleCastingStateModel stateModel, SkillFrameModel frame, float dt) {
            var skillor = stateModel.skillor;

            // if (frame.loopModel.isDone) {
            //     return;
            // }

            // frame.loopModel.CheckDone();

            if (skillor == null) {
                Debug.Log("Skillor_Frame_Acting skillor == null");
                return;
            }

            if (frame.loopModel.isDone) {
                Debug.Log("Skillor_Frame_Acting frame.loopModel.isDone");
                return;
            }

            // TODO:问J 不懂用法
            // frame.loopModel.CheckDone();

            // -Role Behavior
            if (frame.hasRoleBehaviour) {
                Behavior_Act(ctx, role, frame.roleBehaviour, dt);
            }
        }

        public static void Behavior_Act(GameSystemContext ctx, RoleEntity role, RoleBehaviorModel model, float dt) {

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
                BulletDomain.SpawnByRole_Spawner(ctx, role, spawnTM);
                // 记录一共发射了多少次
                ctx.gameEntity.playerSaveData.shootAllTimes += 1;
            }

            if (model.hasBulletTearDown) {
                // TearDown
                Debug.LogWarning("RoleDomain.TearDown");
            }


        }
        #endregion

        #region Input   

        public static void Input_Record(GameSystemContext ctx, RoleEntity role, float dt) {
            var game = ctx.gameEntity;
            var inputCom = role.InputComponent;
            var inputModule = ctx.inputModule;

            inputCom.Jump_Set(inputModule.IsJump);
            if (game.isEliminateWar) {
                inputCom.Grap_Set(inputModule.IsGrap);
            }
            inputCom.RecoredCastKey(inputModule.InputCastKey);
            inputCom.MoveDir_Set(inputModule.MoveDir);
        }

        #endregion

        #region Skill Cast

        public static void Skillor_Input_FlushKeys(GameSystemContext ctx, RoleEntity role, float dt) {

            SkillModel skill = role.Skill_GetByInput();
            if (skill != null) {
                bool succ = Skillor_Cast(ctx, role, skill, dt);
            }
        }

        public static bool Skillor_Cast(GameSystemContext ctx, RoleEntity role, SkillModel skill, float dt) {

            // No Combo, No Input
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

        #region Player physics

        public static void DetectionMinionByPos(GameSystemContext ctx, RoleEntity role, Vector2 pos, int damage) {
            // 检测周围的minion 碰到扣血
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] minions);

            for (int i = 0; i < len; i++) {

                var minion = minions[i];
                if (minion == role) {
                    continue;
                }

                float dis = Vector2.Distance(minion.TF_GetPos(), pos);
                // TODO:可能要改成用 collider   
                float radius = 2f;

                if (dis < radius) {
                    // player扣血 或者失败  
                    HpReduce_Role(ctx, role, damage);

                    // minion进基地死掉
                    HpReduce_Role(ctx, minion, damage);
                }
            }
        }
        public static void DetectionEnemyAndEnemyByPos(GameSystemContext ctx, RoleEntity Enemy, Vector2 pos, int damage) {
            // 检测周围的minion 碰到扣血
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] touchMinions);

            for (int i = 0; i < len; i++) {

                var Touchminion = touchMinions[i];
                if (Touchminion == Enemy) {
                    continue;
                }

                float dis = Vector2.Distance(Touchminion.TF_GetPos(), pos);
                // TODO:可能要改成用 collider   
                float radius = 2f;

                if (dis < radius) {

                    HpReduce_MinionFall(ctx, Touchminion, damage);
                }
            }
        }

        public static void HpReduce_MinionFall(GameSystemContext ctx, RoleEntity role, int damage) {

            var attCom = role.AttributeComponent;
            int finalHp = attCom.hp - damage;
            role.Attr_Hp_Set(finalHp);

            var playerData = ctx.gameEntity.playerSaveData;
            playerData.SetHitTimes(playerData.hitTimes + 1);

            if (attCom.hp <= 0) {
                role.SetRoleState(RoleState.Fall);
                Debug.Log("进入掉落状态");
                role.Fall();
                role.Set_RbConstraints();
            }
        }

        public static void HpReduce_Role(GameSystemContext ctx, RoleEntity role, int damage) {
            var playerData = ctx.gameEntity.playerSaveData;
            if (role.roleType == RoleType.Player) {
                // 受到的伤害
                playerData.SetOwnerGetHurt(playerData.GetRoleGetHurt() + damage);
            }
            var attCom = role.AttributeComponent;
            int finalHp = attCom.hp - damage;
            role.Attr_Hp_Set(finalHp);

            if (attCom.hp <= 0) {
                role.SetRoleState(RoleState.Dead);
            }
        }
        
        // 下落过程的检测 检测到地面状态变为死亡 和检测是否碰到敌人
        public static void FallingtodeathCheck(GameSystemContext ctx, RoleEntity role) {
            if (role.state != RoleState.Fall) {
                return;
            }

            // TODO:感觉是这里的问题
            DetectionEnemyAndEnemyByPos(ctx, role, role.GetPos(), role.AttributeComponent.GetFallDamage());

            // 和地面的交叉检测
            RaycastHit2D[] hits = Physics2D.RaycastAll(role.GetPos(), Vector2.down, 1.1f);
            Debug.DrawRay(role.GetPos(), Vector2.down * 0.6f, Color.gray, 0.1f);

            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];
                if (hit.collider.gameObject.CompareTag("Ground")) {
                    Debug.Log("死亡动画");
                    role.SetRoleState(RoleState.Dead);

                }
            }
        }



        // 否是死亡状态 
        public static void IsDeadState(GameSystemContext ctx, RoleEntity role) {
            var game = ctx.gameEntity;

            if (role.GetPos().x < -21) {
                role.SetRoleState(RoleState.Dead);
            }

            if (role.state != RoleState.Dead) {
                return;
            }

            UnSpawn(ctx, role);

            if (role.roleType == RoleType.Player) {
                // TODO:L写这里其实不太好
                ctx.gameEntity.state = GameState.GameOver;
                var playerData = game.playerSaveData;
                ctx.appUI.Panel_GameOver_Open(false, playerData.GetRoleGetHurt(), playerData.shootAllTimes, playerData.hitTimes, playerData.GetRoleHitRate(), 0);
            }
        }

        #endregion
    }
}