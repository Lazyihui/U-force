using System;
using UnityEngine;

namespace UF {
    public class BulletFrameModel {

        public LoopModel loopModel;

        public bool hasBulletBehaviour;
        public BulletBehaviourModel bulletBehaviourModel;

        public bool hasWorldBehaviour;
        public WorldBehaviourModel worldBehaviourModel;


        public BulletFrameModel() {
            this.loopModel = new LoopModel();
        }

        public void Reset() {
            loopModel.Reset();
        }

        public void FromTM(in BulletFrameTM tm) {

            loopModel.FromTM(tm.loopTM);

            hasBulletBehaviour = tm.hasBulletBehaviour;
            if (hasBulletBehaviour) {
                bulletBehaviourModel.FormTM(tm.bulletBehaviourTM);
            }

            hasWorldBehaviour = tm.hasWorldBehaviour;
            if (hasWorldBehaviour) {
                worldBehaviourModel.FormTM(tm.worldBehaviourTM);
            }

        }


    }
}