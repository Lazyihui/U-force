using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class SkillFrameModel {

        public LoopModel loopModel;

        // -移动方式
        public SkillMoveModel moveModel;

        // -Role Behaviour
        public bool hasRoleBehaviour;
        public RoleBehaviorModel roleBehaviour;

        public bool hasWorldBehaviour;
        public WorldBehaviourModel worldBehaviourModel;

        public SkillFrameModel() {
            this.loopModel = new LoopModel();
        }

        public void Reset() {
            loopModel.Reset();
        }

        public SkillFrameModel FromTM(int typeID, SkillFrameTM tm) {

            loopModel.FromTM(tm.loopTM);

            // moveModel = new SkillMoveModel();
            // moveModel.moveOffset = tm.moveTM.moveOffset;
            // moveModel.moveSpeed = tm.moveTM.moveSpeed;

            hasRoleBehaviour = tm.hasRoleBehaviour;
            if (hasRoleBehaviour) {
                roleBehaviour.FromTM(tm.roleBehaviourTM);
            }

            hasWorldBehaviour = tm.hasWorldBehaviour;
            if (hasWorldBehaviour) {
                worldBehaviourModel.FromTM(tm.worldBehaviourTM);
            }

            return this;
        }

    }
}