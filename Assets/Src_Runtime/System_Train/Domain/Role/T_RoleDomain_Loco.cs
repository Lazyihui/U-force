using System;
using UnityEngine;

namespace UF.System_Train {
    public static partial class RoleDomain {
        public static void Loco_Execute(TrainSystemContext ctx, RoleEntity role, float dt) {
            // var moveCom = role.MoveComponent;
            GroundCheck(role);

            Loco_MoveReset(role);
            Loco_Move(role, dt);

            Loco_Jump(role);
            Loco_Attack(ctx, role);

            // Debug.Log(role.InputComponent.Player1Attack_Get());
        }

        #region Move
        static void Loco_Move(RoleEntity role, float dt) {
            if (dt <= 0) {
                return;
            }

            if (role.state != RoleState.Move) {
                return;
            }

            // 超出屏幕
            if (role.transform.position.y >= 11.25) {
                role.Jump_Stop();
                return;
            }

            var attrCom = role.AttributeComponent;

            RoleMoveType moveType = role.moveType;

            if (role.IsGround() && role.AllowPlayerMove()) {
                role.Move_Player(attrCom.moveSpeed);
            }
        }

        static void Loco_MoveReset(RoleEntity role) {
            role.Move_Reset();
        }

        static void Loco_Jump(RoleEntity role) {
            var inputCom = role.InputComponent;

            if (role.t_roleType == T_RoleType.Player1) {
                if (!inputCom.Player1Jump_Get()) { return; }
                role.Jump();
            } else if (role.t_roleType == T_RoleType.Player2) {
                if (!inputCom.Player2Jump_Get()) { return; }
                role.Jump();
            }
        }
        #endregion

        // 先放在Loco里
        static void Loco_Attack(TrainSystemContext ctx, RoleEntity role) {
            var inputCom = role.InputComponent;

            if (role.t_roleType == T_RoleType.Player1) {
                if (!inputCom.Player1Attack_Get()) { return; }
                BulletDomain.SpawnByRolePos(ctx, role);
            } else if (role.t_roleType == T_RoleType.Player2) {
                if (!inputCom.Player2Attack_Get()) { return; }
                BulletDomain.SpawnByRolePos(ctx, role);
            }
        }

        static void Loco_Armor(TrainSystemContext ctx, RoleEntity role) { // 防御
            var inputCom = role.InputComponent;

            if (role.t_roleType == T_RoleType.Player1) {
                if (!inputCom.Player1Defense_Get()) { return; }
                PropDomain.SpawnByRole(ctx, role);
            } else if (role.t_roleType == T_RoleType.Player2) {
                if (!inputCom.Player2Defense_Get()) { return; }
                PropDomain.SpawnByRole(ctx, role);
            }
        }

        static void GroundCheck(RoleEntity role) {
            bool isColliderGround = false;
            if (role.GetVelocity().y > 0) {
                return;
            }

            RaycastHit2D[] hits = Physics2D.RaycastAll(role.GetPos(), Vector2.down, 1.1f);
            Debug.DrawRay(role.GetPos(), Vector2.down * 1.1f, Color.red, 0.1f);

            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];
                if (hit.collider.gameObject != null) {
                }

                if (hit.collider.gameObject.CompareTag("Ground")) {
                    isColliderGround = true;
                    break;
                }
            }

            if (isColliderGround) {
                role.EnterGround();
            }
        }
    }
}