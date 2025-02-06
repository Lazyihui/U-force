using System;
using UnityEngine;

namespace UF {
    public static partial class RoleDomain {

        public static void Loco_Execute(GameContext ctx, RoleEntity role, float dt) {
            var moveCom = role.MoveComponent;
            GroundCheck(ctx, role);

            Loco_MoveReset(ctx, role);
            Loco_Jump(ctx, role, moveCom.jumpForce);
            Loco_Move(ctx, role, dt);
        }
        #region Move: Move
        static void Loco_Move(GameContext ctx, RoleEntity role, float dt) {
            if (dt <= 0) {
                return;
            }

            if (role.state != RoleState.Move) {
                return;
            }

            // 超出屏幕
            if (role.transform.position.y >= 11.25)
            {
                role.Jump_Stop();
                return;
            }

            var moveCom = role.MoveComponent;

            RoleMoveType moveType = role.MoveType;

            if (moveType == RoleMoveType.Player) {
                if (role.IsGround() && role.AllowPlayerMove()) {
                    role.Move_Player(moveCom.moveSpeed);
                }
            } else if (moveType == RoleMoveType.Ghost) {
                role.Move_Ghost(moveCom.moveSpeed);

            }
        }

        public static void Loco_MoveStop(GameContext ctx, RoleEntity role) {
            role.Move_Stop();
        }

        public static void Loco_MoveReset(GameContext ctx, RoleEntity role) {
            role.Move_Reset();
        }

        public static void Loco_Jump(GameContext ctx, RoleEntity role, float jumpForce) {
            var inputCom = role.InputComponent;
            if (!inputCom.Jump_Get()) {
                return;
            }
            role.Jump();
        }

        #endregion

        public static void GroundCheck(GameContext ctx, RoleEntity role) {
            if (role.roleType != RoleType.Player) {
                return;
            }

            bool isColliderGround = false;
            if (role.GetVelocity().y > 0) {
                return;
            }

            RaycastHit2D[] hits = Physics2D.RaycastAll(role.GetPos(), Vector2.down, 1.1f);
            Debug.DrawRay(role.GetPos(), Vector2.down * 1.1f, Color.red, 0.1f);

            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];
                if (hit.collider.gameObject != null) {
                    Debug.LogWarning("Collider:" + hit.collider.gameObject.tag);
                }

                if (hit.collider.gameObject.CompareTag("Ground")) {
                    isColliderGround = true;
                    Debug.LogWarning("ColliderGround");
                    break;
                }
            }

            if (isColliderGround) {
                role.EnterGround();
            }
        }
    }
}