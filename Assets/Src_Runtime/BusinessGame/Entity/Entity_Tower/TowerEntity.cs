using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    //生成怪物的基地（可以没图片就是一个看不见的东西） 
    public class TowerEntity : MonoBehaviour {
        public IDSignature idSig;

        public int typeID;

        public float interval;
        public float intervalTimer;

        public void Ctor() {
            interval = 2;
            intervalTimer = 0;
        }

        public void TF_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        public void TF_SetRot(Vector3 rot) {
            transform.eulerAngles = rot;
        }

        public Vector2 GetPos() {
            return transform.position;
        }

        public bool AllowSpawn(float dt) {
            intervalTimer -= dt;
            if (intervalTimer <= 0) {
                intervalTimer = interval;
                return true;
            }
            return false;
        }
    }
}
