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

        public SkillFrameModel() {
            this.loopModel = new LoopModel();
        }

        public void Reset() {
            loopModel.Reset();
        }

        public SkillFrameModel FromTM(int typeID, SkillFrameTM tm) {

            this.loopModel.FromTM(tm.loopTM);

            // this.moveModel = new SkillMoveModel();
            // this.moveModel.moveOffset = tm.moveTM.moveOffset;
            // this.moveModel.moveSpeed = tm.moveTM.moveSpeed;

            this.hasRoleBehaviour = tm.hasRoleBehaviour;

            if (this.hasRoleBehaviour) {
                // 有角色行为就把角色行为配置到角色行为模型
                this.roleBehaviour.FromTM(tm.roleBehaviourTM);
            }

            return this;
        }

    }
}