using System;
using UnityEngine;
using TriInspector;

namespace UF {

    [Serializable]
    public class BulletFrameTM {

        [InfoBox("==== Life Cycle ====")]

        [Title("什么阶段可以运行")]
        public LoopTM loopTM;


        [Title("子弹行为")]
        public bool hasBulletBehaviour;
        [ShowIf(nameof(hasBulletBehaviour))] public BulletBehaviourTM bulletBehaviourTM;

    }
}   