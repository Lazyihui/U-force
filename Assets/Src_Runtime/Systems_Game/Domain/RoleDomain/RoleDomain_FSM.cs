using System;
using System.Collections;
using UnityEngine;


namespace UF {
    public static partial class RoleDomain {

        #region  防守战
        public static void FSM_Normal_Execute(GameSystemContext ctx, RoleEntity role, float dt) {
            // Skill Cast
            Skillor_Input_FlushKeys(ctx, role, dt);
        }

        public static void FSM_Casting_Execute(GameSystemContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            FSM_Casting_Hand_Execute(ctx, role, fsm.CastingStateModel, dt);
        }

        public static void FSM_All_Execute(GameSystemContext ctx, RoleEntity role, float dt) {
            var game = ctx.gameEntity;

            // 任何状态都运行的逻辑
            LocoDefendAndOffensive_Execute(ctx, role, dt);

            // skill
            int skillLen = role.skillorSlotComponent.TakeAll(out var skills);
            for (int i = 0; i < skillLen; i += 1) {
                var skill = skills[i];
                skill.cd -= dt;
                if (skill.cd < 0) {
                    skill.cd = 0;
                }

                ctx.appUI.Panel_GameDefend_Update(skill, skill.cd, skill.cdMax);
            }

            // 和地面的检测
            RoleDomain.FallingtodeathCheck(ctx, role);
            RoleDomain.IsDeadState(ctx, role);

            var feaCom = role.FeatureComponent;
            if (feaCom.featureType == FeatureType.DisAppear) {
                // 1.怪物隐身
                role.Set_SpriteAppear();
                // 2.怪物不可被普通子弹攻击到

            }

        }

        public static void FSM_All_Pause(RoleEntity role) {
            Loco_MoveStop(role);
            //TODO: 施法过程中暂停
        }

        static void FSM_Casting_Enter(GameSystemContext ctx, RoleEntity role, SkillModel skillor, float dt) {
            // 完成
            var fsm = role.FSMComponent;
            fsm.Casting_Enter(skillor);
        }

        static void FSM_Normal_Enter(GameSystemContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            fsm.Normal_Enter();
        }

        static void FSM_Casting_Exit(GameSystemContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            var inputCom = role.InputComponent;
            var input = ctx.inputModule;

            FSM_Normal_Enter(ctx, role, dt);//施法状态结束 进入正常状态
            inputCom.ClearCastKeys();

            // TODO:
            input.isUpdateProcess = true;

        }
        #endregion


        #region 进攻战

        public static void FSM_AllOffensive_Execute(GameSystemContext ctx, RoleEntity role, float dt) {
            var game = ctx.gameEntity;

            // 任何状态都运行的逻辑
            Loco_MoveOffensive(role);
            // skill
            int skillLen = role.skillorSlotComponent.TakeAll(out var skills);
            for (int i = 0; i < skillLen; i += 1) {
                var skill = skills[i];
                skill.cd -= dt;
                if (skill.cd < 0) {
                    skill.cd = 0;
                }
                ctx.appUI.Panel_GameDefend_Update(skill, skill.cd, skill.cdMax);
            }

        }

        #endregion

        #region 资源战
        public static void FSM_AllEliminate_Execute(GameSystemContext ctx, RoleEntity role, float dt) { // 只有障碍和核心逻辑
            if (role.moveType == RoleMoveType.Prohibit) {
                role.Move_Stop();
            }

            bool isLeave = ComparePos(role);

            if (isLeave) {
                role.Fall();
            }
        }
        #endregion

        #region Execute(tick)
        // Tick Execute循环的意思
        public static void FSM_Casting_Hand_Execute(GameSystemContext ctx, RoleEntity role, RoleCastingStateModel stateModel, float dt) {
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
                var handStateModel = role.FSMComponent.CastingStateModel;
                if (handStateModel.skillor == null) {
                    FSM_Casting_Exit(ctx, role, dt);
                }
                stateModel.skillor = null;
                return;
            }

            Skillor_Input_FlushKeys(ctx, role, dt);

            var action = frames[stateModel.frameIndex];
            // RoleDomain.Skillor_Act_Move(ctx, role, skillor, action, dt);

            action.loopModel.Tick(dt, out bool istrigger, out bool isNextFrame);

            if (istrigger) {
                RoleDomain.Skillor_Frame_Acting(ctx, role, stateModel, action, dt);
            }

            if (isNextFrame) {
                actionIndex += 1;
                if (actionIndex >= frames.Length) {
                    // var nextFrame = frames[actionIndex];
                    // RoleDomain.Skillor_Act_BeforeActing(ctx, role, stateModel, nextFrame, dt);// 重置
                    FSM_Casting_Exit(ctx, role, dt);
                }
            }
        }
        #endregion

    }
}