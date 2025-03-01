using System;
using UnityEngine;

namespace UF {
    public static partial class RoleDomain {

        public static void LocoDefendAndOffensive_Execute(GameSystemContext ctx, RoleEntity role, float dt) {

            // 人物移动
            var moveCom = role.MoveComponent;
            GroundCheck(ctx, role);
            ControlledCheck(ctx, role);

            Loco_MoveReset(role);
            Loco_Jump(role, moveCom.jumpForce);
            Loco_MoveDefend(ctx, role, dt);

            // 抓取
            Loco_Grap(ctx, role);
        }
        #region Move: 防守战
        static void Loco_MoveDefend(GameSystemContext ctx, RoleEntity role, float dt) {
            if (dt <= 0) {
                return;
            }

            if (role.state != RoleState.Move) {
                return;
            }

            // TODO:改成近基地死掉
            if (role.transform.position.y >= 11.25) {
                role.Jump_Stop();
                return;
            }

            var attrCom = role.AttributeComponent;

            RoleMoveType moveType = role.moveType;

            if (moveType == RoleMoveType.PlayerUpDown) {
                if (role.IsGround() && role.AllowPlayerMove()) {
                    role.Move_Player(attrCom.moveSpeed);
                }
            } else if (moveType == RoleMoveType.Ghost) {
                role.Move_Ghost(attrCom.moveSpeed);
            }
        }

        public static void Loco_MoveStop(RoleEntity role) {
            role.Move_Stop();
        }

        public static void Loco_MoveReset(RoleEntity role) {
            role.Move_Reset();
        }

        public static void Loco_Jump(RoleEntity role, float jumpForce) {
            var inputCom = role.InputComponent;
            if (!inputCom.Jump_Get()) {
                return;
            }
            role.Jump();
        }

        #endregion

        #region Mvoe:进攻战

        static void Loco_MoveOffensive(RoleEntity role) {

            var inputCom = role.InputComponent;
            var attrCom = role.AttributeComponent;
            if (role.moveType == RoleMoveType.OffensiveUDRL) {
                role.Move_Offensive(inputCom.MoveDir, attrCom.GetMoveSpeed());
            } else if (role.moveType == RoleMoveType.Ghost) {

            } else if (role.moveType == RoleMoveType.PlayerUpDown) {

            } else if (role.moveType == RoleMoveType.Prohibit) {
                role.Move_Stop();
            }

        }

        #endregion

        #region 消除战

        public static void Loco_Grap(GameSystemContext ctx, RoleEntity role) {
            // 抓取
            bool canGrap = ctx.isRopeUnExist();
            var inputCom = role.InputComponent;

            if (canGrap && inputCom.Grap_Get()) {
                Vector2 pos = role.GetPos();
                ctx.ropeEntity = RopeDomain.Spawn(ctx, pos);
            }

        }

        #endregion

        public static void GroundCheck(GameSystemContext ctx, RoleEntity role) {
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

     


        static void ControlledCheck(GameSystemContext ctx, RoleEntity role) {
            var pos = role.GetPos();

            if (pos.x >= role.rightPos.x + 3) {
                role.isControlledRight = false;
            }
            if (pos.y <= role.downPos.y - 2) {
                role.isControlledDown = false;
            }
        }
    }
}