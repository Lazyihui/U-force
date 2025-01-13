using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    // -技能槽
    public class RoleSkillSlotComponent {

        Dictionary<IDSignature, SkillModel> all;
        List<SkillModel> orderByInputTypeList; // 临时存放按照输入顺序排序的技能

        SkillModel[] tempOrderByInputType; // 临时存放按照输入类型排序的技能 还没用到

        public RoleSkillSlotComponent() {
            all = new Dictionary<IDSignature, SkillModel>();
            orderByInputTypeList = new List<SkillModel>();

            tempOrderByInputType = new SkillModel[10];

        }

        public void Release() {
            all.Clear();
            orderByInputTypeList.Clear();
        }

        public void Add(SkillModel skillor) {
            all.Add(skillor.idSig, skillor);
            orderByInputTypeList.Add(skillor);
        }

        public int TakeSkillsByFlushInput(out SkillModel[] result) {
            int count = 0;

            for (int i = 0; i < orderByInputTypeList.Count; i++) {
                var skill = orderByInputTypeList[i];
                if (skill.isFitFlushKey()) {
                    tempOrderByInputType[count] = skill;
                    count++;
                }
            }
            result = tempOrderByInputType;
            return count;
        }



    }
}