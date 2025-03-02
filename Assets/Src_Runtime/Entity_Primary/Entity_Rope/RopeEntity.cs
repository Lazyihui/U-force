using System;
using UnityEngine;

namespace UF {

    public class RopeEntity : MonoBehaviour {

        public IDSignature idSig;
        public RopeStage state;

        [SerializeField] public GameObject hook;

        [SerializeField] LineRenderer line;

        public float outMoveSpeed; // 绳子伸长速度
        public float backMoveSpeed; // 绳子回收速度
        public float backWithStuffMoveSpeed; // 抓取物品绳子回收速度
        public float maxLength;

        public Vector2 spawnPos;

        public Vector2 originalPos; // 绳子起点
        public Vector2 targetPos; // 绳子终点

        public void Ctor() {
            outMoveSpeed = 10;
            backMoveSpeed = 50;
            maxLength = 30;
            spawnPos = hook.transform.position;

            // 防止一开始闪烁一下
            originalPos = spawnPos;
            targetPos = spawnPos;
        }

        public void TearDown(RopeEntity entity) {
            Destroy(entity.gameObject);
        }

        public void EnterStretch() {
            Debug.Log("EnterStretch");
            state = RopeStage.Stretch;
        }

        public void EnterShortening() {
            Debug.Log("EnterShortening Enter");
            state = RopeStage.Shortening;
        }

        public void EnterShorteningWithStuff() {
            Debug.Log("EnterShorteningWithStuff");
            state = RopeStage.ShorteningWithStuff;
        }

        public void TF_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        public void Set_OriginalPos(Vector2 pos) {
            originalPos = pos;
        }

        public void Set_TargetPos() {
            targetPos = hook.transform.position;
        }

        public void Set_Line() {
            line.SetPosition(0, originalPos);
            line.SetPosition(1, targetPos);
        }

        public void Set_HookPos(Vector2 pos)
        {
            hook.transform.position = pos;
        }
    }
}