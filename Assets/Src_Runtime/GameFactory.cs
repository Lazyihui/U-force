using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public static class GameFactory {

        public static RoleEntity Role_CreateBySpawner(AssetsCore assetsCore, TemplateCore templateCore,
         IDService idService, Vector3 pos, int typeID) {
            RoleTM tm;
            bool has = templateCore.Role_TryGet(typeID, out tm);
            if (!has) {
                Debug.LogError("Role_Create: tm is null" + typeID);
                return null;
            }
            GameObject prefab = assetsCore.Entity_GetRole();
            GameObject go = GameObject.Instantiate(prefab);
            RoleEntity entity = go.GetComponent<RoleEntity>();


            var mod = RoleMod_Create(typeID, assetsCore, entity.bodyRoot);

            entity.idSig = new IDSignature(EntityType.Role, idService.PickRoleID());
            entity.roleType = tm.roleType;

            entity.Ctor();
            return entity;
        }

        public static BulletEntity Bullet_Create(AssetsCore assetsCore, TemplateCore templateCore,
         IDService idService, int typeID) {
            BulletTM tm;
            bool has = templateCore.Bullet_TryGet(typeID, out tm);
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
            var mod = BulletMod_Creat(typeID, assetsCore, bullet.bodyRoot);

            bullet.transform.position = new Vector3(-4, 0, 0);
            bullet.idSig = new IDSignature(EntityType.Bullet, idService.PickBulletID());
            bullet.typeID = typeID;
            bullet.hp = tm.hp;
            bullet.hitMaxTimes = tm.hitTimes;
            bullet.hitTimes = tm.hitTimes;
            bullet.isBomb = tm.isBomb;

            bullet.Ctor();


            return bullet;
        }

        #region BulletMod

        static BulletMod BulletMod_Creat(int typeID, AssetsCore assetsCore, Transform bodyRoot) {

            GameObject go = assetsCore.Entity_GetBulletMod(typeID);
            go = GameObject.Instantiate(go, bodyRoot);
            BulletMod mod = go.GetComponent<BulletMod>();

            return mod;
        }

        #endregion

        static RoleMod RoleMod_Create(int typeID, AssetsCore assetsCore, Transform bodyRoot) {
            GameObject go = assetsCore.Entity_GetRoleMod(typeID);
            go = GameObject.Instantiate(go, bodyRoot);
            RoleMod mod = go.GetComponent<RoleMod>();
            return mod;
        }
    }
}