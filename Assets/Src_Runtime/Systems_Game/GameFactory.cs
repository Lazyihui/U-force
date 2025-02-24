using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public static class GameFactory {

        public static RoleEntity Role_CreateByTypeSkillType(AssetsModule assetsCore, TemplateMoudle templateCore,
         IDService idService, int typeID, int[] skillorTypeIDArray) {

            bool has = templateCore.Role_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Role_Create: tm is null" + typeID);
                return null;
            }
            GameObject prefab = assetsCore.Entity_GetRole();
            GameObject go = GameObject.Instantiate(prefab);
            RoleEntity role = go.GetComponent<RoleEntity>();
            string n = "Entity_Role_" + tm.typeName;
            if (go.name != n) {
                go.name = n;
            }

            var mod = RoleMod_Create(tm, role.bodyRoot);

            role.idSig = new IDSignature(EntityType.Role, idService.PickRoleID());
            role.roleType = tm.roleType;
            role.moveType = tm.moveType;
            role.SetRoleState(tm.roleState);


            role.Ctor();
            role.Inject(mod);
            // TODO:
            role.Set_RbConstraints();

            // -move
            var moveCom = role.MoveComponent;
            moveCom.SetJumpForce(tm.jumpForce);

            // -Attribute
             var attrCom = role.AttributeComponent;
            attrCom.SetHp(tm.hp);
            attrCom.SetHpMax(tm.hp);
            attrCom.SetMoveSpeed(tm.moveSpeed);


            // - Skillor
            var skillorCom = role.skillorSlotComponent;
            var skillorSoArray = tm.skillorSoArray;

            if (skillorSoArray != null) {
                for (int i = 0; i < skillorSoArray.Length; i += 1) {
                    var skillSo = skillorSoArray[i];
                    var skillor = Skillor_Create(assetsCore, templateCore, idService, skillSo.tm.typeID);
                    skillorCom.Add(skillor);
                }
            }


            return role;
        }
        public static RoleEntity Role_Create(AssetsModule assetsCore, TemplateMoudle templateCore, IDService idService, int typeID) {

            bool has = templateCore.Role_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Role_Create: tm is null" + typeID);
                return null;
            }
            GameObject prefab = assetsCore.Entity_GetRole();
            GameObject go = GameObject.Instantiate(prefab);
            RoleEntity role = go.GetComponent<RoleEntity>();
            Debug.Assert(role != null, "role != null");

            var mod = RoleMod_Create(tm, role.bodyRoot);

            role.idSig = new IDSignature(EntityType.Role, idService.PickRoleID());
            role.roleType = tm.roleType;
            role.moveType = tm.moveType;
            role.SetRoleState(tm.roleState);

            role.Ctor();
            role.Inject(mod);
            // TODO:
            role.Set_RbConstraints();

            // -move
            var moveCom = role.MoveComponent;
            moveCom.SetJumpForce(tm.jumpForce);

            // -Attribute
            var attrCom = role.AttributeComponent;
            attrCom.SetHp(tm.hp);
            attrCom.SetHpMax(tm.hp);
            attrCom.SetMoveSpeed(tm.moveSpeed);


            // - Skillor
            var skillorCom = role.skillorSlotComponent;
            var skillorSoArray = tm.skillorSoArray;

            if (skillorSoArray != null) {
                for (int i = 0; i < skillorSoArray.Length; i += 1) {
                    var skillSo = skillorSoArray[i];
                    var skillor = Skillor_Create(assetsCore, templateCore, idService, skillSo.tm.typeID);
                    skillorCom.Add(skillor);
                }
            }


            return role;
        }
        public static RoleEntity Role_CreateBySpawner(AssetsModule assetsCore, TemplateMoudle templateCore, IDService idService, RoleSpawnerTM spawnerTM) {

            int typeID = spawnerTM.so.tm.typeID;

            bool has = templateCore.Role_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Role_Create: tm is null" + typeID);
                return null;
            }
            GameObject prefab = assetsCore.Entity_GetRole();
            GameObject go = GameObject.Instantiate(prefab);
            RoleEntity role = go.GetComponent<RoleEntity>();
            Debug.Assert(role != null, "role != null");

            var mod = RoleMod_Create(tm, role.bodyRoot);

            role.idSig = new IDSignature(EntityType.Role, idService.PickRoleID());
            role.roleType = tm.roleType;
            role.moveType = tm.moveType;
            role.SetRoleState(tm.roleState);

            role.Ctor();
            role.Inject(mod);
            // TODO:
            role.Set_RbConstraints();

            // -move
            var moveCom = role.MoveComponent;
            moveCom.SetJumpForce(tm.jumpForce);

            // - Spawn
            role.TF_SetPos(spawnerTM.position);
            role.TF_SetRot(spawnerTM.rotation);

            // -Attribute
            var attrCom = role.AttributeComponent;
            attrCom.SetHp(tm.hp);
            attrCom.SetHpMax(tm.hp);
            attrCom.SetMoveSpeed(tm.moveSpeed);

            // - Skillor
            var skillorCom = role.skillorSlotComponent;
            var skillorTMArray = tm.skillorSoArray;

            if (skillorTMArray != null) {
                for (int i = 0; i < skillorTMArray.Length; i += 1) {
                    var skillSo = skillorTMArray[i];
                    var skillor = Skillor_Create(assetsCore, templateCore, idService, skillSo.tm.typeID);
                    skillorCom.Add(skillor);
                }
            }


            return role;
        }

        #region Bullet
        public static BulletEntity Bullet_Create(AssetsModule assetsCore, TemplateMoudle templateCore,
         IDService idService, int typeID, Vector3 pos, Vector3 dir) {
            bool has = templateCore.Bullet_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Bullet_Create: tm is null" + typeID);
                return null;
            }

            // - go
            GameObject prefab = assetsCore.Entity_GetBullet();
            GameObject go = GameObject.Instantiate(prefab);
            BulletEntity bullet = go.GetComponent<BulletEntity>();
            bullet.Reuse();

            // - Frames
            // var frameTMs = tm.frames;
            // if (frameTMs != null) {
            //     for (int i = 0; i < frameTMs.Length; i += 1) {
            //         var frameTM = frameTMs[i];
            //         var frame = BulletFrame_Create(poolService, frameTM);
            //         bullet.frames.Add(frame);
            //     }
            // }
            bullet.frameIndex = 0;

            //  - mod
            var mod = BulletMod_Creat(tm, bullet.bodyRoot);

            bullet.transform.position = new Vector3(-4, 0, 0);
            bullet.idSig = new IDSignature(EntityType.Bullet, idService.PickBulletID());
            bullet.typeID = typeID;
            bullet.type = tm.type;
            bullet.hp = tm.hp;
            bullet.damage = tm.attackDamage;
            bullet.hitMaxTimes = tm.hitTimes;
            bullet.hitTimes = tm.hitTimes;
            bullet.isBomb = tm.isBomb;
            bullet.moveSpeed = tm.moveSpeed;
            bullet.isHaveContinueDamege = tm.isHaveContinueDamege;
            bullet.continueDamege = tm.continueDamege;
            bullet.isHaveDamageRange = tm.isHaveDamageRange;
            bullet.damageRange = tm.damageRange;


            bullet.Inject(mod);
            bullet.Ctor();

            bullet.TF_SetPosition(pos);
            bullet.TF_SetRotation(dir);


            return bullet;
        }

        #endregion

        public static MapEntity Map_Create(AssetsModule assetsCore, IDService idService, GameObject mapPrefab) {
            GameObject go = GameObject.Instantiate(mapPrefab);
            MapEntity map = go.GetComponent<MapEntity>();
            map.mapID = idService.PickMapID();
            map.Ctor();
            return map;

        }

        #region Stuff
        public static StuffEntity Stuff_Create(AssetsModule assetsCore, TemplateMoudle templateCore, IDService iDService, int typeID) {
            bool has = templateCore.Stuff_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Stuff_Create: tm is null" + typeID);
                return null;
            }
            GameObject prefab = assetsCore.Entity_GetStuff();
            Debug.Assert(prefab != null, "prefab != null");
            GameObject go = GameObject.Instantiate(prefab);
            StuffEntity stuff = go.GetComponent<StuffEntity>();

            // - mod
            var mod = StuffMod_Create(tm, stuff.bodyRoot);

            stuff.idSig = new IDSignature(EntityType.Stuff, iDService.PickStuffID());
            stuff.typeID = typeID;
            stuff.typeName = tm.typeName;
            stuff.cd = tm.cd;
            stuff.cdMax = tm.cdMax;
            stuff.damage = tm.damage;
            stuff.isHaveContinueDamege = tm.isHaveContinueDamege;
            stuff.continueDamege = tm.continueDamege;
            stuff.damageRange = tm.damageRange;
            stuff.Ctor();
            return stuff;
        }
        #endregion

        #region Model


        // skill
        public static SkillModel Skillor_Create(AssetsModule assetsCore, TemplateMoudle templateCore, IDService idService, int typeID) {

            bool has = templateCore.Skillor_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Skillor_Create: tm is null" + typeID);
                return null;
            }

            SkillModel model = new SkillModel();

            model.idSig = new IDSignature(EntityType.Skillor, idService.PickSkillorID());
            model.typeID = typeID;
            model.cd = tm.cd;
            model.cdMax = tm.cd;
            model.restUseTimes = tm.restUseTimes;
            model.maxUseTimes = tm.maxUseTimes;
            model.castKeys = tm.castKeys;

            model.Ctor();

            model.frames = new SkillFrameModel[tm.actionTMs.Length];
            for (int i = 0; i < tm.actionTMs.Length; i += 1) {
                var actionTM = tm.actionTMs[i];
                // new一个新的SkillFrameModel并且赋值   
                var actionModel = new SkillFrameModel().FromTM(typeID, actionTM);
                model.frames[i] = actionModel;
            }

            return model;
        }

        // Card
        public static CardModel Card_Create(AssetsModule assetsCore, TemplateMoudle templateCore, int typeID) {
            bool has = templateCore.Card_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Card_Create: tm is null" + typeID);
                return null;
            }

            CardModel model = new CardModel();

            model.typeID = typeID;

            // var skillorTMArray = tm.skillorSoArray;

            // if (skillorTMArray != null) {
            //     for (int i = 0; i < skillorTMArray.Length; i += 1) {
            //         var skillSo = skillorTMArray[i];
            //         var skillor = Skillor_Create(assetsCore, templateCore, idService, skillSo.tm.typeID);
            //         skillorCom.Add(skillor);
            //     }
            // }

            var roleSoArr = tm.roleSoArray;
            if (roleSoArr != null) {
                for (int i = 0; i < roleSoArr.Count; i += 1) {
                    var roleSo = roleSoArr[i];
                    model.roleTMs.Add(roleSo.tm);
                }
            }

            if (tm.isTrain == true) { return model; }

            var bulletAgrSoArr = tm.bulletAgrSoArray;
            if (bulletAgrSoArr != null) {
                for (int i = 0; i < bulletAgrSoArr.Count; i += 1) {
                    var bulletSo = bulletAgrSoArr[i];
                    model.bulletAgrTMs.Add(bulletSo.tm);
                }
            }

            var bulletIndSoArr = tm.bulletIndSoArray;
            if (bulletIndSoArr != null) {
                for (int i = 0; i < bulletIndSoArr.Count; i += 1) {
                    var bulletSo = bulletIndSoArr[i];
                    model.bulletIndTMs.Add(bulletSo.tm);
                }
            }

            var bulletSciSoArr = tm.bulletSciSoArray;
            if (bulletSciSoArr != null) {
                for (int i = 0; i < bulletSciSoArr.Count; i += 1) {
                    var bulletSo = bulletSciSoArr[i];
                    model.bulletSciTMs.Add(bulletSo.tm);
                }
            }

            var roleEnemySoArr = tm.roleEnemySoArray;
            if (roleEnemySoArr != null) {
                for (int i = 0; i < roleEnemySoArr.Count; i += 1) {
                    var roleSo = roleEnemySoArr[i];
                    model.roleEnemyTMs.Add(roleSo.tm);
                }
            }

            return model;
        }
        #endregion

        public static CaveEntity Tower_Create(AssetsModule assetsCore, TemplateMoudle templateCore, IDService idService, int typeID, Vector2 pos) {
            bool has = templateCore.Tower_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Tower_Create: tm is null" + typeID);
                return null;
            }

            GameObject prefab = assetsCore.Entity_GetTower();
            GameObject go = GameObject.Instantiate(prefab);
            CaveEntity tower = go.GetComponent<CaveEntity>();


            tower.idSig = new IDSignature(EntityType.Tower, idService.PickTowerID());
            tower.typeID = typeID;

            tower.TF_SetPos(pos);
            tower.Ctor();

            TMToCaveEntity(tower, tm);

            return tower;
        }

        public static CaveEntity Tower_CreateBySpawner(AssetsModule assetsCore, TemplateMoudle templateCore, IDService idService, CaveSpawnerTM spawnerTM) {
            int typeID = spawnerTM.so.tm.typeID;

            bool has = templateCore.Tower_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Tower_Create: tm is null" + typeID);
                return null;
            }

            GameObject prefab = assetsCore.Entity_GetTower();
            GameObject go = GameObject.Instantiate(prefab);
            CaveEntity tower = go.GetComponent<CaveEntity>();


            tower.idSig = new IDSignature(EntityType.Tower, idService.PickTowerID());
            tower.typeID = typeID;

            tower.TF_SetPos(spawnerTM.position);
            tower.TF_SetRot(spawnerTM.rotation);

            tower.Ctor();


            TMToCaveEntity(tower, tm);

            return tower;
        }

        static void TMToCaveEntity(CaveEntity entity, CaveTM tm) {
        }


        #region Mod

        static BulletMod BulletMod_Creat(BulletTM tm, Transform bodyRoot) {
            var mod = GameObject.Instantiate(tm.modPrefab, bodyRoot).GetComponent<BulletMod>();
            mod.Ctor();
            return mod;
        }

        static RoleMod RoleMod_Create(RoleTM tm, Transform bodyRoot) {
            var mod = GameObject.Instantiate(tm.modPrefab, bodyRoot).GetComponent<RoleMod>();
            mod.transform.position = new Vector3(0, 0, 0);
            mod.Ctor();
            return mod;
        }

        static StuffMod StuffMod_Create(StuffTM tm, Transform bodyRoot) {
            var mod = GameObject.Instantiate(tm.modPrefab, bodyRoot).GetComponent<StuffMod>();
            mod.Ctor();
            return mod;
        }
        #endregion
    }
}