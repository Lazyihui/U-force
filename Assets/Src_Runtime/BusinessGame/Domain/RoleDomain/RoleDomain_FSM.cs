using System;
using System.Collections;
using UnityEngine;


namespace UF {
    public static partial class RoleDomain {
        public static void FSM_Normal_Execute(GameContext ctx, RoleEntity role, float dt) {
            // Skill Cast
            Skillor_Input_FlushKeys(ctx, role, dt);
        }

        public static void FSM_Casting_Execute(GameContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            FSM_Casting_Hand_Execute(ctx, role, fsm.CastingStateModel, dt);
        }

        public static void FSM_All_Execute(GameContext ctx, RoleEntity role, float dt) {
            // 任何状态都运行的逻辑
            GroundCheck(ctx, role);
            Loco_Move(ctx, role, 15, dt);

            // skill
            int skillLen = role.skillorSlotComponent.TakeAll(out var skills);
            for (int i = 0; i < skillLen; i += 1) {
                var skill = skills[i];
                skill.cd -= dt;
                if (skill.cd < 0) {
                    skill.cd = 0;
                }
            }

        }
        static void FSM_Casting_Enter(GameContext ctx, RoleEntity role, SkillModel skillor, float dt) {
            // 完成
            var fsm = role.FSMComponent;
            fsm.Casting_Enter(skillor);
            Debug.Log("FSM_Casting_Enter");
        }

        static void FSM_Normal_Enter(GameContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            fsm.Normal_Enter();
        }

        static void FSM_Casting_Exit(GameContext ctx, RoleEntity role, float dt) {
            var fsm = role.FSMComponent;
            FSM_Normal_Enter(ctx, role, dt);
            Debug.Log("FSM_Casting_Exit");
        }


        #region Execute(tick)
        // Tick Execute循环的意思
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