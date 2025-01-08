using System;
using UnityEngine;

namespace UF {
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

        
        public void Ctor() {

        }


    }
}