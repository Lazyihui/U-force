using System;
using UnityEngine;

namespace UF {

    public class RopeEntity : MonoBehaviour {

        public IDSignature idSig;

        public void Ctor() {

        }

        public void UnSpawn(RopeEntity entity) {
            Destroy(entity.gameObject);
        }
    }
}