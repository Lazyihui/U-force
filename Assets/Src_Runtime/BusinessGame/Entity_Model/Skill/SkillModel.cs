using System;
using System.Collections;
using UnityEngine;

namespace UF {
    public class SkillModel {

        public IDSignature idSig;
        public int typeID;
        public string name;

        public SkillFrameModel[] frames;

        public SkillModel() {
        }

        public void Reset() {
            if (frames != null) {
                for (int i = 0; i < frames.Length; i++) {
                    frames[i].Reset();
                }
            }
        }
        

    }
}