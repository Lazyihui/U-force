using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public static class RoleDomain {
        public static RoleEntity Spawn(GameContext ctx, int typeID) {
            RoleEntity role = GameFactory.Role_CreateBySpawner(ctx.assetsCore, ctx.templateCore, ctx.idService, Vector3.zero, typeID);
            ctx.roleRepository.Add(role);
            return role;
        }

        #region Jump
        public static void Jump(GameContext ctx, RoleEntity role) {
            role.Jump();
        }

        #endregion

        #region  FSM
        public static void FSM_Normal_Execute(GameContext ctx, RoleEntity role, float dt) {
            // Skill Cast
            Skillor_Input_FlushKeys(ctx, role, dt);
        }

        public static void FSM_Casting_Execute(GameContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            FSM_Casting_Hand_Execute(ctx, role, fsm.CastingStateModel, dt);
        }

        public static void FSM_Casting_Hand_Execute(GameContext ctx, RoleEntity role, RoleCastingStateModel stateModel, float dt) {
            var skillor = stateModel.skillor;

            if (skillor == null) {
                Debug.Log("FSM_Casting_Hand_Execute skillor == null");
                return;
            }

            if (stateModel.isEntering) {
                stateModel.isEntering = false;
            }

            var frames = skillor.frames;
            ref int actionIndex = ref stateModel.frameIndex;

            if (actionIndex >= frames.Length) {
                FSM_Casting_Exit(ctx, role, dt);
                return;
            }

            Skillor_Input_FlushKeys(ctx, role, dt);

            var action = frames[stateModel.frameIndex];
            // TODO:这里要写Loop Action 的逻辑 逻辑帧LoopModel


            // if(''){
            // Skillor_Act_Acting
            Skillor_Frame_Acting(ctx, role, stateModel, action, dt);
            // }

            // if(..){
            //     actionIndex =+ 1;
            // }
        }

        static void FSM_Casting_Exit(GameContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            FSM_Normal_Enter(ctx, role, dt);
        }

        static void FSM_Normal_Enter(GameContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            fsm.Normal_Enter();
        }
        #endregion

        #region Act
        public static void Skillor_Frame_Acting(GameContext ctx, RoleEntity role, RoleCastingStateModel stateModel, SkillFrameModel frame, float dt) {
            var skillor = stateModel.skillor;
            if (skillor == null) {
                return;
            }

            // -Role Behavior
            Debug.Assert(frame.hasRoleBehaviour, "frame.hasRoleBehaviour");

            if (frame.hasRoleBehaviour) {
                Behavior_Act(ctx, role, frame.roleBehaviour, dt);
            }
        }

        public static void Behavior_Act(GameContext ctx, RoleEntity role, RoleBehaviorModel model, float dt) {

            if (model.isMoveTo) {
                // MoveTo
            }

            if (model.isJump) {
                RoleDomain.Jump(ctx, role);
            }

            if (model.isRestoreSkillTimes) {
                // RestoreSkillTimes
            }

            if (model.isSpawnBullet) {
                // SpawnBullet
                BulletDomain.Spawn(ctx, 100);
            }

            if (model.hasBulletTearDown) {
                // TearDown
            }


        }

        #endregion
        #region Input   

        public static void Input_Record(GameContext ctx, RoleEntity role, float dt) {
            var inputCom = role.InputComponent;
            var inputModule = ctx.inputModule;

            inputCom.Jump_Set(inputModule.IsJump);
            inputCom.RecoredCastKey(inputModule.InputKey);
        }

        #endregion


        #region Skill Cast

        public static void Skillor_Input_FlushKeys(GameContext ctx, RoleEntity role, float dt) {

            SkillModel skill = role.Skill_GetByInput();
            if (skill != null) {
                Debug.Log("Skillor_Cast");
                bool succ = Skillor_Cast(ctx, role, skill, dt);
                if (succ) {
                    Debug.Log("Skillor_Cast succ");
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
            if (!Skillor_AllowCast(ctx, role, skill)) {
                Debug.Log("Skillor_Cast !Skillor_AllowCast");
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

        static bool Skillor_AllowCast(GameContext ctx, RoleEntity role, SkillModel skillor) {
            // 完成
            if (skillor == null) {
                Debug.Log("Skillor_AllowCast skillor == null");
                return false;
            }

            if (skillor.restUseTimes <= 0) {
                Debug.Log("Skillor_AllowCast skillor.restUseTimes <= 0");
                return false;
            }

            // if (skillor.cd > 0) {
            //     // TODO:技能冷却提示
            //     Debug.Log("Skillor_AllowCast skillor.cd > 0");
            //     return false;
            // }

            return true;
        }
        static void FSM_Casting_Enter(GameContext ctx, RoleEntity role, SkillModel skillor, float dt) {
            // 完成
            var fsm = role.FSMComponent;
            fsm.Casting_Enter(skillor);
        }
        #endregion
    }
}