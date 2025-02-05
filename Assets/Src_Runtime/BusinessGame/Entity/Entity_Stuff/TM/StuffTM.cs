using System;
using TriInspector;

namespace UF {

    [Serializable]
    public class StuffTM {
        public int typeID;
        public string typeName;

        [Title("Attribute")]
        public float cd;
        public float cdMax;
        public float damage;

        public bool isHaveContinueDamege;
        public float continueDamege;

        public float damageRange;

        [Title("mod")]
        public StuffMod modPrefab;
        public string description;

    }
}