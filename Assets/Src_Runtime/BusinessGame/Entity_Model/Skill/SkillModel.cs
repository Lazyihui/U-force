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
            typeID = 1;
        }

        public void Reset() {
            if (frames != null) {
                for (int i = 0; i < frames.Length; i++) {
                    frames[i].Reset();
                }
            }
        }

        public bool isFitFlushKey() {
            return true;
            // if (frames == null) {
            //     return false;
            // }

            // // for (int i = 0; i < frames.Length; i++) {
            // //     if (frames[i].inputType == InputType.Flush) {
            // //         return true;
            // //     }
            // // }

            // return false;
        }


    }
}