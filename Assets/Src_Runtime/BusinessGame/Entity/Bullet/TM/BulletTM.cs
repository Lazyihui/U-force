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

        public bool isBomb;

        public BulletMod modPrefab;


    }
}