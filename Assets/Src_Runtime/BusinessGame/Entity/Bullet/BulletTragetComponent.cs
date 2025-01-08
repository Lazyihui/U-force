using System;
using UnityEngine;

namespace UF {

    [Serializable]
    public class BulletTragetComponent {

        public bool isFound;
        public IDSignature foundTargetUniqueSig;
        public Vector2 foundTargetOriginPos;

        public void Release() {
            isFound = false;
            foundTargetUniqueSig = new IDSignature(EntityType.None, 0);
            foundTargetOriginPos = Vector2.zero;
        }

    }

}