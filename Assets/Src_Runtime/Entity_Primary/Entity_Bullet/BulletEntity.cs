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

        public IDSignature touchRoleIDSig;//碰撞到的角色的ID

        [SerializeField] public Transform bodyRoot; // 子弹生成的根节点        


        // -life
        public int hp;
        public int damage;

        public int hitTimes;//击中次数 可以集中几下

        public int continueDamege;//持续伤害
        public float damageInterval;//伤害间隔
        public float damageTimer;//伤害计时器

        public bool isMakeBulletAppear;//是否生使子弹出现
        // temp bool
        public bool isStartContinueDamege;//是否开始持续伤害
        public bool isStartBoomDamege;//是否开始爆炸伤害

        public float damageRange;//伤害范围

        // -Frames
        public List<BulletFrameModel> frames;
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
            bool isDead = hitTimes <= 0;
            // Debug.Log(hp + frameIndex + hitMaxTimes);
            isDead |= frameIndex >= frames.Count;
            return isDead;
        }

        public void TearDown() {
            GameObject.Destroy(gameObject);
        }

        #region  Locomotion

        public void Loco_Shaped() {
            moveSpeed *= 5;
            // 向右移动
            rb.velocity = new Vector2(1, 0) * moveSpeed;
        }

        public void Loco_ToLeft() {
            moveSpeed *= 5;
            rb.velocity = new Vector2(-1, 0) * moveSpeed;
        }

        public void Move_Stop() {
            rb.velocity = Vector2.zero;
        }
        #endregion

        public void Ctor() {
            frames = new List<BulletFrameModel>();
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
        }

        #region frames

        public bool Frame_TryGetCurrent(out BulletFrameModel frame) {
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