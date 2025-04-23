using System;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    // -技能槽
    public class RoleSkillSlotComponent {

        public Dictionary<IDSignature, SkillModel> all;
        List<SkillModel> orderByInputTypeList; // 临时存放按照输入顺序排序的技能

        SkillModel[] tempOrderByInputType; // 临时存放按照输入类型排序的技能 还没用到

        SkillModel currentSkill;
        public SkillModel CurrentSkill => currentSkill;


        public RoleSkillSlotComponent() {
            all = new Dictionary<IDSignature, SkillModel>();
            orderByInputTypeList = new List<SkillModel>();

            tempOrderByInputType = new SkillModel[10];
            temp = new SkillModel[10];

        }

        public void Release() {
            all.Clear();
            orderByInputTypeList.Clear();
            currentSkill = null;
        }

        public void Add(SkillModel skillor) {
            all.Add(skillor.idSig, skillor);
            orderByInputTypeList.Add(skillor);
        }

        // 获取技能槽中第一个技能
        public SkillModel GetSkillModel() {
            if (all.Count == 0) {
                return null;
            }
            foreach (var skill in all.Values) {
                currentSkill = skill;
                return skill;
            }
            return null;
        }

        SkillModel[] temp;
        public int TakeAll(out SkillModel[] result) {
            if (all.Count > temp.Length) {
                temp = new SkillModel[all.Count];
            }
            all.Values.CopyTo(temp, 0);
            result = temp;
            return all.Count;
        }

        // 获取技能槽中第一个技能
        public int TakeSkillsByFlushInput(out SkillModel[] result, InputKeyFlag inputKeys) {
            int count = 0;

            for (int i = 0; i < orderByInputTypeList.Count; i++) {
                var skill = orderByInputTypeList[i];
                
                if (skill.isFitInputKey(inputKeys)) {
                    tempOrderByInputType[count] = skill;
                    currentSkill = skill;
                    count++;
                }
            }
            result = tempOrderByInputType;
            return count;
        }

    }
}