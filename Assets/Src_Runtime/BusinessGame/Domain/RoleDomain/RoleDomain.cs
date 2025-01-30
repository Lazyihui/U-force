using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public static partial class RoleDomain {

        #region LifeCycle

        public static RoleEntity Spawn(GameContext ctx, int typeID) {
            RoleEntity role = GameFactory.Role_Create(ctx.assetsCore, ctx.templateCore, ctx.idService, typeID);
            ctx.roleRepository.Add(role);
            return role;
        }

        public static RoleEntity SpawnByPos(GameContext ctx, Vector2 pos, int typeID) {
            RoleEntity entity = Spawn(ctx, typeID);
            entity.TF_SetPos(pos);
            return entity;
        }

        public static RoleEntity SpawnBySpawner(GameContext ctx, RoleSpawnerTM spawnerTM) {
            RoleEntity role = GameFactory.Role_CreateBySpawner(ctx.assetsCore, ctx.templateCore, ctx.idService, spawnerTM);
            ctx.roleRepository.Add(role);
            return role;
        }

        #endregion

        #region Act 行为
        public static void Skillor_Frame_Acting(GameContext ctx, RoleEntity role, RoleCastingStateModel stateModel, SkillFrameModel frame, float dt) {
            var skillor = stateModel.skillor;

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

        public static void Behavior_Act(GameContext ctx, RoleEntity role, RoleBehaviorModel model, float dt) {

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
                Debug.LogWarning("RoleDomain.SpawnBullet");
            }

            if (model.hasBulletTearDown) {
                // TearDown
                Debug.LogWarning("RoleDomain.TearDown");
            }


        }

        #endregion
        #region Input   

        public static void Input_Record(GameContext ctx, RoleEntity role, float dt) {
            Debug.Assert(role != null, "RoleDomain.Input_Record role == null");
            Debug.Assert(ctx != null, "RoleDomain.Input_Record ctx == null");
            Debug.Assert(role.InputComponent != null, "RoleDomain.Input_Record role.InputComponent == null");
            var inputCom = role.InputComponent;
            var inputModule = ctx.inputModule;

            inputCom.Jump_Set(inputModule.IsJump);
            if (inputModule.InputCastKey != InputKeyFlag.None) {
                Debug.LogWarning("inputModule.InputKey:" + inputModule.InputCastKey);
            }
            inputCom.RecoredCastKey(inputModule.InputCastKey);
        }

        #endregion


        #region Skill Cast

        public static void Skillor_Input_FlushKeys(GameContext ctx, RoleEntity role, float dt) {

            SkillModel skill = role.Skill_GetByInput();
            if (skill != null) {
                bool succ = Skillor_Cast(ctx, role, skill, dt);
                if (succ) {
                    Debug.Log("Skillor_Input_FlushKeys succ");
                } else {
                    Debug.Log("Skillor_Input_FlushKeys fail");
                }
            }
        }


        public static bool Skillor_Cast(GameContext ctx, RoleEntity role, SkillModel skill, float dt) {

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

        static bool Skillor_AllowCast(GameContext ctx, RoleEntity role, SkillModel skillor, float dt) {
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
                // TODO:技能冷却提示
                Debug.Log("Skillor_AllowCast skillor.cd > 0要做提示+" + skillor.cd);
                FSM_Casting_Exit(ctx, role, dt);
                return false;
            }

            return true;
        }

        #endregion
    }
}