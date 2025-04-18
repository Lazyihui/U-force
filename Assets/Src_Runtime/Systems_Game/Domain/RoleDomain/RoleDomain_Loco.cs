using System;
using UnityEngine;

namespace UF {
    public static partial class RoleDomain {

        public static void LocoDefendAndOffensive_Execute(GameSystemContext ctx, RoleEntity role, float dt) {
            // 人物移动
            var moveCom = role.MoveComponent;
            GroundCheck(ctx, role);
            ControlledCheck(ctx, role);

            role.Move_Reset();
            Loco_Jump(role, moveCom.jumpForce);
            Loco_MoveDefend(ctx, role, dt);

            ElectricityCheck(ctx, role, dt); // 带电检测, 不知道放哪了，先放这

            // 抓取
            Loco_Grap(ctx, role);
        }

        // 进攻战
        static void Loco_MoveOffensive(RoleEntity role, RoleEntity target, float dt) {

            var inputCom = role.InputComponent;
            var attrCom = role.AttributeComponent;

            // RoleEntity onwer = role.Get_Owner();

            if (role.moveType == RoleMoveType.OffensiveUDRL) {
                role.Move_Offensive(inputCom.MoveDir, attrCom.GetMoveSpeed(), dt);
            } else if (role.moveType == RoleMoveType.Ghost) {

            } else if (role.moveType == RoleMoveType.PlayerUpDown) {

            } else if (role.moveType == RoleMoveType.Prohibit) {
                role.Move_Stop();
            } else if (role.moveType == RoleMoveType.MoveTarget) {
                role.Move_Target(target.GetPos(), attrCom.GetMoveSpeed(), dt);
            }


        }

        #region Move: 防守战

        static void Loco_MoveDefend(GameSystemContext ctx, RoleEntity role, float dt) {
            if (dt <= 0) {
                return;
            }

            if (role.state != RoleState.Move) {
                return;
            }
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

            // 加速怪
            Accelerate(role, dt);
        }

        public static void Loco_Jump(RoleEntity role, float jumpForce) {
            var inputCom = role.InputComponent;
            if (!inputCom.Jump_Get()) {
                return;
            }
            role.Jump();
        }
        #endregion

        #region Move: 资源战
        public static void Loco_MoveEliminate(GameSystemContext ctx, RoleEntity role, float dt) {
            if (dt <= 0) {
                return;
            }

            if (role.state != RoleState.Move) {
                return;
            }

            var attrCom = role.AttributeComponent;

            RoleMoveType moveType = role.moveType;

            if (moveType == RoleMoveType.Regular) {
                role.Move_Regular(attrCom.moveSpeed);
            } else if (moveType == RoleMoveType.Random) {
                role.Move_Random(attrCom.moveSpeed);
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
            if (role.roleType == RoleType.Player) { return; }

            var pos = role.GetPos();

            if (pos.x >= role.repulsedPos.x + role.repulsedLength) {
                role.isRepulsed = false;
            }

            var dirY = role.changeLineDir.y;
            if (dirY > 0 && pos.y >= role.changeLinePos.y + role.changeLineLength) {
                role.isChangeLine = false;
                role.hasChangeLinePos = false;
                role.Set_RbConstraints();
            } else if (dirY < 0 && pos.y <= role.changeLinePos.y - role.changeLineLength) {
                role.isChangeLine = false;
                role.hasChangeLinePos = false;
                role.Set_RbConstraints();
            }
        }

        #region 移动
        public static void Accelerate(RoleEntity role, float dt) {
            // 加速
            var feaCom = role.FeatureComponent;
            if (feaCom.featureType != FeatureType.Accelerate) {
                return;
            }

            feaCom.AccelerateTimer -= dt;
            if (feaCom.AccelerateTimer <= 0) {
                feaCom.SetFeatureType(FeatureType.None);
                var attrCom = role.AttributeComponent;
                attrCom.moveSpeed = attrCom.moveSpeed * 2;
            }
        }

        #endregion

        #region 带电掉血
        public static void ElectricityCheck(GameSystemContext ctx, RoleEntity role, float dt) {
            if (role.isElectricity) {
                if (role.elecTimes <= 0) {
                    role.isElectricity = false;
                    return;
                }

                role.elecTimer -= dt;

                if (role.elecTimer <= 0) {
                    HpReduce_Minion(ctx, role, role.elecDamage);
                    role.elecTimes -= 1;
                    role.elecTimer = role.elecMax;
                }
            }
        }
        #endregion
    }
}