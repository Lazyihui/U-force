using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UF.TemplateInternal {

    public class TemplateContext {

        public Dictionary<int, RoleTM> roles;
        public AsyncOperationHandle rolePtr;

        public Dictionary<int, BulletTM> bullets;
        public AsyncOperationHandle bulletPtr;

        public TemplateContext() {
            roles = new Dictionary<int, RoleTM>();
            bullets = new Dictionary<int, BulletTM>();
        }


        public void Role_Add(RoleTM role) {
            roles.Add(role.typeID, role);
        }

    }
}