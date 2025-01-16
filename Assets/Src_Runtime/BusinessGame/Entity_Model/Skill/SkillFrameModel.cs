using System;
using System.Collections;
using UnityEngine;

namespace UF {

    public class SkillFrameModel {

        // -移动方式
        public SkillMoveModel moveModel;

        // -Role Behaviour
        public bool hasRoleBehaviour;
        public RoleBehaviorModel roleBehaviour;

        public void Reset() {
            
        }

        public SkillFrameModel FromTM(int typeID, SkillFrameTM tm) {

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