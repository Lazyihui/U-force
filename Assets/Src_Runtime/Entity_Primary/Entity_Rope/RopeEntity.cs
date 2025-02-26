using System;
using UnityEngine;

namespace UF {

    public class RopeEntity : MonoBehaviour {

        public IDSignature idSig;
        public RopeStage state;

        [SerializeField] public GameObject rope;
        [SerializeField] public GameObject hook;

        public float stretchLength;
        public float stretchSpeed;
        public float shorteningSpeed;

        public float curLength;

        public Vector2 originalPos;
        public Vector2 targetPos;

        public void Ctor() {
            stretchLength = 30;
            stretchSpeed = 5;
            shorteningSpeed = 5;
            originalPos = rope.transform.position;
        }

        public void UnSpawn(RopeEntity entity) {
            Destroy(entity.gameObject);
        }

        public void EnterStretch() {
            Debug.Log("EnterStretch");
            state = RopeStage.Stretch;
        }

        public void EnterShortening() {
            Debug.Log("EnterShortening Enter");
            state = RopeStage.shortening;
        }

    }
}