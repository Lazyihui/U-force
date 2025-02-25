using System;
using TriInspector;
using UnityEngine;

namespace UF {
    public class PropEntity : MonoBehaviour {
        public int typeID;
        public IDSignature idSig;

        public PropType type;

        [SerializeField] public Transform bodyRoot; // 根节点

        public PropMod mod;

        public bool isPlayerProp;
        [ShowIf(nameof(isPlayerProp))][SerializeField] Rigidbody2D rb;

        public void Ctor() {

        }

        public void Inject(PropMod mod) {
            this.mod = mod;
        }

        public void TF_SetPosition(Vector3 pos) {
            transform.position = pos;
        }

        public void TF_SetRotation(Vector3 rot) {
            transform.eulerAngles = rot;
        }

        public void Reuse() {
            gameObject.SetActive(true);
        }

        public void Release() {
            gameObject.SetActive(false);
        }

        public void TearDown() {
            Destroy(gameObject);
        }
    }
}