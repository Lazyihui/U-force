using System;
using System.Collections;
using UnityEngine;
using TriInspector;

namespace UF {
    [Serializable]
    public class SkillFrameTM {
        // -移动方式
        [Title("Move")]
        public SkillMoveTM moveTM;//没用到

        // -Role Behaviour
        [Title("Role Behaviour")]
        public bool hasRoleBehaviour;
        public RoleBehaviourTM roleBehaviourTM;

        public void Transtion() {
            if (hasRoleBehaviour) {
                // roleBehaviourTM.Transtion();//没实现还不知道是干什么的
            }
        }

    }
}