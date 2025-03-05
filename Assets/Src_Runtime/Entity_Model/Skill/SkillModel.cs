using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public class SkillModel {

        public IDSignature idSig;
        public int typeID;
        public string name;

        public InputKeyFlag castKeys;

        public SkillFrameModel[] frames;
        int frameIndex;
        public int FrameIndex => frameIndex;

        public int restUseTimes;//剩余使用次数
        public int maxUseTimes;//最大使用次数

        public float cd;
        public float cdMax;

        public int mpCost;
        public int mpCostMax;

        public SkillModel() {
            typeID = 1;
            frameIndex = 0;
        }

        public void Set_InputCastKeyWeap(int skillIndex) {
            int index = skillIndex + 1;
            if (index == 1) {
                castKeys = InputKeyFlag.Weap1;
            } else if (index == 2) {
                castKeys = InputKeyFlag.Weap2;
            } else if (index == 3) {
                castKeys = InputKeyFlag.Weap3;
            } else if (index == 4) {
                castKeys = InputKeyFlag.Weap4;
            }
        }

        public void Set_InputCastKey(InputKeyFlag key) {
            castKeys = key;
        }

        public void Ctor() {

        }
        public void Reuse() {
            Reset();
        }

        public void Reset() {
            if (frames != null) {
                for (int i = 0; i < frames.Length; i++) {
                    frames[i].Reset();
                }
            }
        }

        public bool isFitInputKey(InputKeyFlag InputKey) {
            if (InputKey == castKeys) {
                return true;
            }
            return false;
        }


    }
}