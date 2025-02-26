using System;
using TriInspector;
using UnityEngine;

namespace UF {
    public class PropEntity : MonoBehaviour {
        // Common
        public int typeID;
        public IDSignature idSig;

        public PropType type;
        public PropMoveType moveType;

        [SerializeField] public Transform bodyRoot; // 根节点

        // Loco
        public PropMod mod;

        public bool isPlayerProp;
        [ShowIf(nameof(isPlayerProp))] Rigidbody2D rb;

        public float moveSpeed;
        public int times; // 可使用次数

        public void Ctor() {
            rb = gameObject.GetComponent<Rigidbody2D>();
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

        public Vector3 TF_GetPos() {
            return transform.position;
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

        public void Move_Down() {
            rb.velocity = new Vector2(0, -1) * moveSpeed;
        }

        public void Move_Stop() {
            rb.velocity = Vector2.zero;
        }

        public int UseProp1() // 玩家1使用道具
        {
            if (type != PropType.SceneProp) { return 0; }

            switch (typeID) {
                case 1000:
                    return 1;
                case 1100:
                    return 2;
                case 1200:
                    return 3;
                case 1300:
                    return 4;
                default:
                    return 0;
            }
        }

        public int UseProp2() // 玩家2使用道具
        {
            if (type != PropType.SceneProp) { return 0; }

            switch (typeID) {
                case 1000:
                    return 1;
                case 1100:
                    return 2;
                case 1200:
                    return 3;
                case 1300:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}