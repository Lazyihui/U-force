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

        // public BulletType type; //暂时没用到

        [SerializeField] public Transform bodyRoot; // 子弹生成的根节点        

        public BulletTragetComponent targetComponent;

        // -life
        public int hp;

        // -hit
        public int hitTimes;
        public int hitMaxTimes;

        // -Frames
        public List<BulletFrameMod> frames;
        public int frameIndex;

        // -Interaction
        public bool isDisappeared;

        // public int SkillTypeID // 后面写技能系统的时候再加 

        // -rigidbody
        [SerializeField] Rigidbody2D rb;
        public Rigidbody2D Rb => rb;

        public BulletMod mod;
        public Vector2 centerPos => this.transform.position;


        public bool IsDead() {
            bool isDead = hp <= 0;
            /* 这里还没弄懂
            isDead |= hitTimes < 0;
            isDead |= frameIndex >= frames.Count;
            */
            return isDead;
        }

        // -Temp

        public void Ctor() {
            frames = new List<BulletFrameMod>();
            targetComponent = new BulletTragetComponent();
        }

        public void Inject(BulletMod mod) {
            this.mod = mod;
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