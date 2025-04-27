using System;
using UnityEngine;
using TriInspector;

namespace UF {
    [Serializable]
    public class LaserTM {
        public int typeID;
        public string typeName;

        [Title("基础属性")]
        public int damage;
        public float showTime;
        public float HideTime;
    }
}