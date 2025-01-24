using System;
using UnityEngine;

namespace UF {
    public static partial class RoleDomain {
        #region Move: Move
        public static void Loco_Move(GameContext ctx, RoleEntity role, float moveSpeed, float dt) {
            if (dt <= 0) {
                return;
            }

            RoleMoveType moveType = role.moveType;

            if (moveType == RoleMoveType.Player) {
                if (role.IsGround() && role.AllowPlayerMove()) {
                    role.Move_Player(moveSpeed, dt);
                }
            } else if (moveType == RoleMoveType.Ghost) {
                Loco_Move_Ghost(ctx, role, moveSpeed, dt);
            }
        }

        public static void Loco_MoveStop(GameContext ctx, RoleEntity role) {
            //TODO
        }
        #endregion

        public static void Loco_Move_Ghost(GameContext ctx, RoleEntity role, float moveSpeed, float dt) {
            role.Move_Ghost(-1, moveSpeed, dt);
        }

        public static void Loco_Jump(GameContext ctx, RoleEntity role, bool isJumpKeyDown, float jumpForce) {
            role.Jump(isJumpKeyDown, jumpForce);
        }

        //地面检测不确定写在哪
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
    }
}