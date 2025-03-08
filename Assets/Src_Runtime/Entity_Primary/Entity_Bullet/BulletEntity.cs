using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace UF {

    // 没写的TODO:状态机
    public class BulletEntity : MonoBehaviour {

        public int typeID;
        public IDSignature idSig;//自身唯一标识
        public IDSignature belongIDSig;//属于谁的 玩家 还是 敌人
        public BulletMoveType moveType;
        public void Set_MoveType(BulletMoveType moveType) => this.moveType = moveType;
        public IDSignature touchRoleIDSig;//碰撞到的角色的ID
        [SerializeField] public Transform bodyRoot; // 子弹生成的根节点        

        // -life
        public int hp;
        public int damage;

        public int hitTimes;//击中次数 可以集中几下

        public bool isHaveContinueDamege;//是否有持续伤害
        public int continueDamege;//持续伤害
        public float damageInterval;//伤害间隔
        public float damageTimer;//伤害计时器

        // 特性
        public bool isMakeBulletAppear;//是否生使子弹出现
        public bool isDestotyALine;//是否销毁一行

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

        // 判断hitTime
        public bool isHit;

        // 使击中目标变换Move方式(位置)效果参数
        public bool isChangeLine;
        public Vector2 changeLineDir;
        public int LineNum; // 变换的行数
        public float LineLength; // 每行长度

        public bool isRepelled; // 击退
        public float repeLength; // 击退距离

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
            // 向右移动
            rb.velocity = new Vector2(1, 0) * moveSpeed;
        }

        public void Loco_ToLeft() {
            rb.velocity = new Vector2(-1, 0) * moveSpeed;
        }

        public void Move_Stop() {
            rb.velocity = Vector2.zero;
        }

        public void Loco_MoveDir(Vector2 dir) {
            rb.velocity = dir * moveSpeed / 10;
        }

        #endregion

        public void Ctor() {
            frames = new List<BulletFrameModel>();

            isHit = false;
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

        #region Gizmos
        void OnDrawGizmos() {
            if (!isHaveContinueDamege) {
                return;
            }
            int segments = 320;   // 圆的线段数量，越多越圆滑
            Gizmos.color = Color.green;
            float angle = 0;
            var center = this.transform.position;
            Vector3 lastPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * damageRange;

            for (int i = 0; i < segments; i += 10) {
                angle = (i / (float)segments) * 2 * Mathf.PI;
                Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * damageRange;
                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }

        }

        #endregion 

    }
}