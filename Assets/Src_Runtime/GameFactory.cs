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

            entity.idSig = new IDSignature(EntityType.Role, idService.PickRoleID());
            entity.roleType = tm.roleType;

            entity.Ctor();
            return entity;
        }

        public static BulletEntity Bullet_Create(AssetsCore assetsCore, TemplateCore templateCore,
         IDService idService, Vector3 pos, int typeID) {
            BulletTM tm;
            bool has = templateCore.Bullet_TryGet(typeID, out tm);
            if (!has) {
                Debug.LogError("Bullet_Create: tm is null" + typeID);
                return null;
            }

            GameObject prefab = assetsCore.Entity_GetBullet();
            GameObject go = GameObject.Instantiate(prefab);
            BulletEntity entity = go.GetComponent<BulletEntity>();

            entity.idSig = new IDSignature(EntityType.Bullet, idService.PickBulletID());
            entity.typeID = typeID;
            entity.hp = tm.hp;
            entity.hitMaxTimes = tm.hitTimes;
            entity.hitTimes = tm.hitTimes;
            // entity.frames = tm.frames;
            entity.frameIndex = 0;
            entity.isDisappeared = tm.isDisappeared;
            
            entity.Ctor();

            return entity;
        }
    }
}