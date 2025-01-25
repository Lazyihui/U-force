using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UF.TemplateInternal;


namespace UF {
    public class TemplateCore {

        public TemplateContext ctx;

        public TemplateCore() {
            ctx = new TemplateContext();
        }

        public async Task LoadAll() {
            {
                AssetLabelReference labelReference = new AssetLabelReference();
                labelReference.labelString = "So_Role";
                var handle = Addressables.LoadAssetsAsync<RoleSo>(labelReference, null);
                var all = await handle.Task;
                foreach (var so in all) {
                    var tm = so.tm;
                    ctx.Role_Add(tm);
                }
                ctx.rolePtr = handle;
            }
            {
                AssetLabelReference labelReference = new AssetLabelReference();
                labelReference.labelString = "So_Bullet";
                var handle = Addressables.LoadAssetsAsync<BulletSo>(labelReference, null);
                var all = await handle.Task;
                foreach (var so in all) {
                    var tm = so.tm;
                    ctx.Bullet_Add(tm);
                }
                ctx.bulletPtr = handle;
            }
            {
                AssetLabelReference labelReference = new AssetLabelReference();
                labelReference.labelString = "So_Skill";
                var handle = Addressables.LoadAssetsAsync<SkillorSo>(labelReference, null);
                var all = await handle.Task;
                foreach (var so in all) {
                    var tm = so.tm;
                    ctx.Skill_Add(tm);
                }

                ctx.skillPtr = handle;
            }
            {
                AssetLabelReference labelReference = new AssetLabelReference();
                labelReference.labelString = "So_Tower";
                var handle = Addressables.LoadAssetsAsync<TowerSo>(labelReference, null);
                var all = await handle.Task;
                foreach (var so in all) {
                    var tm = so.tm;
                    ctx.Tower_Add(tm);
                }
                ctx.towerPtr = handle;
            }

        }
        // 找种类的Role
        public bool Role_TryGet(int typeID, out RoleTM role) {
            return ctx.roles.TryGetValue(typeID, out role);
        }

        public bool Bullet_TryGet(int typeID, out BulletTM bullet) {
            return ctx.bullets.TryGetValue(typeID, out bullet);
        }

        public bool Skillor_TryGet(int typeID, out SkillorTM skill) {
            return ctx.skills.TryGetValue(typeID, out skill);
        }

        public bool Tower_TryGet(int typeID, out TowerTM tower) {
            return ctx.towers.TryGetValue(typeID, out tower);
        }

        public void UnLoadAll() {
            if (ctx.rolePtr.IsValid()) {
                Addressables.Release(ctx.rolePtr);
            }
            if (ctx.bulletPtr.IsValid()) {
                Addressables.Release(ctx.bulletPtr);
            }
            if (ctx.skillPtr.IsValid()) {
                Addressables.Release(ctx.skillPtr);
            }
            if (ctx.towerPtr.IsValid()) {
                Addressables.Release(ctx.towerPtr);
            }
        }



    }
}