using System;
using System.Collections;
using UnityEngine;

namespace UF {

    [Serializable]
    public class SkillTM {
        public int typeID;
        public string typeName;

        public float cd;

        [Header("技能帧")]
        public SkillFrameTM[] skillFrameTMs;

        public void Transtion() {
            if (skillFrameTMs == null || skillFrameTMs.Length == 0) {
                Debug.LogError("SkillTM Transition: skillFramesTM is null");
                return;
            }
            for (int i = 0; i < skillFrameTMs.Length; i++) {
                skillFrameTMs[i].Transtion();
            }

        }

    }
}