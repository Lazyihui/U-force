using System;

namespace UF {

    [Serializable]
    public class BulletTM {
        public int typeID;
        public string typeName;
        public BulletType type;

        public int hp;
        public int hitTimes;

        public bool isDisappeared;

        public BulletMod mod;

        
    }
}