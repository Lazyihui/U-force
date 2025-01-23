using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public static class GameFactory {

        public static RoleEntity Role_CreateBySpawner(AssetsCore assetsCore, TemplateCore templateCore,
         IDService idService, Vector3 pos, int typeID) {

            bool has = templateCore.Role_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Role_Create: tm is null" + typeID);
                return null;
            }
            GameObject prefab = assetsCore.Entity_GetRole();
            GameObject go = GameObject.Instantiate(prefab);
            RoleEntity role = go.GetComponent<RoleEntity>();


            var mod = RoleMod_Create(typeID, templateCore, role.bodyRoot);

            role.idSig = new IDSignature(EntityType.Role, idService.PickRoleID());
            role.roleType = tm.roleType;

            role.Ctor();
            role.Inject(mod);



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


        public static BulletEntity Bullet_Create(AssetsCore assetsCore, TemplateCore templateCore,
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
            var mod = BulletMod_Creat(typeID, templateCore, bullet.bodyRoot);

            bullet.transform.position = new Vector3(-4, 0, 0);
            bullet.idSig = new IDSignature(EntityType.Bullet, idService.PickBulletID());
            bullet.typeID = typeID;
            bullet.type = tm.type;
            bullet.hp = tm.hp;
            bullet.hitMaxTimes = tm.hitTimes;
            bullet.hitTimes = tm.hitTimes;
            bullet.isBomb = tm.isBomb;
            bullet.moveSpeed = tm.moveSpeed;


            bullet.Inject(mod);
            bullet.Ctor();

            bullet.TF_SetPosition(pos);
            bullet.TF_SetRotation(dir);

            return bullet;
        }

        #region Skillor

        public static SkillModel Skillor_Create(AssetsCore assetsCore, TemplateCore templateCore, IDService idService, int typeID) {

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

        #endregion

        #region Mod

        static BulletMod BulletMod_Creat(int typeID, TemplateCore templateCore, Transform bodyRoot) {
            bool has = templateCore.Bullet_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Bullet_Create: tm is null" + typeID);
                return null;
            }
            var mod = GameObject.Instantiate(tm.modPrefab, bodyRoot).GetComponent<BulletMod>();
            mod.Ctor();
            return mod;
        }

        static RoleMod RoleMod_Create(int typeID, TemplateCore templateCore, Transform bodyRoot) {

            bool has = templateCore.Role_TryGet(typeID, out var tm);
            if (!has) {
                Debug.LogError("Role_Create: tm is null" + typeID);
                return null;
            }
            var mod = GameObject.Instantiate(tm.modPrefab, bodyRoot).GetComponent<RoleMod>();
            mod.Ctor();
            return mod;
        }
        #endregion
    }
}