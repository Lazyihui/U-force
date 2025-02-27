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
        public void Set_MoveTarget(Vector2 pos) {
            targetPos = pos;
        }

        public void Ctor() {
            stretchLength = 15;
            stretchSpeed = 5;
            shorteningSpeed = 5;
            originalPos = rope.transform.position;
        }

        public void TearDown(RopeEntity entity) {
            Destroy(entity.gameObject);
        }

        public void TF_Pos(Vector2 pos) {
            transform.position = pos;
        }

        public void Set_Pos(Vector2 pos) {
            rope.transform.position = pos;
        }

        public Vector2 MoveDir() {

            Vector2 moveDir = targetPos - (Vector2)hook.transform.position.normalized;

            // 绝对值
            Vector2 dir = new Vector2(Mathf.Abs(moveDir.x), Mathf.Abs(moveDir.y));
            return -dir;
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