using System;
using UnityEngine;


namespace UF {
    public class RoleCastingStateModel {
        public bool isEntering;
        public SkillModel skillor;

        public int frameIndex;

        public RoleCastingStateModel() {
            frameIndex = 0;
        }

    }
}