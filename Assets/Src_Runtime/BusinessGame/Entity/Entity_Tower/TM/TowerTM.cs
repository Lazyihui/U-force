using System;
using UnityEngine;

namespace UF {

    [Serializable]
    public class TowerTM {

        public string typeName;
        public int typeID;

        public float interval;
        public float maintain;
        public float cdMax;
        public float cd;

        // -sprite
        public Sprite sprite;

    }
}