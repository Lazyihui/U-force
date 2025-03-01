using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;


namespace UF {

    public class StuffEntity : MonoBehaviour {

        [SerializeField] public Transform bodyRoot;

        [SerializeField] public Rigidbody2D rb;
        public IDSignature idSig;
        public int typeID;
        public string typeName;

        // 属性内容
        public int weight;

        public bool isEnergy;

        public bool isTimeStuff;
        [ShowIf(nameof(isTimeStuff))] public int addTime;

        public void Ctor() {

        }

        public void TearDown() {
            Destroy(gameObject);
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
        public void TF_SetRot(Vector3 rot) {
            transform.eulerAngles = rot;
        }

        public Vector3 TF_GetPos() {
            return transform.position;
        }

        public void Fall() {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}