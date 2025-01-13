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

        public Dictionary<int, SkillTM> skills;
        public AsyncOperationHandle skillPtr;

        public TemplateContext() {
            roles = new Dictionary<int, RoleTM>();
            bullets = new Dictionary<int, BulletTM>();
            skills = new Dictionary<int, SkillTM>();
        }


        public void Role_Add(RoleTM role) {
            roles.Add(role.typeID, role);
        }

        public void Bullet_Add(BulletTM bullet) {
            bullets.Add(bullet.typeID, bullet);
        }

        public void Skill_Add(SkillTM skill) {
            skills.Add(skill.typeID, skill);
        }

    }
}