using System;
using System.Collections;
using UnityEngine;
using TriInspector;

namespace UF {

    [Serializable]
    public class SkillorTM {
        public int typeID;
        public string typeName;

        public float cd;

        public int restUseTimes;
        public int maxUseTimes;

        

        [Header("技能帧")]
        public SkillFrameTM[] actionTMs;

        public void Transtion() {
            if (actionTMs == null || actionTMs.Length == 0) {
                Debug.LogError("SkillTM Transition: skillFramesTM is null");
                return;
            }
            for (int i = 0; i < actionTMs.Length; i++) {
                actionTMs[i].Transtion();
            }

        }

    }
}