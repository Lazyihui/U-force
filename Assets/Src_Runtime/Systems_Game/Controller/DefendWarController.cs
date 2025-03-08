using System;
using UnityEngine;

namespace UF {
    public static class DefendWarController { // 防守战
        public static void Enter(GameSystemContext ctx, int stageID, int selectedRoleTypeID, int[] skillorTypeIDArray, int sciType, StageTM tm) {
            var game = ctx.gameEntity;
            var playerData = game.playerSaveData;

            // UI
            ctx.appUI.Panel_GameDefend_Button_Open(playerData.slotMax);
            ctx.appUI.HUD_Progressbar_Open(game.maxWaves);
            if (stageID == 1) {
                ctx.appUI.Panel_GameDefend_SetBtnTech_Hide();
            }

            // Map
            MapEntity map = MapDomain.Spawn(ctx.assetsModule, ctx.idService, tm.mapPrefab);
            ctx.gameEntity.playerSaveData.SetStageCurID(stageID);
            ctx.gameEntity.playerSaveData.mapEntity = map;

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
            var ui = ctx.appUI;
            RoleEntity owner = ctx.Get_Owner();
            // 生成怪物 不需要循环
            CaveDomain.TrySpawnMinion(ctx, dt);
            // UI
            var attrCom = owner.AttributeComponent;
            UIDomain.HUD_Progressbar_Update(ctx.appUI, attrCom, game.wavesProgress);

            // 输入记录
            RoleDomain.Input_Record(ctx, owner, dt);
        }

        public static void Tick(GameSystemContext ctx, float dt) {

            var game = ctx.gameEntity;
            //  Role
            RoleEntity owner = ctx.Get_Owner();
            if (owner == null) {
                return;
            }

            // Player的逻辑
            var fsm = owner.FSMComponent;
            var state = fsm.Status;
            var attrCom = owner.AttributeComponent;
            RoleDomain.DetectionMinionByPos(ctx, owner, owner.TF_GetPos(), attrCom.GetBaseDamage());

            if (game.state == GameState.GameOver) {
                return;
            }

            int len = ctx.roleRepository.TakeAll(out RoleEntity[] roles);
            for (int i = 0; i < len; i += 1) {
                RoleEntity role = roles[i];
                // 这里是player和Minion的逻辑
                RoleDomain.FSM_All_Execute(ctx, role, dt);
            }

            // 这里是主角的逻辑
            if (state == RoleFSMStatus.Normal) {
                RoleDomain.FSM_Normal_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Casting) {
                RoleDomain.FSM_Casting_Execute(ctx, owner, dt);
            } else if (state == RoleFSMStatus.Dead) {

            }
            // 所有状态都要执行

            //   bullet
            int bulletLen = ctx.bulletRepository.TakeAll(out BulletEntity[] bullets);

            for (int i = 0; i < bulletLen; i += 1) {
                BulletEntity bullet = bullets[i];
                BulletDomain.CheckUnSpawn(ctx, bullet, dt);
                BulletDomain.FSM_All_Execute(ctx, bullet, dt);
            }


            //  tower
            int towerLen = ctx.towerRepository.TakeAll(out CaveEntity[] towers);

            for (int i = 0; i < towerLen; i += 1) {
                CaveEntity tower = towers[i];
                CaveDomain.FSM_All_Execute(ctx, tower, dt);
            }

            // Stuff
            int stuffLen = ctx.stuffRepository.TakeAll(out StuffEntity[] stuffs);
            for (int i = 0; i < stuffLen; i += 1) {
                StuffEntity stuff = stuffs[i];
                StuffDomain.CheckUnSpawn(ctx, stuff);
                StuffDomain.FSM_Normal_Execute(ctx, stuff, dt);
            }
        }

        public static void LastTick(GameSystemContext ctx,float dt) {

        }
    }
}