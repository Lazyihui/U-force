using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public static partial class RoleDomain {

        #region LifeCycle

        public static RoleEntity SpawnByTypeID(GameSystemContext ctx, int typeID) {
            RoleEntity role = GameFactory.Role_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, typeID);

            // Boss战玩家血量固定为3
            var game = ctx.gameEntity;
            if (game.isBossWar) {
                role.BossWarPlayer();
            }

            ctx.roleRepository.Add(role);
            return role;
        }

        public static RoleEntity SpawnByPos(GameSystemContext ctx, int typeID, Vector2 pos) {
            RoleEntity entity = SpawnByTypeID(ctx, typeID);
            entity.TF_SetPos(pos);
            return entity;
        }

        public static RoleEntity SpawnBySpawner(GameSystemContext ctx, RoleSpawnerTM spawnerTM) {
            RoleEntity role = GameFactory.Role_CreateBySpawner(ctx.assetsModule, ctx.templateCore, ctx.idService, spawnerTM);
            ctx.roleRepository.Add(role);
            return role;
        }
        public static void UnSpawn(GameSystemContext ctx, RoleEntity role) {
            ctx.roleRepository.Remove(role);
            role.TearDown();
        }

        public static void Clear(GameSystemContext ctx) {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];
                role.TearDown();
            }
            ctx.roleRepository.Clear();
        }

        #endregion

        #region Input   
        public static void Input_Record(GameSystemContext ctx, RoleEntity role, float dt) {
            var game = ctx.gameEntity;
            var inputCom = role.InputComponent;
            var inputModule = ctx.inputModule;
            inputCom.Jump_Set(inputModule.IsJump);
            if (game.isEliminateWar) {
                inputCom.Grap_Set(inputModule.IsGrap);
            }
            inputCom.RecoredCastKey(inputModule.InputCastKey);
            inputCom.MoveDir_Set(inputModule.MoveDir);
            inputCom.XAxis_Set(inputModule.XAxis);
        }
        #endregion

        #region Player physics
        public static void DetectionMinionByPos(GameSystemContext ctx, RoleEntity role, Vector2 pos, int damage) {
            // minion进入基地
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] minions);

            for (int i = 0; i < len; i++) {

                var minion = minions[i];
                if (minion == role) {
                    continue;
                }

                var minionPos = minion.TF_GetPos();
                float homeX = -12; // 基地的x坐标

                // TODO:可能要改成用 collider   
                                
                // 近基地死亡
                if (minionPos.x <= homeX) {
                    if (minion.state == RoleState.Dead || minion.state == RoleState.Fall) {
                        return;
                    }

                    // player扣血 或者失败  
                    HpReduce_Role(ctx, role, damage);
                    // minion进基地死掉
                    int hpMax = minion.AttributeComponent.GetHpMax();
                    HpReduce_Role(ctx, minion, hpMax);
                }
            }
        }
        public static void DetectionEnemyAndEnemyByPos(GameSystemContext ctx, RoleEntity Enemy, Vector2 pos, int damage) {
            // 检测周围的minion 碰到扣血
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] touchMinions);

            for (int i = 0; i < len; i++) {
                var touchMinion = touchMinions[i];
                if (touchMinion == Enemy) {
                    continue;
                }
                var owner = ctx.Get_Owner(); // 检测是否碰到玩家
                if (touchMinion == owner) {
                    continue;
                }
                float dis = Vector2.Distance(touchMinion.TF_GetPos(), pos);
                // TODO:可能要改成用 collider   
                float radius = 2f;
                if (dis < radius) {
                    HpReduce_Minion(ctx, touchMinion, damage);
                }
            }
        }

        // 角色碰到敌人扣血
        public static void DetectionOwnerAndEnemy(GameSystemContext ctx, RoleEntity owner) {
            int len = ctx.roleRepository.TakeAll(out RoleEntity[] enemys);
            for (int i = 0; i < len; i++) {
                var enemy = enemys[i];
                if (enemy == owner) {
                    continue;
                }
                float dis = Vector2.Distance(enemy.TF_GetPos(), owner.TF_GetPos());
                if (dis < 1.5f) {
                    var attCom = enemy.AttributeComponent;
                    int damage = attCom.GetBaseDamage();
                    HpReduce_Role(ctx, owner, damage);

                    // TODO:先这样写
                    UnSpawn(ctx, enemy);
                }
            }
        }

        public static void HpReduce_Minion(GameSystemContext ctx, RoleEntity role, int damage) {

            var attCom = role.AttributeComponent;
            var feaCom = role.FeatureComponent;
            int finalHp = attCom.hp - damage;
            var game = ctx.gameEntity;
            role.Attr_Hp_Set(finalHp);
            if (attCom.hp <= 0) {
                if (feaCom.featureType == FeatureType.HurtOneself) {
                    BulletEntity bullet = BulletDomain.SpawnByPos(ctx, 5000, role.TF_GetPos(), new Vector3(0, 0, 0));
                    role.SetRoleState(RoleState.Dead); //TODO:这样写其实不好

                } else if (game.isEliminateWar) {
                    role.SetRoleState(RoleState.Dead);

                    if (role.roleType == RoleType.Core) {
                        CoreDead(ctx);
                    }

                } else {
                    role.SetRoleState(RoleState.Fall);
                    role.Fall();
                    role.Set_RbConstraints();
                }
            }
        }

        public static void HpReduce_Role(GameSystemContext ctx, RoleEntity role, int damage) {
            var playerData = ctx.gameEntity.playerSaveData;
            if (role.roleType == RoleType.Player) {
                // 受到的伤害
                playerData.SetOwnerGetHurt(playerData.GetRoleGetHurt() + damage);
            }
            var attCom = role.AttributeComponent;
            int finalHp = attCom.hp - damage;
            role.Attr_Hp_Set(finalHp);

            if (attCom.hp <= 0) {
                role.SetRoleState(RoleState.Dead);
            }
        }

        // 核心死亡, 所有物品掉落
        public static void CoreDead(GameSystemContext ctx) {
            var game = ctx.gameEntity;
            if (!game.isEliminateWar) { // 没什么用的检测, 但是写上就显得很严谨
                return;
            }

            int roleLen = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            int StuffLen = ctx.stuffRepository.TakeAll(out StuffEntity[] stuffs);

            for (int i = 0; i < roleLen; i++) {
                var role = roles[i];
                if (role.roleType == RoleType.Player || role.roleType == RoleType.Core) {
                    continue;
                }

                role.SetRoleState(RoleState.Fall);
                role.Fall();
                role.Set_RbConstraints();
            }

            for (int i = 0; i < StuffLen; i++) {
                var stuff = stuffs[i];
                if (stuff.typeID != 100) {
                    continue;
                }

                stuff.Fall();
            }
        }

        // 下落过程的检测 检测到地面状态变为死亡 和检测是否碰到敌人
        public static void FallingtodeathCheck(GameSystemContext ctx, RoleEntity role) {
            if (role.state != RoleState.Fall) {
                return;
            }
            // TODO:感觉是这里的问题
            DetectionEnemyAndEnemyByPos(ctx, role, role.GetPos(), role.AttributeComponent.GetFallDamage());
            // 和地面的交叉检测
            RaycastHit2D[] hits = Physics2D.RaycastAll(role.GetPos(), Vector2.down, 1.1f);
            Debug.DrawRay(role.GetPos(), Vector2.down * 0.6f, Color.gray, 0.1f);

            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];
                if (hit.collider.gameObject.CompareTag("Ground")) {
                    Debug.Log("死亡动画");
                    role.SetRoleState(RoleState.Dead);
                }
            }
        }

        // 否是死亡状态 
        public static void IsDeadState(GameSystemContext ctx, RoleEntity role) {
            var game = ctx.gameEntity;
            var pos = role.GetPos();
            if (pos.x < -21 || pos.x > 21 || pos.y < -12 || pos.y > 12) {
                role.SetRoleState(RoleState.Dead);
            }
            if (role.state != RoleState.Dead) {
                return;
            }

            if (role.roleType == RoleType.Player) {
                // TODO:L写这里其实不太好
                var ui = ctx.appUI;
                var attr = role.AttributeComponent;
                UIDomain.HUD_Progressbar_Update(ui, attr, game.wavesProgress);

                ctx.gameEntity.state = GameState.GameOver;
                Debug.Log("GameOver");
                var playerData = game.playerSaveData;
                ctx.appUI.Panel_GameOver_Open(false, 0);

                return;
            }

            UnSpawn(ctx, role);
        }

        #endregion

        #region 资源战检测
        public static bool ComparePos(RoleEntity role) { // true表示离开引力场
            if (!role.isEliminateRole) { return false; }

            var pos = role.TF_GetPos();
            Vector2 pointPos = new Vector2(9, 0);

            float dis = Vector2.Distance(pos, pointPos);

            if (dis < 10) {
                return false;
            }
            role.state = RoleState.Fall;
            return true;
        }
        #endregion
    }
}