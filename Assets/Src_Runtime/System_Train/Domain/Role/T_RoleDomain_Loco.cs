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
            Loco_Defense(ctx, role);

            RecoverMoveSpeed(ctx, role);
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
            var train = ctx.trainEntity;
            var inputCom = role.InputComponent;

            int skillLen = role.skillorSlotComponent.TakeAll(out var skills);
            var skill = skills[0]; // 只获取普攻, 要求必须配第一个技能是Attack, 看看后面能不能改

            if (role.t_roleType == T_RoleType.Player1) {
                if (!inputCom.Player1Attack_Get() || train.attackCD1 > 0) { return; }
                BulletDomain.SpawnByRolePos(ctx, role);
                train.attackCD1 = skill.cd;
                train.attackMax1 = skill.cd;
            } else if (role.t_roleType == T_RoleType.Player2) {
                if (!inputCom.Player2Attack_Get() || train.attackCD2 > 0) { return; }
                BulletDomain.SpawnByRolePos(ctx, role);
                train.attackCD2 = skill.cd;
                train.attackMax2 = skill.cd;
            }
        }

        static void Loco_Defense(TrainSystemContext ctx, RoleEntity role) { // 防御
            var train = ctx.trainEntity;
            var inputCom = role.InputComponent;

            if (role.t_roleType == T_RoleType.Player1) {
                if (!inputCom.Player1Defense_Get() || train.defenseCD1 > 0) { return; }
                PropEntity prop = PropDomain.SpawnByRole(ctx, role);

                prop.type = PropType.Armor1;
                train.defenseCD1 = train.defenseMax;

                if (train.player1Shields > 0) {
                    prop.times = 2;
                }
            } else if (role.t_roleType == T_RoleType.Player2) {
                if (!inputCom.Player2Defense_Get() || train.defenseCD2 > 0) { return; }
                PropEntity prop = PropDomain.SpawnByRole(ctx, role);

                prop.type = PropType.Armor2;
                train.defenseCD2 = train.defenseMax;

                if (train.player2Shields > 0) {
                    prop.times = 2;
                }
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

        static void RecoverMoveSpeed(TrainSystemContext ctx, RoleEntity role) {
            var train = ctx.trainEntity;
            ctx.templateMoudle.Role_TryGet(role.typeID, out var tm);

            if (role.t_roleType == T_RoleType.Player1 && train.player2Trap <= 0) {
                role.AttributeComponent.SetMoveSpeed(tm.moveSpeed);
                role.MoveComponent.Set_Garvity(1);
            } else if (role.t_roleType == T_RoleType.Player2 && train.player1Trap <= 0) {
                role.AttributeComponent.SetMoveSpeed(tm.moveSpeed);
                role.MoveComponent.Set_Garvity(1);
            }
        }

        #region AI
        public static void AI_Execute(TrainSystemContext ctx, RoleEntity role, float dt) {
            GroundCheck(role);

            Loco_MoveReset(role);
            Loco_Move(role, dt);
        }

        static int AI_Check(TrainSystemContext ctx, RoleEntity role) { // 1防御, 2攻击, 3跳跃

            float randomDev = UnityEngine.Random.Range(0f, role.AIDeviation); // ai的偏差
            var pos = role.GetPos();

            var attrCom = role.AttributeComponent;
            var moveSpeed = attrCom.GetMoveSpeed();
            float attackCD;
            float defenseCD;

            float g = 9.81f; // 重力加速度
            float minY = role.GetMinY();

            if (role.t_roleType == T_RoleType.Player1) {
                attackCD = ctx.trainEntity.attackCD1;
                defenseCD = ctx.trainEntity.defenseCD1;
            } else if (role.t_roleType == T_RoleType.Player2) {
                attackCD = ctx.trainEntity.attackCD2;
                defenseCD = ctx.trainEntity.defenseCD2;
            }

            int roleLen = ctx.roleRepository.TakeAll(out var roles);
            RoleEntity enemy = null;

            for (int i = 0; i < roleLen; i++) {
                if (roles[i] != role) {
                    enemy = roles[i];
                }
            }

            BulletMoveType type = BulletMoveType.None;
            if (enemy.t_roleType == T_RoleType.Player1) {
                type = BulletMoveType.Shaped;
            } else if (enemy.t_roleType == T_RoleType.Player2) {
                type = BulletMoveType.ToLeft;
            }

            int bulletLen = ctx.bulletRepository.TakeAll(out var bullets);
            for (int i = 0; i < bulletLen; i++) {
                if (bullets[i].moveType == type) {
                    // TODO: 检测子弹位置, 判断防御(防御cd不够就跳跃)
                    var bullet = bullets[i];
                    var bulletPos = bullet.TF_GetPos();
                    var bulletSpeed = bullet.moveSpeed;

                    if (bulletPos.x > pos.x) { return 0; }

                    float time = (pos.x - bulletPos.x) / bulletSpeed; // 子弹到达玩家x轴时间
                    float upTime = moveSpeed / g; // 玩家上升总时间
                    float T = 2 * upTime; // 玩家运动周期
                    time = time % T;
                    float playerY = minY;
                    float maxY = minY + (float)(Math.Sqrt(moveSpeed) / (2 * g)); // 玩家最高点
                    // TODO: 还有种可能是跳跃, 因为改变了velo, 所以这么算不准, 还没想好怎么写
                    if (time < upTime) {
                        playerY += (moveSpeed * time) - (float)(g / 2 * Math.Sqrt(time));
                    } else {
                        playerY += maxY - (float)(g / 2 * Math.Sqrt(time - upTime));
                    }

                    float dis = Mathf.Abs(playerY - bulletPos.y);
                    if (dis < randomDev) {
                        return 1;
                    }
                }
            }

            // TODO: 检测敌人位置, 判断攻击

            int propLen = ctx.propRepository.TakeAll(out var props);
            for (int i = 0; i < propLen; i++) {
                // TODO: 检测道具位置, 判断攻击和跳跃
            }

            return 0;
        }

        static void AI_Jump(TrainSystemContext ctx, RoleEntity role) {

        }
        #endregion
    }
}