using System;
using UnityEngine;

namespace UF {

    [Serializable]
    public class CaveTM {

        public string typeName;
        public int typeID;

        public float interval;
        public int spawnCount;
        public float cdMax;
        public float cd;

        // -sprite
        public Sprite sprite;

    }
}