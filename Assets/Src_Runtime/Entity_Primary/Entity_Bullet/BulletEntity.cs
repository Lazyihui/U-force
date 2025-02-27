using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UF {

    // 没写的TODO:状态机
    public class BulletEntity : MonoBehaviour {

        public int typeID;
        public IDSignature idSig;//自身唯一标识
        public IDSignature belongIDSig;//属于谁的 玩家 还是 敌人
        public BulletMoveType type;
        public BulletSkillType skillType;

        [SerializeField] public Transform bodyRoot; // 子弹生成的根节点        

        public BulletTragetComponent targetComponent;//暂时没用到

        // -life
        public int hp;
        public int damage;

        public int hitTimes;//击中次数 可以集中几下
        public int hitMaxTimes;//最大击中次数 是否有穿透(可以打几个敌人)

        public bool isHaveContinueDamege;//是否有持续伤害
        public float continueDamege;//持续伤害

        public bool isHaveDamageRange;//是否有伤害范围
        public float damageRange;//伤害范围

        // -Frames
        public List<BulletFrameMod> frames;
        public int frameIndex;

        // -Interaction
        public bool isBomb;

        // TODO:
        public int SkillTypeID; // 后面写技能系统的时候再加 

        // -rigidbody
        [SerializeField] Rigidbody2D rb;
        public Rigidbody2D Rb => rb;
        public float moveSpeed;


        public BulletMod mod;
        public Vector2 centerPos => this.transform.position;
        public Vector3 TF_GetPos() => this.transform.position;


        public bool IsDead() {
            bool isDead = hp <= 0;
            // TODO:L
            /* 这里还没弄懂
            isDead |= hitTimes < 0;
            isDead |= frameIndex >= frames.Count;
            */
            return isDead;
        }

        public void TearDown() {
            GameObject.Destroy(gameObject);
        }

        #region  Locomotion

        public void Loco_Shaped() {
            // 向右移动
            rb.velocity = new Vector2(1, 0) * moveSpeed;
        }

        public void Loco_ToLeft() {
            rb.velocity = new Vector2(-1, 0) * moveSpeed;
        }

        public void Move_Stop() {
            rb.velocity = Vector2.zero;
        }
        #endregion

        public void Ctor() {
            frames = new List<BulletFrameMod>();
            targetComponent = new BulletTragetComponent();

            moveSpeed *= 5;
        }

        public void Inject(BulletMod mod) {
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
            targetComponent.Release();
        }

        #region frames

        public bool Frame_TryGetCurrent(out BulletFrameMod frame) {
            if (frameIndex >= frames.Count) {
                frame = null;//如果超出边界就返回null
                return false;
            }
            frame = frames[frameIndex];//否则返回当前帧
            return true;
        }
        #endregion



    }
}