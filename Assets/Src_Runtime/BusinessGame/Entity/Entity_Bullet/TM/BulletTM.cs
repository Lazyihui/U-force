using System;
using TriInspector;

namespace UF {

    [Serializable]
    public class BulletTM {
        public int typeID;
        public string typeName;
        public BulletType type;

        [Title("life")]
        public int hp;
        public int hitTimes;
        public int attackDamage;

        public bool isBomb;

        [Title("Locomotion")]
        public float moveSpeed;

        public BulletMod modPrefab;


    }
}