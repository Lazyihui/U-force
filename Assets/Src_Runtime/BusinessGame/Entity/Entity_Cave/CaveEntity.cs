using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    //生成怪物的基地（可以没图片就是一个看不见的东西） 
    public class CaveEntity : MonoBehaviour {
        public IDSignature idSig;

        public int typeID;

        // 生成怪物的间隔
        public float interval;
        public float intervalTimer;

        public int spawnCount;
        public int spawnMaxCount;

        public float cd;
        public float cdMax;

        public void Ctor() {
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

    }
}
