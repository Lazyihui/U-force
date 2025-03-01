using System;
using System.Collections.Generic;
using UnityEngine;


namespace UF {

    public class StuffEntity : MonoBehaviour {

        [SerializeField] public Transform bodyRoot;
        public IDSignature idSig;
        public int typeID;
        public string typeName;


        // 属性内容
        public float cd;
        public float cdMax;

        public float damage;

        public bool isHaveContinueDamege;//是否有持续伤害
        public float continueDamege;//持续伤害

        public float damageRange;//伤害范围

        public bool isHavePenetrate;//是否有穿透不知道这个是干什么用的

        public void Ctor() {

        }

        public void TearDown() {
            gameObject.SetActive(false);
        }

        public void Reuse() {
            gameObject.SetActive(true);
        }

        public void TF_SetPos(Vector3 pos) {
            transform.position = pos;
        }
        public void TF_SetRot(Vector3 rot) {
            transform.eulerAngles = rot;
        }

        public Vector3 TF_GetPos() {
            return transform.position;
        }
    }
}