using System;
using UnityEngine;

namespace UF {

    public class RopeEntity : MonoBehaviour {

        public IDSignature idSig;
        public RopeStage state;

        [SerializeField] GameObject rope;
        [SerializeField] GameObject hook;

        public float stretchLength;
        public float stretchSpeed;
        public float shorteningSpeed;

        public float curLength;

        public Vector2 originalPos;
        public Vector2 targetPos;

        public void Ctor() {
            stretchLength = 10;
            stretchSpeed = 5;
            shorteningSpeed = 5;
            originalPos = rope.transform.position;
        }

        public void UnSpawn(RopeEntity entity) {
            Destroy(entity.gameObject);
        }

        public void EnterStretch() {
            state = RopeStage.Stretch;
        }

        public void EnterShortening() {
            state = RopeStage.shortening;
        }

        public void RestoreHookScale() {
            hook.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

    }
}