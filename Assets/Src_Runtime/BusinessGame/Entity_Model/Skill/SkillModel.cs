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

        public int restUseTimes;

        public int cd;
        public int cdMax;

        public int mpCost;
        public int mpCostMax;

        public SkillModel() {
            typeID = 1;
            frameIndex = 0;
        }

        public void Reset() {
            if (frames != null) {
                for (int i = 0; i < frames.Length; i++) {
                    frames[i].Reset();
                }
            }
        }

        public bool isFitFlushKey(InputKeyFlag InputKey) {
            if (InputKey == castKeys) {
                return true;
            }
            return false;
        }


    }
}