using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    // -技能槽
    public class RoleSkillSlotComponent {

        Dictionary<IDSignature, SkillModel> all;

        SkillModel[] tempOrderByInputType; // 临时存放按照输入类型排序的技能 还没用到

        public RoleSkillSlotComponent() {
            all = new Dictionary<IDSignature, SkillModel>();
            tempOrderByInputType = new SkillModel[10];
        }

        public void Release() {
            all.Clear();
        }

        public void Add(SkillModel skillor) {
            all.Add(skillor.idSig, skillor);
        }

        

    }
}