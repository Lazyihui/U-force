using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public static partial class RoleDomain {
        public static RoleEntity Spawn(GameContext ctx, int typeID) {
            RoleEntity role = GameFactory.Role_CreateBySpawner(ctx.assetsCore, ctx.templateCore, ctx.idService, Vector3.zero, typeID);
            ctx.roleRepository.Add(role);
            return role;
        }

        #region Move & Jump
        public static void Move(GameContext ctx, RoleEntity role, float moveSpeed, float dt) {
            if (dt <= 0) {
                return;
            }

            RoleMoveType moveType = role.moveType;

            if (moveType == RoleMoveType.Player) {
                if (!role.AllowPlayerMove()) {
                    return;
                }
                role.Move_Player(Vector2.up, moveSpeed, dt);
            } else if (moveType == RoleMoveType.Ghost) {
                // role.Move(Vector2.left, moveSpeed, false, dt);
            }
        }

        #endregion

        public static void GroundCheck(GameContext ctx, RoleEntity role) {
            bool isColliderGround = false;
            if (role.GetVelocity().y > 0) {
                return;
            }

            RaycastHit2D[] hits = Physics2D.RaycastAll(role.GetPos(), Vector2.down, 0.6f);
            Debug.DrawRay(role.GetPos(), Vector2.down * 0.6f, Color.red, 0.1f);

            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];
                if (hit.collider.gameObject.CompareTag("Ground")) {
                    isColliderGround = true;
                    break;
                }
            }

            if (isColliderGround) {
                role.EnterGround();
            }
        }

        #region Act 行为
        public static void Skillor_Frame_Acting(GameContext ctx, RoleEntity role, RoleCastingStateModel stateModel, SkillFrameModel frame, float dt) {
            var skillor = stateModel.skillor;

            if (skillor == null) {
                return;
            }

            if (frame.loopModel.isDone) {
                return;
            }

            frame.loopModel.CheckDone();

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
                bool succ = Skillor_Cast(ctx, role, skill, dt);
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

        #endregion
    }
}