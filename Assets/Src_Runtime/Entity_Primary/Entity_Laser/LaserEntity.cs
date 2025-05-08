using System;
using UF;
using UnityEngine;

namespace UF {
    public class LaserEntity : MonoBehaviour {
        [HideInInspector] public int typeID;
        [HideInInspector] public IDSignature idSig;
        [HideInInspector] public LaserState state;

        public LineRenderer laserWarning; // 预警, 没伤害
        public LineRenderer laser; // 激光, 有伤害

        [HideInInspector] public int damage;

        [HideInInspector] public float showTime; // 从预警到显示激光的时间
        [HideInInspector] public float showTimer;

        [HideInInspector] public float HideTime; // 从显示激光到消失的时间
        [HideInInspector] public float HideTimer;

        [HideInInspector] public Vector2 startPos;
        [HideInInspector] public Vector2 endPos;

        [HideInInspector] public bool isHit; // 是否击中过

        public void Ctor() {
            state = LaserState.LaserWarning;

            isHit = false;

            showTimer = 0;
            HideTimer = 0;
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void TF_SetPos(Vector2 startPos, Vector2 endPos) {
            this.startPos = startPos;
            this.endPos = endPos;

            ShowLaserWarning();
        }

        public void ShowLaserWarning() {
            laserWarning.SetPosition(0, startPos);
            laserWarning.SetPosition(1, endPos);
        }

        public void ShowLaser() {
            laser.SetPosition(0, startPos);
            laser.SetPosition(1, endPos);
            laserWarning.gameObject.SetActive(false);
            laser.gameObject.SetActive(true);
        }
    }
}