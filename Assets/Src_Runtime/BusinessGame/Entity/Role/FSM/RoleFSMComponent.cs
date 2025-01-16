using System;
using UnityEngine;


namespace UF {
    public class RoleFSMConponent {

        RoleFSMStatus status;
        public RoleFSMStatus Status => status;

        RoleCastingStateModel castingStateModel;
        public RoleCastingStateModel CastingStateModel => castingStateModel;


        public RoleFSMConponent() {
            status = RoleFSMStatus.Normal;
            castingStateModel = new RoleCastingStateModel();
        }

        public void Normal_Enter() {
            status = RoleFSMStatus.Normal;
        }

        public void Dead_Enter() {
            status = RoleFSMStatus.Dead;
        }

        public void Casting_Enter(SkillModel skill) {
            status = RoleFSMStatus.Casting;
            var stateModel = castingStateModel;
            stateModel.isEntering = true;
            stateModel.frameIndex = 0;
            stateModel.skillor = skill;
        }

    }
}