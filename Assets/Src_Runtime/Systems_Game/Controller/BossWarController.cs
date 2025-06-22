using System;
using UnityEngine;

namespace UF {
    public static class BossWarController { // boos战

        public static void Enter(GameSystemContext ctx, int selectedRoleTypeID, int[] skillorTypeIDArray, int sciType, StageTM tm) {

            var game = ctx.gameEntity;
            var playerData = game.playerSaveData;

            // UI
            ctx.appUI.Panel_GameDefend_Button_Open(playerData.slotMax);
            ctx.appUI.HUD_Progressbar_Open(game.maxWaves);
            int stageID = playerData.stageCurID;
            Debug.Log("进入游戏" + stageID);

            if (stageID == 1) {
                ctx.appUI.Panel_GameDefend_SetBtnTech_Hide();
            }
            // 这里没有地图

            // Role
            RoleEntity owner = null;
            if (selectedRoleTypeID == -1) {
                // 没有自己选择的角色的情况
                for (int i = 0; i < tm.roleSpawns.Length; i += 1) {
                    RoleSpawnerTM spawnerTM = tm.roleSpawns[i];
                    owner = RoleDomain.SpawnBySpawner(ctx, spawnerTM);
                    if (owner.roleType == RoleType.Player) {
                        game.OwnerIDsig = owner.idSig;
                    }
                }
            }

            owner = RoleDomain.SpawnByPos(ctx, selectedRoleTypeID, new Vector3(-15, 0, 0));
            game.OwnerIDsig = owner.idSig;

            owner.invincibilityMax = 1.5f;

            var skillorCom = owner.skillorSlotComponent;

            // 大招
            if (sciType != -2 && sciType != -1) {
                SkillModel skillorSci = GameFactory.Skillor_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, sciType);
                skillorSci.Set_InputCastKey(InputKeyFlag.Tech);
                skillorCom.Add(skillorSci);
            }


            // 道具
            if (skillorTypeIDArray != null) {
                for (int i = 0; i < skillorTypeIDArray.Length; i += 1) {
                    var skilltypeID = skillorTypeIDArray[i];
                    SkillModel skillor = GameFactory.Skillor_Create(ctx.assetsModule, ctx.templateCore, ctx.idService, skilltypeID);
                    skillor.Set_InputCastKeyWeap(i);
                    skillorCom.Add(skillor);
                }
            }

            // Tower
            for (int i = 0; i < tm.towerSpawns.Length; i += 1) {
                CaveSpawnerTM spawnerTM = tm.towerSpawns[i];
                CaveDomain.SpawnBySpawner(ctx, spawnerTM);
            }
        }

        public static void PreTick(GameSystemContext ctx, float dt) {
            var game = ctx.gameEntity;

            //  boss战不从cave生成怪物 因为怪物是随机生成的
            CaveDomain.TrySpawnEnemyRandomPos(ctx, dt);

            RoleEntity owner = ctx.Get_Owner();
            // 输入记录
            RoleDomain.Input_Record(ctx, owner, dt);
            // UI
            var attrCom = owner.AttributeComponent;
            UIDomain.HUD_Progressbar_Update(ctx.appUI, attrCom, game.wavesProgress);
        }

        public static void Tick(GameSystemContext ctx, float dt) {

            var game = ctx.gameEntity;
            //  Role
            RoleEntity owner = ctx.Get_Owner();
            if (owner == null) {
                return;
            }

            // Player的逻辑 所有状态都要执行
            var fsm = owner.FSMComponent;
            var state = fsm.Status;
            var attrCom = owner.AttributeComponent;
            // RoleDomain.DetectionMinionByPos(ctx, owner, owner.TF_GetPos(), attrCom.GetBaseDamage());

            if (game.state == GameState.GameOver) {
                return;
            }
            // 遍历所有角色 碰到角色扣血
            RoleDomain.DetectionOwnerAndEnemy(ctx, owner);
            RoleDomain.CheckInvincible(ctx, owner, dt);

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];

                // 这里是player和Minion的逻辑
                RoleDomain.FSM_AllBossWar_Execute(ctx, role, dt);

                if (role.roleType == RoleType.Player) {
                    continue;
                }
                // 这里是小怪的逻辑
                int skillLen = role.skillorSlotComponent.TakeAll(out var skills);
                for (int j = 0; j < skillLen; j += 1) {
                    var skill = skills[j];
                    skill.cd -= dt;
                    if (skill.cd < 0) {
                        skill.cd = 0;
                    }

                    ctx.appUI.Panel_GameDefend_Update(skill, skill.cd, skill.cdMax);
                }

                var roleFSMStatus = role.FSMComponent.Status;
                if (roleFSMStatus == RoleFSMStatus.Dead) {

                } else if (roleFSMStatus == RoleFSMStatus.Normal) {
                    // 正常状态的逻辑
                    // RoleDomain.FSM_Normal_Execute(ctx, role, dt);
                    RoleDomain.Skillor_Cd(ctx, role, dt);

                } else if (roleFSMStatus == RoleFSMStatus.Casting) {
                    // 施法状态的逻辑
                    // RoleDomain.FSM_Casting_Execute(ctx, role, dt);
                    var fsmEnemy = role.FSMComponent;

                    RoleDomain.FSM_Casting_Cd_Execute(ctx, role, fsmEnemy.CastingStateModel, dt);

                }

            }

            // 主角一直要执行的逻辑
            RoleDomain.FSM_OwnerBossWar_Execute(ctx, owner, dt);

            // 这里是主角的逻辑 这里是主角施法的逻辑 （）    
            if (state == RoleFSMStatus.Normal) {
                RoleDomain.FSM_Normal_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Casting) {
                RoleDomain.FSM_Casting_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Dead) {

            }

            //   bullet
            int bulletLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);

            for (int i = 0; i < bulletLen; i += 1) {
                BulletEntity bullet = bullets[i];
                BulletDomain.FSM_All_Execute(ctx, bullet, dt);
                BulletDomain.CheckUnSpawn(ctx, bullet, dt);
            }

            // Laser
            int laserLen = ctx.laserRepository.TakeAll(out LaserEntity[] lasers);
            for (int i = 0; i < laserLen; i += 1) {
                LaserEntity laser = lasers[i];
                LaserDomain.FSM_All_Execute(ctx, laser, dt);
            }

            //  tower
            // int towerLen = ctx.towerRepository.TakeAll(out CaveEntity[] towers);

            // for (int i = 0; i < towerLen; i += 1) {
            //     CaveEntity tower = towers[i];
            //     CaveDomain.FSM_All_Execute(ctx, tower, dt);
            // }
        }

        public static void LastTick(GameSystemContext ctx, float dt) {

            RoleEntity owner = ctx.Get_Owner();
            if (owner == null) {
                return;
            }
            // 相机跟随
            var moduleCamera = ctx.moduleCamera;
            moduleCamera.Follow(owner.transform);
        }
    }
}