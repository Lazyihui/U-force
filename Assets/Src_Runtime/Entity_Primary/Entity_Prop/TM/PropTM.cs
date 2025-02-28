using System;
using UnityEngine;
using TriInspector;

namespace UF {
    [Serializable]
    public class PropTM {
        public int typeID;
        public string typeName;

        public PropType type;
        public PropMoveType moveType;

        public Sprite sprite;
        public string feature; // 特征/详情

        public bool isPlayerProp;

        [Title("Locomotion")]
        public PropMod modPrefab;
        public float moveSpeed;
        public int times;
    }
}