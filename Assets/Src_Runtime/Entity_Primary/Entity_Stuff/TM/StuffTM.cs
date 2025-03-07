using System;
using UnityEngine;
using TriInspector;

namespace UF {

    [Serializable]
    public class StuffTM {
        public int typeID;
        public string typeName;

        [Title("Attribute")]
        public int weight; // 重量(计算抓取速度)

        public bool isEnergy;

        public bool isTimeStuff; // 时间道具
        [ShowIf(nameof(isTimeStuff))] public int addTime;

        public bool isAttackPoint; // 是否是攻击点
        [ShowIf(nameof(isAttackPoint))] public float DisAppearTimer;
        
        [Title("mod")]
        public StuffMod modPrefab;
        public Sprite sprite;
        public string description;

    }
}