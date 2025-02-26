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

        public void UseProp1() // 玩家1使用道具
        {
            if (type != PropType.SceneProp) { return; }

            if (typeID == 1000) { // 陷害对手
                Debug.Log("玩家2变慢");
            } else if (typeID == 1100) { // 增加护盾
                Debug.Log("玩家1增加护盾");
            } else if (typeID == 1200) { // 增加血量
                Debug.Log("玩家1增加血量");
            } else if (typeID == 1300) { // 增加时间
                Debug.Log("增加时间");
            }
        }

        public void UseProp2() // 玩家2使用道具
        {
            if (type != PropType.SceneProp) { return; }

            if (typeID == 1000) { // 陷害对手
                Debug.Log("玩家1变慢");
            } else if (typeID == 1100) { // 增加护盾
                Debug.Log("玩家2增加护盾");
            } else if (typeID == 1200) { // 增加血量
                Debug.Log("玩家2增加血量");
            } else if (typeID == 1300) { // 增加时间
                Debug.Log("增加时间");
            }
        }
    }
}